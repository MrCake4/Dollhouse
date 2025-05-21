using UnityEngine;
using UnityEngine.Animations;

public class AIScanState : AIBaseState
{
    RoomContainer currentTargetRoom;
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered SCAN State");
        currentTargetRoom = ai.currentTargetRoom;
        ai.scanDone = false;

        ai.eye.setIsDoneScanning(true);
        
        // incase of null
        if (currentTargetRoom == null || currentTargetRoom.windowAnchorPoints.Length == 0)
        {
            ai.switchState(ai.patrolState, false);
            Debug.Log("Room is empty or has no windows");
            return;
        }

        // Sets the first window as target
        ai.setCurrentTargetWindow(currentTargetRoom.windowAnchorPoints[ai.currentWindowIndex]);
    }
    
    public override void onUpdate(AIStateManager ai){
        
        if (ai.currentTargetWindow == null) return;
         // If reached current window
        if (Vector3.Distance(ai.transform.position, ai.currentTargetWindow.position) < 0.1f) {
            if (!ai.eye.getStartScan && ai.eye.getIsDoneScanning)
            {
                ai.eye.setStartScan(true);
                ai.eye.setIsDoneScanning(false); // <<< HINZUGEFÜGT!
                Debug.Log("Started scan at window " + ai.currentWindowIndex);
            }
            bool isDoneScanning = ai.eye.getIsDoneScanning;
            Debug.Log(isDoneScanning + "|" + ai.eye.getStartScan);

            if (ai.isPatroling && isDoneScanning)
            {
                resetVariables(ai);
                ai.scanDone = true;
                ai.eye.setStartScan(false);
                ai.switchState(ai.getLastState, false);
                return;
            }

            if (isDoneScanning)
            {
                ai.currentWindowIndex++;
                ai.eye.setStartScan(false);
            }

            // if done with room
            if(ai.currentWindowIndex >= currentTargetRoom.windowAnchorPoints.Length) {
                // Reset window index for next room
                resetVariables(ai);
                ai.scanDone = true;
                ai.eye.setStartScan(false);
                ai.switchState(ai.getLastState, false);
                return;
            }
            ai.setCurrentTargetWindow(currentTargetRoom.windowAnchorPoints[ai.currentWindowIndex]);

             // Move toward current window
        } else {ai.transform.position = Vector3.MoveTowards(ai.transform.position,ai.currentTargetWindow.position,Time.deltaTime * ai.moveSpeed);}
    }

    public override void resetVariables(AIStateManager ai){
        ai.currentWindowIndex = 0;
    }

    public override void exitState(AIStateManager ai)
    {
    }
}
