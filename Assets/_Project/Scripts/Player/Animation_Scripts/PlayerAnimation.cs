using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerStateManager player;
    private Animator animator;

    [Header("Animator Parameters")]
    public string speedParam = "Speed";
    public string groundDistanceParam = "GroundDistance"; // <- neu

    [Header("Raycast")]
    public float maxGroundDistance = 2f;
    public Transform rayOriginOverride; // falls du lieber einen leichten offset willst

    void Start()
    {
        player = GetComponentInParent<PlayerStateManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Horizontale Geschwindigkeit
        Vector3 horizontalVel = player.GetHorizontalVelocity();
        float speed = horizontalVel.magnitude;
        animator.SetFloat(speedParam, speed, 0.1f, Time.deltaTime);

        // Ground distance
        Vector3 rayOrigin = rayOriginOverride ? rayOriginOverride.position : player.transform.position;
        rayOrigin += Vector3.up * 0.1f; // leichter Offset
        float groundDistance = maxGroundDistance;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, maxGroundDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            groundDistance = hit.distance;
        }

        // Normalisiert: 0 = Boden direkt drunter, 1 = max hoch
        float normalized = Mathf.Clamp01(groundDistance / maxGroundDistance);

        animator.SetFloat(groundDistanceParam, normalized, 0.1f, Time.deltaTime);
    }

    public void EndPullUp()
    {
        if (player.getCurrentState is PullUpState pullUp)
        {
            pullUp.pullUpFinished = true;
        }
    }

}
