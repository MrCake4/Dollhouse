using UnityEngine;

public class GrabPointTrigger : MonoBehaviour
{
    private PushableObject pushable;

    void Awake()
    {
        pushable = GetComponentInParent<PushableObject>();
        if (pushable == null)
        {
            Debug.LogError("GrabPointTrigger hat kein PushableObject als Parent!");
            enabled = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pushable.SetGrabPoint(transform);
        }
        else if (!other.CompareTag("Player") && !other.isTrigger)
        {
            // blockiert nur bei echten Objekten
            pushable.SetGrabBlocked(true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pushable.ClearGrabPoint(transform);
        }
        else
        {
            pushable.SetGrabBlocked(false);
        }
    }
}
