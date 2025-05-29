using UnityEngine;

public class AIAttackState : AIBaseState
{
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered state ATTACK");
    }
    
    public override void onUpdate(AIStateManager ai){
        /*
            if eye shot
                ai.switchState(ai.huntState);
        */
    }

    public override void resetVariables(AIStateManager ai){}

        public override void exitState(AIStateManager ai)
    {
    }
}
