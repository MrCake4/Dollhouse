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
        //EnableRagdoll(player.transform);
    }

    public override void onUpdate(PlayerStateManager player) { }
    public override void onFixedUpdate(PlayerStateManager player) { }

    public override void onExit(PlayerStateManager player)
    {
        // Im DeadState bleibt man – normalerweise kein Exit nötig,
        // aber falls doch: Ragdoll deaktivieren (optional).

        playerRigidbody.isKinematic = false;
        mainCollider.enabled = true;
        animator.enabled = true;
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



// !!!!!!!!!!!!! NEEDS TESTING !!!!!!!!!!!!

/*
    using UnityEngine;

public class DeadState : BasePlayerState
{
    private Rigidbody playerRigidbody;
    private CapsuleCollider mainCollider;
    private Animator animator;

    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("Player is dead!");

        // 1. Referenzen holen
        playerRigidbody = player.GetComponent<Rigidbody>();
        mainCollider = player.GetComponent<CapsuleCollider>();
        animator = player.GetComponentInChildren<Animator>();

        // 2. Haupt-Komponenten deaktivieren
        if (playerRigidbody != null) playerRigidbody.isKinematic = true;
        if (mainCollider != null) mainCollider.enabled = false;
        if (animator != null) animator.enabled = false;

        // 3. Ragdoll aktivieren
        EnableRagdoll(player.transform);
    }

    public override void onUpdate(PlayerStateManager player) { }
    public override void onFixedUpdate(PlayerStateManager player) { }

    public override void onExit(PlayerStateManager player)
    {
        // In der Regel bleibt man in DeadState. Wenn nicht, dann hier ggf. zurückbauen.
    }

    private void EnableRagdoll(Transform root)
    {
        // Alle Child-Rigidbodies & Collider aktivieren
        Rigidbody[] ragdollBodies = root.GetComponentsInChildren<Rigidbody>(true);
        Collider[] ragdollColliders = root.GetComponentsInChildren<Collider>(true);

        foreach (var rb in ragdollBodies)
        {
            if (rb.gameObject == root.gameObject) continue; // Root-Rigidbody NICHT anfassen
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        foreach (var col in ragdollColliders)
        {
            if (col.gameObject == root.gameObject) continue; // Root-Collider NICHT anfassen
            col.enabled = true;
        }
    }
}

*/