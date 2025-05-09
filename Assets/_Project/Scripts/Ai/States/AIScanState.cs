using UnityEngine;
using UnityEngine.Animations;

public class AIScanState : AIBaseState
{
    RoomContainer currentTargetRoom;
    float checkRoomTime;
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered Scan State");
        checkRoomTime = ai.getCheckRoomTime;
        currentTargetRoom = ai.currentTargetRoom;
        ai.scanDone = false;

        // incase of null
        if (currentTargetRoom == null || currentTargetRoom.windowAnchorPoints.Length == 0) {
            ai.switchState(ai.patrolState, false);
            Debug.Log("Room is empty or has no windows");
            return;
        }

        ai.eye.setStartScan(true);

        // Sets the first window as target
        ai.setCurrentTargetWindow(currentTargetRoom.windowAnchorPoints[ai.currentWindowIndex]);
    }
    
    public override void onUpdate(AIStateManager ai){
        if (ai.currentTargetWindow == null) return;

         // If reached current window
        if (Vector3.Distance(ai.transform.position, ai.currentTargetWindow.position) < 0.1f) {
            checkRoomTime -= Time.deltaTime;

            if(ai.isPatroling && checkRoomTime <= 0.0f){
                resetVariables(ai);
                ai.scanDone = true;
                ai.switchState(ai.getLastState,false);
            }
    
            if (checkRoomTime <= 0.0f) {
                ai.currentWindowIndex++;
                checkRoomTime = ai.getCheckRoomTime;
            }

            // if done with room
            if(ai.currentWindowIndex >= currentTargetRoom.windowAnchorPoints.Length) {
                // Reset window index for next room
                resetVariables(ai);
                ai.scanDone = true; 
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
