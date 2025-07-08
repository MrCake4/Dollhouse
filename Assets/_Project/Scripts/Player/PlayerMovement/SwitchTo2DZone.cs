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

        // If other Object is *layer* "bigObject", find all game objects in children with tag "disableIn2D" and disable then
        // big object layer
        if (other.gameObject.layer == LayerMask.NameToLayer("bigObject"))
        {
            foreach (Transform child in other.transform)
            {
                if (child.CompareTag("disableIn2D"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStateManager>().is2DMode = false;
            Debug.Log("back to 3D now!");
        }

        // If other Object is tag "bigObject", find all game objects in children with tag "disableIn2D" and enable them
        if (other.gameObject.layer == LayerMask.NameToLayer("bigObject"))
        {
            foreach (Transform child in other.transform)
            {
                if (child.CompareTag("disableIn2D"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }
}
