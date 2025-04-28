using UnityEngine;

public class AIAttackState : AIBaseState
{
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered state 4");
    }
    
    public override void onUpdate(AIStateManager ai){
    }

    public override void resetVariables(){}
}
