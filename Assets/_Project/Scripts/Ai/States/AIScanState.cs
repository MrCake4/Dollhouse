using UnityEngine;

public class AIScanState : AIBaseState
{
    private RoomContainer currentTargetRoom;
    private bool scanStarted;

    public override void enterState(AIStateManager ai)
    {
        Debug.Log("Dolly entered SCAN State");

        currentTargetRoom = ai.currentTargetRoom;
        ai.scanDone = false;
        scanStarted = false;

        // Fail-safe check
        if (currentTargetRoom == null || currentTargetRoom.windowAnchorPoints.Length == 0)
        {
            Debug.LogWarning("No windows found in room — reverting to patrol.");
            ai.switchState(ai.patrolState);
            return;
        }

        // Set first window as scanning target
        ai.setCurrentTargetWindow(currentTargetRoom.windowAnchorPoints[ai.currentWindowIndex]);
        ai.eye.ApplyAnchorOverride(ai.currentTargetWindow);
    }

    public override void onUpdate(AIStateManager ai)
    {
        if (ai.currentTargetWindow == null)
        {
            Debug.LogWarning("No current target window set.");
            return;
        }

        // Move to current window anchor
        if (Vector3.Distance(ai.transform.position, ai.currentTargetWindow.position) > 0.1f)
        {
            ai.transform.position = Vector3.MoveTowards(
                ai.transform.position,
                ai.currentTargetWindow.position,
                Time.deltaTime * ai.moveSpeed
            );
            return;
        }

        // Begin scanning if not already started
        if (!scanStarted)
        {
            ai.eye.BeginScanSweep();
            scanStarted = true;
            return;
        }

        // If target acquired, follow and shoot
        if (ai.eye.TargetAcquired)
        {
            resetVariables(ai);
            ai.switchState(ai.attackState);
            return;
        }

        // Wait until scan sweep finishes
        if (!ai.eye.IsDoneSweeping)
        {
            ai.eye.UpdateLaser(); // Optional visual update while scanning
            return;
        }

        // if he is currently Patroling 
        if (ai.isPatroling && ai.eye.IsDoneSweeping)
        {
            resetVariables(ai);
            ai.switchState(ai.getLastState);
            return;
        }

        // Done scanning current window
        ai.currentWindowIndex++;
        scanStarted = false;

        // If all windows done
        if (ai.currentWindowIndex >= currentTargetRoom.windowAnchorPoints.Length)
        {
            resetVariables(ai);
            ai.scanDone = true;
            ai.switchState(ai.getLastState);
            return;
        }

        // Otherwise, proceed to next window
        ai.setCurrentTargetWindow(currentTargetRoom.windowAnchorPoints[ai.currentWindowIndex]);
        ai.eye.ApplyAnchorOverride(ai.currentTargetWindow);
    }

    public override void resetVariables(AIStateManager ai)
    {
        ai.currentWindowIndex = 0;
    }

    public override void exitState(AIStateManager ai)
    {
    }
}
