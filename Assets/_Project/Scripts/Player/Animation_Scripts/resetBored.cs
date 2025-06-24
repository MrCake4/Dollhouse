using UnityEngine;

public class ResetBoredState : StateMachineBehaviour
{
    private bool hasReset = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasReset = false; // Animation beginnt → noch nicht zurücksetzen
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Wenn die Animation einmal vollständig abgespielt wurde
        if (!hasReset && stateInfo.normalizedTime >= 1f)
        {
            animator.SetBool("isBored", false);
            hasReset = true; // nur einmal zurücksetzen
        }
    }
}
