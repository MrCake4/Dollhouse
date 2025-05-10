using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    public Transform attachPoint;
    private GameObject carriedObject = null;
    private BoxCollider triggerCollider;

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
        if (triggerCollider == null || !triggerCollider.isTrigger)
            Debug.LogError("❗ Spieler braucht einen BoxCollider mit isTrigger=true");

        triggerCollider.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (carriedObject == null)
            {
                triggerCollider.enabled = true; // Trigger kurz aktivieren
            }
            else
            {
                DropItem();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (carriedObject != null) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("smallObject"))
        {
            PickupItem(other.gameObject);
        }
    }

    private void PickupItem(GameObject item)
    {
        carriedObject = item;

        // Objekt an eine Position hinter dem Spieler verschieben
        Vector3 offset = transform.TransformDirection(new Vector3(0, 0.5f, -0.6f));
        carriedObject.transform.position = transform.position + offset;

        // Optional: leicht ausrichten
        carriedObject.transform.rotation = transform.rotation;

        carriedObject.transform.SetParent(transform); // Objekt mitbewegen
        Collider col = carriedObject.GetComponent<Collider>();
        if (col) col.enabled = false;

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        triggerCollider.enabled = false;
    }

    private void DropItem()
    {
        carriedObject.transform.SetParent(null);

        Collider col = carriedObject.GetComponent<Collider>();
        if (col) col.enabled = true;

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;

        carriedObject = null;
    }
}
