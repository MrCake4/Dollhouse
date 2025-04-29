using UnityEngine;

public class AIHuntState : AIBaseState
{
    public RoomContainer currentTargetRoom;
    private Transform currentTargetWindow;
    private int windowIndex = 0;

    public override void enterState(AIStateManager ai) {
        Debug.Log("Dolly entered HUNT state");

        
        if (currentTargetRoom == null) {
            ai.switchState(ai.patrolState);
            return;
        }

        // Start from first window
        windowIndex = 0;
        currentTargetWindow = currentTargetRoom.windowAnchorPoints[windowIndex];
    }

    public override void onUpdate(AIStateManager ai) {
        if (currentTargetRoom == null || currentTargetRoom.windowAnchorPoints.Length == 0) {
            ai.switchState(ai.patrolState);
            Debug.Log("No Windows");
            return;
        }
        currentTargetRoom.triggered = false;
        // Move toward current window
        ai.transform.position = Vector3.MoveTowards(
            ai.transform.position,
            currentTargetWindow.position,
            Time.deltaTime * ai.moveSpeed
        );

        // If reached current window, move to next
        if (Vector3.Distance(ai.transform.position, currentTargetWindow.position) < 0.1f) {
            windowIndex++;

            if (windowIndex < currentTargetRoom.windowAnchorPoints.Length) {
                currentTargetWindow = currentTargetRoom.windowAnchorPoints[windowIndex];
            } else {
                // Finished all windows
                currentTargetRoom.checkedRoom = true;
                ai.switchState(ai.patrolState);
            }
        }
    }

    public override void resetVariables() {
        currentTargetRoom = null;
        currentTargetWindow = null;
        windowIndex = 0;
    }

    public void setCurrentTargetRoom(RoomContainer targetRoom) {
        this.currentTargetRoom = targetRoom;
    }
}
