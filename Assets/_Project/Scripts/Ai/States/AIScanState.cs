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

        ai.eye.setStartScan(true);

        // Sets the first window as target
        ai.setCurrentTargetWindow(currentTargetRoom.windowAnchorPoints[ai.currentWindowIndex]);
    }
    
    public override void onUpdate(AIStateManager ai){

        Debug.Log("Scan Window");

        checkRoomTime -= Time.deltaTime;
        if (checkRoomTime <= 0.0f) {
            exitState(ai);
        }

        if (currentTargetRoom == null || currentTargetRoom.windowAnchorPoints.Length == 0) {
            ai.switchState(ai.patrolState, false);
            return;
        }

         // If reached current window, move to next
        if (Vector3.Distance(ai.transform.position, ai.currentTargetWindow.position) < 0.1f) {
            
            if(ai.currentWindowIndex <= currentTargetRoom.windowCount - 1) {
                ai.currentWindowIndex++;
            } else {
                ai.seekIncrement = 0;        
                ai.currentWindowIndex = 0; // Reset window index for next room
                ai.switchState(ai.seekState, false); // Go to next room
            }
             // Move toward current window
        } else {ai.transform.position = Vector3.MoveTowards(ai.transform.position,ai.currentTargetWindow.position,Time.deltaTime * ai.moveSpeed);}
    }

    public override void resetVariables(AIStateManager ai){}

    public override void exitState(AIStateManager ai)
    {
        ai.eye.setStartScan(false);
        ai.switchState(ai.getLastState, false);
    }
}
