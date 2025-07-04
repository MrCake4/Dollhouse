using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [HideInInspector] public bool isGrounded;

    private void OnTriggerEnter(Collider other)
    {
        if (IsValidGround(other))
            isGrounded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsValidGround(other))
            isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsValidGround(other))
            isGrounded = false;
    }

    private bool IsValidGround(Collider other)
    {
        // Ignoriere alles, was den Tag oder Layer "Player" hat
        if (other.CompareTag("Player")) return false;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) return false;

        // evtl. noch Trigger ignorieren
        if (other.isTrigger) return false;

        return true;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
    }*/
}
