using UnityEngine;

public class AIHuntState : AIBaseState
{
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered state 3");
    }
    
    public override void onUpdate(AIStateManager ai){
    }

    public override void resetVariables(){}
}
