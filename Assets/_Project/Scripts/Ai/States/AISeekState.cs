using UnityEngine;

public class AISeekState : AIBaseState
{
   int centerRoomIndexPosition = 0;

    public override void enterState(AIStateManager ai)
    {
        Debug.Log("Dolly entered SEEK state");
        
        // gets index of room
        if(ai.lastKnownRoom != null){
            for (int i = 0; i < ai.rooms.Length; i++){
                if (ai.lastKnownRoom == ai.rooms[i]){
                    centerRoomIndexPosition = i;
                    break;
                }
            }
        }

        // Set next room based on seek increment
        int nextIndex = centerRoomIndexPosition + ai.seekIncrement;

        if (nextIndex >= 0 && nextIndex < ai.rooms.Length)
        {
            RoomContainer nextRoom = ai.rooms[nextIndex];

            // increments starts with 1 so this only applies if it's -1
            if(ai.currentTargetRoom == nextRoom){
                resetVariables(ai);
                ai.switchState(ai.patrolState,false);
                return;
            }

            ai.setCurrentTargetRoom(nextRoom);
            ai.seekIncrement = -1;
            ai.switchState(ai.scanState, false);
            return;
        }
        exitState(ai);
    }

    public override void onUpdate(AIStateManager ai) {
    }

    public override void resetVariables(AIStateManager ai)
    {
        ai.currentTargetRoom = null;
        ai.seekIncrement = 1;
    }

    public override void exitState(AIStateManager ai)
    {
    }
}