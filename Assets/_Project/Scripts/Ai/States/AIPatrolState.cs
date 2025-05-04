using UnityEngine;
using UnityEngine.Animations;

public class AIPatrolState : AIBaseState
{
// Removed unused variable 'currentTarget'
    float checkTimer;      // How long does the ai check one window in seconds
    int increment = 0;              // +1 every checked window
    int maxIncrement;        // max amount of windows dolly checks each patrol
    bool startPatrol = false; // 

    public override void enterState(AIStateManager ai) {
        Debug.Log("Dolly entered state 1");

        checkTimer = ai.getCheckRoomTime;
        maxIncrement = ai.getCheckWindowPerPatrol;   

        // Teleport to Patrol Spawn point
        if (!startPatrol) {
            ai.transform.position = new Vector3(ai.patrolSpawn.position.x, ai.patrolSpawn.position.y, ai.patrolSpawn.position.z);
            startPatrol = true;    
        }

        // Pick first target
        PickNewTarget(ai);
    }

    public override void onUpdate(AIStateManager ai) {
        if (ai.currentTargetWindow == null) return;

        if( increment >= maxIncrement) exitState(ai);
        else {
            // Move towards the current target
            ai.transform.position = Vector3.MoveTowards(
                ai.transform.position,
                ai.currentTargetWindow.position,
                Time.deltaTime * ai.moveSpeed // move speed
            );

            // Check if Dolly reached the target
            if (Vector3.Distance(ai.transform.position, ai.currentTargetWindow.position) < 0.1f) {
                
                // Checking current window

                increment++;
                
                ai.switchState(ai.scanState, false);
                
                // checkTimer -= Time.deltaTime;

            /*
            
            if eye of ai sees player

                ai.switchState(ai.attackState);

            */

    /*
                if (checkTimer <= 0.0f)
                {
                    increment++;
                    checkTimer = 5f;
                    if(increment >= maxIncrement){
                        exitState(ai);
                    }
                    PickNewTarget(ai);              // Pick a new target or switch state
                }
            */
            }
        }
        
    }

    private void PickNewTarget(AIStateManager ai) {
        // Pick a random room
        RoomContainer randomRoom = ai.rooms[Random.Range(0, ai.rooms.Length)];

        // Pick a random window in that room
        if (randomRoom.windowCount > 0) {
            ai.currentTargetWindow = randomRoom.windowAnchorPoints[Random.Range(0, randomRoom.windowAnchorPoints.Length)];
        }
    }

    public override void resetVariables(AIStateManager ai)
    {
        increment = 0;
        ai.currentTargetRoom = null;
        ai.currentTargetWindow = null;
        startPatrol = false;
    }

    public override void exitState(AIStateManager ai)
    {
        resetVariables(ai);
        ai.switchState(ai.idleState, false);
    }
}