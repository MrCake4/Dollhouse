using UnityEngine;

public class AISeekState : AIBaseState
{
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered state 2");
    }
    
    public override void onUpdate(AIStateManager ai){
    }
}
