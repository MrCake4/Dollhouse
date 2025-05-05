using UnityEngine;
using UnityEngine.Animations;

public class AIHuntState : AIBaseState
{
    RoomContainer currentTargetRoom;
    // float checkTime;

    public override void enterState(AIStateManager ai) {
        Debug.Log("Dolly entered HUNT state");

        currentTargetRoom = ai.currentTargetRoom;
        
        if (currentTargetRoom == null) {
            ai.switchState(ai.patrolState, false);
            return;
        }

        // Start from first window
        // checkTime = ai.getCheckRoomTime;
        if(ai.currentWindowIndex <= currentTargetRoom.windowCount - 1) ai.setCurrentTargetWindow(currentTargetRoom.windowAnchorPoints[ai.currentWindowIndex]);
    }

    public override void onUpdate(AIStateManager ai) {
        if (currentTargetRoom == null || currentTargetRoom.windowAnchorPoints.Length == 0) {
            ai.switchState(ai.patrolState, false);
            return;
        }

        currentTargetRoom.triggered = false;

        // Move toward current window
        ai.transform.position = Vector3.MoveTowards(
            ai.transform.position,
            ai.currentTargetWindow.position,
            Time.deltaTime * ai.moveSpeed
        );

    
        // If reached current window, move to next
        if (Vector3.Distance(ai.transform.position, ai.currentTargetWindow.position) < 0.1f) {
            
            if(ai.currentWindowIndex <= currentTargetRoom.windowCount - 1) {
                ai.currentWindowIndex++;
                ai.switchState(ai.scanState, false); // Go to scan state
            } else {
                // Finished searching room
                // currentTargetRoom.checkedRoom = true;
                // ai.switchState(ai.seekState);             // TODO: He doesn't go to patrol state but to seek state after this
                ai.seekIncrement = 2;    // set to 2 insead of +1
                ai.currentWindowIndex = 0; // Reset window index for next room
                ai.switchState(ai.seekState, false); // Go to next room
            }

            /*
            checkTime -= Time.deltaTime;

            if (checkTime <= 0){            // if timer 0 go to next room
                windowIndex++;

                if (windowIndex < ai.currentTargetRoom.windowCount) {
                    checkTime = ai.getCheckRoomTime;
                    ai.setCurrentTargetWindow(ai.currentTargetRoom.windowAnchorPoints[windowIndex]);
                } else {
                    // Finished searching room
                    // currentTargetRoom.checkedRoom = true;
                    //ai.switchState(ai.seekState);             // TODO: He doesn't go to patrol state but to seek state after this
                    ai.seekIncrement++;
                    ai.switchState(ai.seekState, false); // Go to next room

                }
            }
            */
        }
    }

    public override void resetVariables(AIStateManager ai) {
        ai.currentTargetRoom = null;
        ai.currentTargetWindow = null;
        ai.currentWindowIndex = 0;
    }

    public override void exitState(AIStateManager ai)
    {
    }
}
