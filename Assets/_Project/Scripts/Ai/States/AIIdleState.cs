using UnityEngine;
using UnityEngine.Animations;

public class AIIdleState : AIBaseState
{

    float idleStateTimer;      // How long does she stay in Idle State in Seconds
    public override void enterState(AIStateManager ai){
        Debug.Log("Dolly entered state 0");

        idleStateTimer = ai.getIdleTime;

        // move to spawn
        ai.transform.position = new Vector3(ai.idleSpawn.position.x, ai.idleSpawn.position.y, ai.idleSpawn.position.z);
    }
    
    public override void onUpdate(AIStateManager ai){

        // Waits until called or until idle timer reaches 0
        Wait(ai);
        
    }

    public override void resetVariables(AIStateManager ai){}

        public override void exitState(AIStateManager ai)
    {
        ai.switchState(ai.patrolState);
    }

    /*
    *  Waits and does nothing until idleStateTimer reaches 0
    */
    private void Wait(AIStateManager ai){
        this.idleStateTimer -= Time.deltaTime;

        if (this.idleStateTimer <= 0.0f)
        {
            ai.switchState(ai.patrolState);
        }
    }
}
