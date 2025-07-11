using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private List<GrabPointTrigger> grabPoints = new List<GrabPointTrigger>();
    private Rigidbody rb;
    private RigidbodyConstraints originalConstraints;
    private bool originalKinematic;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Ursprüngliche Werte sichern
        originalConstraints = rb.constraints;
        originalKinematic = rb.isKinematic;

        //SetPhysicsActive(false);
    }


    public void RegisterGrabPoint(GrabPointTrigger point)
    {
        if (!grabPoints.Contains(point))
        {
            grabPoints.Add(point);
        }
    }

    public Transform GetGrabPoint()
    {
        foreach (GrabPointTrigger point in grabPoints)
        {
            if (point.IsAvailable)
            {
                return point.GetTransform();
            }
        }

        return null;
    }

    public bool IsGrabAllowed()
    {
        return GetGrabPoint() != null;
    }

    public void SetPhysicsActive(bool active)
    {
        rb.isKinematic = !active;
        rb.constraints = active ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezeAll;
    }

    public void ResetPhysics()
    {
        rb.isKinematic = originalKinematic;
        rb.constraints = originalConstraints;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    
}
