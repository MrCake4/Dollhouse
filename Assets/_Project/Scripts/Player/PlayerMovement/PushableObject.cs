using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Transform currentGrabPoint;
    private bool grabBlocked = false;
    private Rigidbody rb;

    public LayerMask playerOnlyLayer; // optional für Layer-Maskenlogik

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Standardmäßig ist es unbeweglich
        SetPhysicsActive(false);
    }

    public void SetGrabPoint(Transform point)
    {
        currentGrabPoint = point;
    }

    public void ClearGrabPoint(Transform point)
    {
        if (currentGrabPoint == point)
        {
            currentGrabPoint = null;
        }
    }

    public Transform GetGrabPoint()
    {
        return currentGrabPoint;
    }

    public bool IsGrabAllowed()
    {
        return currentGrabPoint != null && !grabBlocked;
    }

    public void SetGrabBlocked(bool blocked)
    {
        grabBlocked = blocked;
    }

    public void SetPhysicsActive(bool active)
    {
        rb.isKinematic = !active;
        rb.constraints = active ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezeAll;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public void ResetPhysics()
    {
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

}
