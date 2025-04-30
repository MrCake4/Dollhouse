using UnityEngine;

public class AISeekState : AIBaseState
{
   int currentRoomIndexPosition = 0;

    public override void enterState(AIStateManager ai)
    {
        Debug.Log("Dolly entered SEEK state");

        // Only determine index on first seek pass
        if (ai.seekIncrement == 0 && ai.currentTargetRoom != null)
        {
            for (int i = 0; i < ai.rooms.Length; i++)
            {
                if (ai.currentTargetRoom == ai.rooms[i])
                {
                    currentRoomIndexPosition = i;
                    break;
                }
            }
        }

        // Set next room based on seek increment
        int nextIndex = currentRoomIndexPosition + ai.seekIncrement;

        if (nextIndex < ai.rooms.Length)
        {
            ai.setCurrentTargetRoom(ai.rooms[nextIndex]);
            ai.switchState(ai.huntState); // hunt will use currentTargetRoom
        }
        else
        {
            exitState(ai); // No more rooms to check, go back to patrol
        }
    }

    public override void onUpdate(AIStateManager ai) {
        // No logic here anymore
    }

    public override void resetVariables(AIStateManager ai)
    {
    }

    public override void exitState(AIStateManager ai)
    {
        ai.seekIncrement = 0;
        ai.switchState(ai.patrolState);
    }
}