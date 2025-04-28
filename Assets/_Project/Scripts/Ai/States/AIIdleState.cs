using UnityEngine;
using UnityEngine.Animations;

public class AIIdleState : AIBaseState
{

    float idleStateTimer;      // How long does she stay in Idle State in Seconds
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered state 0");

        idleStateTimer = ai.idleTime;

        // move to spawn
        ai.transform.position = new Vector3(ai.idleSpawn.position.x, ai.idleSpawn.position.y, ai.idleSpawn.position.z);
    }
    
    public override void onUpdate(AIStateManager ai){

        idleStateTimer -= Time.deltaTime;

        if (idleStateTimer <= 0.0f)
        {
            ai.switchState(ai.patrolState);
        }

    }

    public override void resetVariables(){}
}
