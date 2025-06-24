using UnityEngine;

public class DeadState : BasePlayerState
{
    private Rigidbody playerRigidbody;
    private CapsuleCollider mainCollider;
    private Animator animator;

    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("Player is dead!");

        playerRigidbody = player.GetComponent<Rigidbody>();
        mainCollider = player.GetComponent<CapsuleCollider>();
        animator = player.GetComponentInChildren<Animator>();

        if (playerRigidbody != null) playerRigidbody.isKinematic = true;
        if (mainCollider != null) mainCollider.enabled = false;
        if (animator != null) animator.enabled = false;

        // Ragdoll aktivieren
        EnableRagdoll(player.transform);
    }

    public override void onUpdate(PlayerStateManager player) { }
    public override void onFixedUpdate(PlayerStateManager player) { }

    public override void onExit(PlayerStateManager player)
    {
        // Im DeadState bleibt man – normalerweise kein Exit nötig,
        // aber falls doch: Ragdoll deaktivieren (optional).
    }

    private void EnableRagdoll(Transform root)
    {
        Collider[] colliders = root.GetComponentsInChildren<Collider>(true);

        foreach (Collider col in colliders)
        {
            if (col is CapsuleCollider || col is BoxCollider || col is SphereCollider)
            {
                if (col.gameObject == root.gameObject) continue; // überspringt Haupt-Collider

                col.enabled = true;

                // Rigidbody hinzufügen, falls nicht vorhanden
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = col.gameObject.AddComponent<Rigidbody>();
                    rb.mass = 1f;
                }

                rb.isKinematic = false;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }
    }
}
