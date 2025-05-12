using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.HighDefinition;

public class AIHuntState : AIBaseState
{
    RoomContainer currentTargetRoom;

    public override void enterState(AIStateManager ai) {
        Debug.Log("Dolly entered HUNT state");
        currentTargetRoom = ai.currentTargetRoom;
        
        // incase Null
        if (currentTargetRoom == null) {
            Debug.LogWarning("HuntState: currentTargetRoom is null. Returning to patrol.");
            ai.switchState(ai.patrolState, false);
            return;
        }

        // reset trigger
        currentTargetRoom.triggered = false;
    }

    public override void onUpdate(AIStateManager ai) {
        if(ai.scanDone){
            resetVariables(ai);
            ai.switchState(ai.seekState, false);
        } else{
            ai.seekIncrement = 1;
            resetVariables(ai);
            ai.switchState(ai.scanState, false);
        }
    }

    public override void resetVariables(AIStateManager ai) {
        ai.currentWindowIndex = 0;
        ai.currentTargetWindow = null;
    }

    public override void exitState(AIStateManager ai) {
    }
}