using UnityEngine;

public class AISeekState : AIBaseState
{
    int centerRoomIndexPosition = 0;

    public override void enterState(AIStateManager ai)
    {
        Debug.Log("Dolly entered SEEK state");

        // Gets index of last known room
        if (ai.lastKnownRoom != null)
        {
            for (int i = 0; i < ai.rooms.Length; i++)
            {
                if (ai.lastKnownRoom == ai.rooms[i])
                {
                    centerRoomIndexPosition = i;
                    break;
                }
            }
        }

        // Set next room based on seek increment
        int nextIndex = centerRoomIndexPosition + ai.seekIncrement;

        // If nextIndex is out of bounds:
        if (nextIndex < 0)
        {
            // If nextIndex is less than 0, move to the next room to the right
            nextIndex = centerRoomIndexPosition + 1;
        }
        else if (nextIndex >= ai.rooms.Length)
        {
            // If nextIndex exceeds the max index, move to the previous room to the left
            nextIndex = centerRoomIndexPosition - 1;
        }

        if (ai.seekRoomsChecked == 2)
        {
            resetVariables(ai);
            ai.switchState(ai.patrolState);
            return;
        }

        // Check if the calculated nextIndex is valid
        if (nextIndex >= 0 && nextIndex < ai.rooms.Length)
        {
            RoomContainer nextRoom = ai.rooms[nextIndex];

            // If the next room is the same as the current target room, exit seek state
            if (ai.currentTargetRoom == nextRoom)
            {
                resetVariables(ai);
                ai.switchState(ai.patrolState);
                return;
            }


            ai.setCurrentTargetRoom(nextRoom);
            // Switch direction for next time
            ai.seekIncrement = -ai.seekIncrement;
            ai.seekRoomsChecked++;
            ai.switchState(ai.scanState);
            return;
        }
        resetVariables(ai);
        ai.switchState(ai.patrolState);
    }


    public override void onUpdate(AIStateManager ai)
    {
    }

    public override void resetVariables(AIStateManager ai)
    {
        ai.currentTargetRoom = null;
        ai.seekIncrement = 1;
        ai.isPatroling = true;
        ai.windowsPatrolled = 0;
        ai.seekRoomsChecked = 0;
    }

    public override void exitState(AIStateManager ai)
    {

    }
}
