using UnityEngine;
using UnityEngine.Animations;

public class AIHuntState : AIBaseState
{
    public RoomContainer currentTargetRoom;
    private Transform currentTargetWindow;
    private int windowIndex = 0;
    float checkTime;

    public override void enterState(AIStateManager ai) {
        Debug.Log("Dolly entered HUNT state");

        currentTargetRoom = ai.currentTargetRoom;
        
        if (ai.currentTargetRoom == null) {
            ai.switchState(ai.patrolState);
            return;
        }

        // Start from first window
        checkTime = ai.checkRoomTime;
        windowIndex = 0;
        currentTargetWindow = currentTargetRoom.windowAnchorPoints[0];
    }

    public override void onUpdate(AIStateManager ai) {
        if (ai.currentTargetRoom == null || ai.currentTargetRoom.windowAnchorPoints.Length == 0) {
            ai.switchState(ai.patrolState);
            return;
        }

        ai.currentTargetRoom.triggered = false;

        // Move toward current window
        ai.transform.position = Vector3.MoveTowards(
            ai.transform.position,
            currentTargetWindow.position,
            Time.deltaTime * ai.moveSpeed
        );

        // If reached current window, move to next
        if (Vector3.Distance(ai.transform.position, currentTargetWindow.position) < 0.1f) {
            
            checkTime -= Time.deltaTime;

            if (checkTime <= 0){            // if timer 0 go to next room
                windowIndex++;

                if (windowIndex < ai.currentTargetRoom.windowAnchorPoints.Length) {
                    checkTime = ai.checkRoomTime;
                    currentTargetWindow = ai.currentTargetRoom.windowAnchorPoints[windowIndex];
                } else {
                    // Finished searching room
                    // currentTargetRoom.checkedRoom = true;
                    ai.switchState(ai.patrolState);             // TODO: He doesn't go to patrol state but to seek state after this
                }
            }
        }
    }

    public override void resetVariables(AIStateManager ai) {
        ai.currentTargetRoom = null;
        currentTargetWindow = null;
        windowIndex = 0;
    }
}
