using UnityEngine;

public class PlayerInteractionBox : MonoBehaviour
{
    private BoxCollider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<BoxCollider>();
        if (!triggerCollider || !triggerCollider.isTrigger)
        {
            Debug.LogError("❗PlayerInteractionBox benötigt einen BoxCollider mit isTrigger=true");
        }
    }

    // Gibt das näheste Objekt mit bestimmtem Tag zurück
    public GameObject FindClosestWithTag(string tag)
    {
        triggerCollider.enabled = true;

        Collider[] hits = Physics.OverlapBox(
            triggerCollider.bounds.center,
            triggerCollider.bounds.extents,
            transform.rotation,
            ~0, // alle Layer
            QueryTriggerInteraction.Collide
        );

        triggerCollider.enabled = false;

        GameObject closest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.CompareTag(tag))
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = hit.gameObject;
                }
            }
        }

        return closest;
    }

    // Beispiel: Finde Objekt mit bestimmtem Layer (z.B. "smallObject")
    public GameObject FindClosestWithLayer(LayerMask layer)
    {
        triggerCollider.enabled = true;

        Collider[] hits = Physics.OverlapBox(
            triggerCollider.bounds.center,
            triggerCollider.bounds.extents,
            transform.rotation,
            layer,
            QueryTriggerInteraction.Collide
        );

        triggerCollider.enabled = false;

        GameObject closest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit.gameObject;
            }
        }

        return closest;
    }
}
