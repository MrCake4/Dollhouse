using UnityEngine;

public class AIIdleState : AIBaseState
{
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered state 0");

        // move to spawn
        ai.transform.position = new Vector3(ai.spawn.position.x, ai.spawn.position.y, ai.spawn.position.z);
    }
    
    public override void onUpdate(AIStateManager ai){

        

    }
}
