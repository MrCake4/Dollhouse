using UnityEngine;
using UnityEngine.Animations;

public class AIScanState : AIBaseState
{
    float checkRoomTime;
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered Scan State");
        checkRoomTime = ai.getCheckRoomTime;
        ai.eye.setStartScan(true);

    }
    
    public override void onUpdate(AIStateManager ai){

        //Debug.Log("Scan Window");
        checkRoomTime -= Time.deltaTime;
        // if eye sees player
        //      ai.switchState(ai.attackState)
        if (checkRoomTime <= 0.0f) {
            exitState(ai);}
    }

    public override void resetVariables(AIStateManager ai){}

    public override void exitState(AIStateManager ai)
    {
        ai.eye.setStartScan(false);
        ai.switchState(ai.getLastState, false);
    }
}
