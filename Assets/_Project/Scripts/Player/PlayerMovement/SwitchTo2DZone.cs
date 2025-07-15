using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchTo2DZone : MonoBehaviour
{

    float zLerpSpeed = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStateManager>().is2DMode = true;

            // lerp player z to zone z position
            StartCoroutine(SmoothLockToZ(other.transform, transform.position.z));
            Debug.Log("2.5D now!");
        }

        // If other Object is *layer* "bigObject", find all game objects in children with tag "disableIn2D" and disable then
        // big object layer
        if (other.gameObject.layer == LayerMask.NameToLayer("bigObject"))
        {
            StartCoroutine(SmoothLockToZ(other.transform, transform.position.z));
            foreach (Transform child in other.transform)
            {
                if (child.CompareTag("disableIn2D"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    private IEnumerator SmoothLockToZ(Transform target, float targetZ)
    {
        float threshold = 0.01f;
        Vector3 pos = target.position;

        while (Mathf.Abs(target.position.z - targetZ) > threshold)
        {
            pos = target.position;
            pos.z = Mathf.Lerp(pos.z, targetZ, Time.deltaTime * zLerpSpeed);
            target.position = pos;
            yield return null;
        }

        // Snap to exact Z at the end
        pos.z = targetZ;
        target.position = pos;
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
