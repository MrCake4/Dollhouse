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
            if (!ai.isHunting) ai.transform.position = new Vector3(ai.patrolSpawn.position.x, ai.patrolSpawn.position.y, ai.patrolSpawn.position.z);
            else ai.isHunting = false;      // This fixes the AI teleporting to patrol spawn after hunt
            ai.isPatroling = true;    
        }
        // Pick target
        PickNewTarget(ai);
    }

    public override void onUpdate(AIStateManager ai) {
        if (ai.currentTargetWindow == null)
        {
            Debug.Log("Current Target is null");
           return; 
        }

        if (ai.windowsPatrolled >= ai.getCheckWindowPerPatrol)
        {
            // exits patrol, moves to patrol spawn and goes into idle again
            ai.transform.position = Vector3.MoveTowards(ai.transform.position, ai.patrolSpawn.position, Time.deltaTime * ai.moveSpeed);

            if(Vector3.Distance(ai.transform.position, ai.patrolSpawn.position) < 0.1f) exitState(ai);
        }
        else
        {
            ai.setCurrentTargetRoom(randomRoom);
            ai.currentWindowIndex = randomWindow;
            ai.windowsPatrolled++;
            Debug.Log("Windows Patroled: " + ai.windowsPatrolled);
            ai.switchState(ai.scanState, false);
            return;
        }
        
    }

    private void PickNewTarget(AIStateManager ai)
    {
        // Flatten all windows across all rooms
        var validWindows = new System.Collections.Generic.List<(RoomContainer room, int index, Transform window)>();

        foreach (var room in ai.rooms)
        {
            for (int i = 0; i < room.windowAnchorPoints.Length; i++)
            {
                var window = room.windowAnchorPoints[i];
                if (window != null)
                {
                    validWindows.Add((room, i, window));
                }
            }
        }

        if (validWindows.Count == 0)
        {
            Debug.LogWarning("No valid windows found for patrol.");
            return;
        }

        // Pick a random window
        (RoomContainer selectedRoom, int windowIndex, Transform selectedWindow) =
            validWindows[Random.Range(0, validWindows.Count)];

        // Avoid repeating the same window
        int safety = 0;
        while (selectedWindow == lastWindow && safety < 10)
        {
            (selectedRoom, windowIndex, selectedWindow) = validWindows[Random.Range(0, validWindows.Count)];
            safety++;
        }

        // Set the patrol target
        randomRoom = selectedRoom;
        randomWindow = windowIndex;
        ai.currentTargetWindow = selectedWindow;
        lastWindow = selectedWindow;
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