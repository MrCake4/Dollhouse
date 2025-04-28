using UnityEngine;

public class AIIdleState : AIBaseState
{

    float idleStateTimer = 5f;      // How long does she stay in Idle State in Seconds
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered state 0");

        // move to spawn
        ai.transform.position = new Vector3(ai.spawn.position.x, ai.spawn.position.y, ai.spawn.position.z);
    }
    
    public override void onUpdate(AIStateManager ai){

        idleStateTimer -= Time.deltaTime;

        if (idleStateTimer <= 0.0f)
        {
            ai.switchState(ai.patrolState);
        }

    }
}
