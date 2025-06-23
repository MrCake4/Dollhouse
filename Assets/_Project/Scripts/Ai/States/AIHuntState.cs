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
        ai.isHunting = true;
        ai.isPatroling = false;
        
        // incase Null
        if (currentTargetRoom == null)
        {
            Debug.LogWarning("HuntState: currentTargetRoom is null. Returning to patrol.");
            ai.switchState(ai.patrolState);
            return;
        }

        // reset trigger
        currentTargetRoom.triggered = false;
    }

    public override void onUpdate(AIStateManager ai) {
        if (ai.scanDone)
        {
            ai.scanDone = false;
            resetVariables(ai);
            ai.switchState(ai.seekState);
            return;
        }
        else
        {
            ai.seekIncrement = 1;
            resetVariables(ai);
            ai.eye.ResetEyeScan();
            ai.switchState(ai.scanState);
            return;
        }
    }

    public override void resetVariables(AIStateManager ai) {
        ai.currentWindowIndex = 0;
        ai.currentTargetWindow = null;
    }

    public override void exitState(AIStateManager ai) {
    }
}