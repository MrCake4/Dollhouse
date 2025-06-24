using UnityEngine;

public class SwitchTo2DZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStateManager>().is2DMode = true;
            Debug.Log("2.5D now!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStateManager>().is2DMode = false;
            Debug.Log("back to 3D now!");
        }
    }
}
