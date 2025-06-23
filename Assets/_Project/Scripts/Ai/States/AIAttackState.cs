using UnityEngine;

public class AIAttackState : AIBaseState
{
    public override void enterState(AIStateManager ai)
    {
        Debug.Log("Dolly entered state ATTACK");

        // Reset laser buildup timer
        ai.eye.SetHitPlayer(false);
    }

    public override void onUpdate(AIStateManager ai)
    {
        // Actively aim and shoot at player
        ai.eye.FollowAndShoot();

        // Once shot fired (hit or miss), transition to HuntState
        if (ai.eye.PlayerWasHit || !ai.eye.TargetAcquired)
        {
            exitState(ai);
            ai.switchState(ai.huntState);
        }
    }

    public override void resetVariables(AIStateManager ai) { }

    public override void exitState(AIStateManager ai)
    {
        ai.eye.SetSpotlight(false);
    }
}
