using UnityEngine;

public class AIAttackState : AIBaseState
{
    private bool waitingToSwitch = false;
    private float postShotTimer = 0f;
    private const float postShotDelay = 1.5f;

    public override void enterState(AIStateManager ai)
    {
        Debug.Log("Dolly entered state ATTACK");
        ai.eye.SetHitPlayer(false);
        waitingToSwitch = false;
    }

    public override void onUpdate(AIStateManager ai)
    {
        if (!waitingToSwitch)
        {
            ai.eye.FollowAndShoot();

            if (ai.eye.PlayerWasHit || !ai.eye.TargetAcquired)
            {
                ai.eye.SetSpotlight(false);
                ai.scanDone = false; // ensures hunt doesn't skip scan
                postShotTimer = postShotDelay;
                waitingToSwitch = true;
                Debug.Log("Post-shot delay started...");
            }
        }
        else
        {
            postShotTimer -= Time.deltaTime;
            if (postShotTimer <= 0f)
            {
                exitState(ai);
                ai.switchState(ai.huntState);
                ai.huntState.enterState(ai); // Ensure hunt reinitializes
            }
        }
    }

    public override void resetVariables(AIStateManager ai) { }

    public override void exitState(AIStateManager ai)
    {
        ai.eye.SetSpotlight(false);
    }
}
