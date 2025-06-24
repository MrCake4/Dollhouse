using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerStateManager player;
    private Animator animator;


    [Header("Animator Parameters")]
    public string speedParam = "Speed";
    

    void Start()
    {
        player = GetComponentInParent<PlayerStateManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 horizontalVel = player.GetHorizontalVelocity();
        float speed = horizontalVel.magnitude;

        animator.SetFloat(speedParam, speed, 0.1f, Time.deltaTime);

        
    }
}
