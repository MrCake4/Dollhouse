using UnityEngine;
using UnityEngine.Animations;

public class AIScanState : AIBaseState
{
    float checkRoomTime;
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered Scan State");
        checkRoomTime = ai.getCheckRoomTime;

    }
    
    public override void onUpdate(AIStateManager ai){

        Debug.Log("Scan Window");
        checkRoomTime -= Time.deltaTime;
        // if eye sees player
        //      ai.switchState(ai.attackState)
        if (checkRoomTime <= 0.0f) {
            exitState(ai);}
    }

    public override void resetVariables(AIStateManager ai){}

    public override void exitState(AIStateManager ai)
    {
        ai.switchState(ai.getLastState, false);
    }
}
