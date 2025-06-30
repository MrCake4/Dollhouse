using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerStateManager player;
    private Animator animator;

    [Header("Animator Parameters")]
    public string speedParam = "Speed";
    public string verticalVelocityParam = "VerticalVelocity"; // <- NEU

    void Start()
    {
        player = GetComponentInParent<PlayerStateManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Horizontale Bewegung (für Walk/Run/Idle)
        Vector3 horizontalVel = player.GetHorizontalVelocity();
        float speed = horizontalVel.magnitude;
        animator.SetFloat(speedParam, speed, 0.1f, Time.deltaTime);

        // Vertikale Bewegung (für Jump-Fall-BlendTree)
        float verticalVelocity = player.rb.linearVelocity.y;
        animator.SetFloat(verticalVelocityParam, verticalVelocity, 0.1f, Time.deltaTime);

        
    }
}
