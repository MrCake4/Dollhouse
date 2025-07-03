using UnityEngine;

public class GrabPointTrigger : MonoBehaviour
{
    private PushableObject pushable;
    private bool isBlocked = false;
    private bool playerInside = false;

    public bool IsAvailable => playerInside && !isBlocked;

    void Awake()
    {
        pushable = GetComponentInParent<PushableObject>();
        if (pushable == null)
        {
            Debug.LogError("GrabPointTrigger hat kein PushableObject als Parent!");
            enabled = false;
        }
        else
        {
            pushable.RegisterGrabPoint(this); // Neuer Schritt: Registrieren
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
        else if (!other.isTrigger && other.gameObject != pushable.gameObject)
        {
            isBlocked = true;
        }
    
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
        else
        {
            isBlocked = false;
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
