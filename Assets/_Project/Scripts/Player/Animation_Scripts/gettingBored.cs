using UnityEngine;

public class idleVariations : StateMachineBehaviour
{
    [SerializeField] private float timeUntilBored = 5f;
    [SerializeField] private int numberOfBoredVariations = 3; // z. B. 3 Animationen im BlendTree

    private float idleTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTimer = 0f;
        animator.SetBool("isBored", false);
        animator.SetFloat("boredVariation", 0f); // Standard setzen
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float speed = animator.GetFloat("Speed");

        if (speed < 0.01f)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= timeUntilBored)
            {
                animator.SetBool("isBored", true);

                // Zufällige boredVariation wählen (float)
                float variation = Random.Range(0, numberOfBoredVariations); // z. B. 0, 1, 2
                animator.SetFloat("boredVariation", variation);
            }
        }
        else
        {
            idleTimer = 0f;
        }
    }
}
