using UnityEngine;
using UnityEngine.Animations;

public class AIHuntState : AIBaseState
{
    RoomContainer currentTargetRoom;
    int windowIndex = 0;
    float checkTime;

    public override void enterState(AIStateManager ai) {
        Debug.Log("Dolly entered HUNT state");

        currentTargetRoom = ai.currentTargetRoom;
        
        if (currentTargetRoom == null) {
            ai.switchState(ai.patrolState);
            return;
        }

        // Start from first window
        checkTime = ai.checkRoomTime;
        windowIndex = 0;
        ai.setCurrentTargetWindow(currentTargetRoom.windowAnchorPoints[0]);
    }

    public override void onUpdate(AIStateManager ai) {
        if (currentTargetRoom == null || currentTargetRoom.windowAnchorPoints.Length == 0) {
            ai.switchState(ai.patrolState);
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
            
            checkTime -= Time.deltaTime;

            if (checkTime <= 0){            // if timer 0 go to next room
                windowIndex++;

                if (windowIndex < ai.currentTargetRoom.windowCount) {
                    checkTime = ai.checkRoomTime;
                    ai.setCurrentTargetWindow(ai.currentTargetRoom.windowAnchorPoints[windowIndex]);
                } else {
                    // Finished searching room
                    // currentTargetRoom.checkedRoom = true;
                    //ai.switchState(ai.seekState);             // TODO: He doesn't go to patrol state but to seek state after this
                    ai.seekIncrement++;
                    ai.switchState(ai.seekState); // Go to next room

                }
            }
        }
    }

    public override void resetVariables(AIStateManager ai) {
        ai.currentTargetRoom = null;
        ai.currentTargetWindow = null;
        windowIndex = 0;
    }

    public override void exitState(AIStateManager ai)
    {
    }
}
