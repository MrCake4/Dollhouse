using UnityEngine;
using UnityEngine.Animations;

public class AIPatrolState : AIBaseState
{
// Removed unused variable 'currentTarget'
   Transform lastWindow = null;
    RoomContainer randomRoom; 
    int randomWindow;

    public override void enterState(AIStateManager ai) {
        Debug.Log("Dolly entered PATROL State");

        // Teleport to Patrol Spawn point
        if (!ai.isPatroling) {
            ai.transform.position = new Vector3(ai.patrolSpawn.position.x, ai.patrolSpawn.position.y, ai.patrolSpawn.position.z);
            ai.isPatroling = true;    
        }
        // Pick target
        PickNewTarget(ai);
    }

    public override void onUpdate(AIStateManager ai) {
        if (ai.currentTargetWindow == null) return;


        if (ai.windowsPatrolled >= ai.getCheckWindowPerPatrol) exitState(ai);
        else
        {
            ai.setCurrentTargetRoom(randomRoom);
            ai.currentWindowIndex = randomWindow;
            ai.windowsPatrolled++;
            ai.switchState(ai.scanState, false);
        }
        
    }

    private void PickNewTarget(AIStateManager ai) {
        // Pick a random room
        randomRoom = ai.rooms[Random.Range(0, ai.rooms.Length)];

        // Pick a random window in that room
        if (randomRoom.windowCount > 0) {
            Transform newWindow;
            int safety = 0;
            do {
                randomWindow = Random.Range(0, randomRoom.windowAnchorPoints.Length);
                newWindow = randomRoom.windowAnchorPoints[randomWindow];
                safety++;
            } while (newWindow == lastWindow && safety < 10);
        ai.currentTargetWindow = newWindow;
        lastWindow = newWindow;
        }
    }

    public override void resetVariables(AIStateManager ai)
    {
        ai.windowsPatrolled = 0;
        ai.currentTargetWindow = null;
        ai.isPatroling = false;
        ai.currentWindowIndex = 0;
    }

    public override void exitState(AIStateManager ai)
    {
        resetVariables(ai);
        ai.switchState(ai.idleState, false);
    }
}