using UnityEngine;

public class AIPatrolState : AIBaseState
{
    private Transform currentTarget; // Where Dolly is moving to
    float checkTimer = 5f;          // How long does the ai check one window in seconds
    int increment = 0;              // +1 every checked window
    int maxIncrement = 4;

    public override void enterState(AIStateManager ai) {
        Debug.Log("Dolly entered state 1");

        ai.transform.position = new Vector3(ai.patrolSpawn.position.x, ai.patrolSpawn.position.y, ai.patrolSpawn.position.z);

        // Pick first target
        PickNewTarget(ai);
    }

    public override void onUpdate(AIStateManager ai) {
        if (currentTarget == null) return;

        // Move towards the current target
        ai.transform.position = Vector3.MoveTowards(
            ai.transform.position,
            currentTarget.position,
            Time.deltaTime * 2f // move speed
        );

        // Check if Dolly reached the target
        if (Vector3.Distance(ai.transform.position, currentTarget.position) < 0.1f) {
            Debug.Log("checking current window");

            checkTimer -= Time.deltaTime;

        /*
        
        if eye of ai sees player

            ai.switchState(ai.attackState);

        */

            if (checkTimer <= 0.0f)
            {
                increment++;
                checkTimer = 5f;
                if(increment >= maxIncrement){
                    resetVariables();
                    ai.switchState(ai.idleState);
                }
                PickNewTarget(ai);              // Pick a new target or switch state
            }
        }
    }

    private void PickNewTarget(AIStateManager ai) {
        // Pick a random room
        RoomContainer randomRoom = ai.rooms[Random.Range(0, ai.rooms.Length)];

        // Pick a random window in that room
        if (randomRoom.windowAnchorPoints.Length > 0) {
            currentTarget = randomRoom.windowAnchorPoints[Random.Range(0, randomRoom.windowAnchorPoints.Length)];
        }
    }

    public override void resetVariables()
    {
        increment = 0;
    }
}