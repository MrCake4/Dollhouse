using UnityEngine;
using System.Collections;

public class PlayerItemHandler : MonoBehaviour
{
    private bool wantsToPickup = false; // Nur true, wenn Spieler gerade E gedrückt hat
    private GameObject carriedObject = null;
    private BoxCollider triggerCollider;
    private Coroutine pickupWindowRoutine;

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
        //if (triggerCollider == null || !triggerCollider.isTrigger)
            //Debug.LogError("❗ Spieler braucht einen BoxCollider mit isTrigger=true");

        //triggerCollider.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (carriedObject == null)
            {
                TryStartPickupWindow();
            }
            else
            {
                DropItem();
            }
        }
    }

    private void TryStartPickupWindow()
    {
        if (pickupWindowRoutine != null) StopCoroutine(pickupWindowRoutine);
        wantsToPickup = true;
        triggerCollider.enabled = true;
        pickupWindowRoutine = StartCoroutine(PickupWindowCoroutine());
    }
    private IEnumerator PickupWindowCoroutine()
    {
        yield return new WaitForSeconds(0.2f); // 200 ms Fenster
        wantsToPickup = false;
        triggerCollider.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!wantsToPickup) return; // Nur aktiv, wenn Spieler gerade E gedrückt hat

        if (other.gameObject.layer == LayerMask.NameToLayer("smallObject"))
        {
            PickupItem(other.gameObject);
        }
    }

    private void PickupItem(GameObject item)
    {
        carriedObject = item;

        Vector3 offset = transform.TransformDirection(new Vector3(0, 0.5f, -0.6f));     // Objekt an Position hinter Spieler verschieben
        carriedObject.transform.position = transform.position + offset;

        // Optional: leicht ausrichten
        carriedObject.transform.rotation = transform.rotation;

        carriedObject.transform.SetParent(transform);           // Objekt mitbewegen
        Collider col = carriedObject.GetComponent<Collider>();
        if (col) col.enabled = false;

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        wantsToPickup = false;
        triggerCollider.enabled = false;

        if (pickupWindowRoutine != null)
        {
            StopCoroutine(pickupWindowRoutine);
            pickupWindowRoutine = null;
        }
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
