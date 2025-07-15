using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    private GameObject carriedObject = null;
    private BoxCollider triggerCollider;
    private PlayerStateManager player;

    [SerializeField] GameObject carryBone; // Optional: Bone to attach carried object to

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
        player = GetComponent<PlayerStateManager>();
    }

    private void Update()
    {
        if (player.getCurrentState == player.deadState) {
            DropItem(); // Immer fallenlassen bei Tod
            return;
        }

        if (player.pickUpPressed)
        {
            if (carriedObject != null)
                DropItem();
            else
                TryPickUp();
        }

        if (player.interactPressed)
        {
            TryInteract();
        }
    }

    private void TryPickUp()
    {
        triggerCollider.enabled = true;

        Collider[] hits = Physics.OverlapBox(
            triggerCollider.bounds.center,
            triggerCollider.bounds.extents,
            transform.rotation,
            ~0,
            QueryTriggerInteraction.Collide
        );

        triggerCollider.enabled = false;

        foreach (Collider col in hits)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("smallObject"))
            {
                PickupItem(col.gameObject);
                return;
            }
        }
    }

    private void TryInteract()
    {
        triggerCollider.enabled = true;

        Collider[] hits = Physics.OverlapBox(
            triggerCollider.bounds.center,
            triggerCollider.bounds.extents,
            transform.rotation,
            ~0,
            QueryTriggerInteraction.Collide
        );

        triggerCollider.enabled = false;

        foreach (Collider col in hits)
        {
            if (col.CompareTag("interactable"))
            {
                Interactable interactable = col.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.interact();
                    return;
                }
            }
        }
    }

    private void PickupItem(GameObject item)
    {
        carriedObject = item;

        // Optional: Attach to bone if script exists
        item.GetComponent<AttachToBone>()?.SetTargetBone(carryBone); 

        if (item.GetComponent<AttachToBone>() != null && item.GetComponent<AttachToBone>().GetOffset() == Vector3.zero)
        {
            item.GetComponent<AttachToBone>().SetOffset(new Vector3(0, 0.5f, -1f)); // Set default offset if no AttachToBone script
        }
        // if no AttachToBone script, just set position and rotation
        else
        {
            Vector3 offset = transform.TransformDirection(new Vector3(0, 0.5f, -1f));
            carriedObject.transform.position = transform.position + offset;
            carriedObject.transform.SetParent(transform);
        }

        carriedObject.transform.rotation = transform.rotation;

        Collider col = carriedObject.GetComponent<Collider>();
        if (col) col.enabled = false;

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;
    }

    public void DropItem()
    {
        if (carriedObject == null) return;

        // if attach to bone script exists, detach it safely
        if (carriedObject.GetComponent<AttachToBone>() != null)
        {
            carriedObject.GetComponent<AttachToBone>()?.SetTargetBone(null); // Optional: Attach to bone if script exists
            Vector3 offset = transform.TransformDirection(new Vector3(0, 0.5f, -1f));
            carriedObject.transform.position = transform.position + offset;
        }

        carriedObject.transform.SetParent(null);

        Collider col = carriedObject.GetComponent<Collider>();
        if (col) col.enabled = true;

        Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;

        carriedObject = null;
    }

    public GameObject GetCarriedObject() => carriedObject;
}
