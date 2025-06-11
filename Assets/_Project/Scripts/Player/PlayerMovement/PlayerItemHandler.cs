using UnityEngine;
using System.Collections;

public class PlayerItemHandler : MonoBehaviour 
{
    private bool wantsToPickup = false; // Nur true, wenn Spieler gerade E gedrückt hat
    private GameObject carriedObject = null;
    private BoxCollider triggerCollider;
    private Coroutine pickupWindowRoutine;
    Interactable interactable;

    private PlayerStateManager player;

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
        //if (triggerCollider == null || !triggerCollider.isTrigger)
        //Debug.LogError("❗ Spieler braucht einen BoxCollider mit isTrigger=true");

        //triggerCollider.enabled = false;

        // get player state manager
        player = GetComponent<PlayerStateManager>();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button2)) && player.getCurrentState != player.deadState)
        {
            if (carriedObject == null) // Try picking up
            {
                if (LookForPickupItem()) return; // Prioritize item pickup

                if (LookForInteractable()) // Only interact if no item found
                {
                    interactable.interact();
                }
            }
            else if (LookForInteractable()) // Interact while carrying something
            {
                interactable.interact();
            }
            else
            {
                DropItem(); // Drop item if no interaction found
            }
        }


        // This is called every frame, SOURCE FOR REFACTORING
        if (player.getCurrentState == player.deadState) DropItem(); // Wenn Spieler tot ist, Objekt fallen lassen
    }

    private bool LookForPickupItem()
{
    triggerCollider.enabled = true;

    Collider[] hits = Physics.OverlapBox(
        triggerCollider.bounds.center,
        triggerCollider.bounds.extents,
        transform.rotation,
        ~0,
        QueryTriggerInteraction.Collide
    );

    foreach (Collider col in hits)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("smallObject"))
        {
            PickupItem(col.gameObject);
            triggerCollider.enabled = false;
            return true;
        }
    }

    triggerCollider.enabled = false;
    return false;
}


    public bool GetWantsToPickup => wantsToPickup;          //Nur Getter für Interaction wenn man E drückt

    private bool LookForInteractable() {                //Findet Box einen Interactable?

        if (triggerCollider == null) return false;

        triggerCollider.enabled = true;

        Collider[] hits = Physics.OverlapBox(
            triggerCollider.bounds.center,
            triggerCollider.bounds.extents,
            transform.rotation,
            ~0,
            QueryTriggerInteraction.Collide
        );

        foreach (Collider col in hits)
        {
            if (col.CompareTag("interactable"))
            {
                interactable = col.GetComponent<Interactable>();
                triggerCollider.enabled = false;
                return true;
            }
        }

        triggerCollider.enabled = false;
        return false;
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

        Vector3 offset = transform.TransformDirection(new Vector3(0, 0.5f, -1f));     // Objekt an Position hinter Spieler verschieben
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
        if (carriedObject == null) return; // Nichts zum Fallenlassen
        carriedObject.transform.SetParent(null);

        Collider col = carriedObject.GetComponent<Collider>();
        if (col) col.enabled = true;

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;

        carriedObject = null;
    }
    
    public GameObject GetCarriedObject()
    {
        return carriedObject;
    }
}
