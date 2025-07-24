using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

/*
*   This script is part of the AI.
*   The laser reflects of Object that have the tag "reflector"
*   calculated with vector math. (reflection, normals)
*
*   So, this abomination works as follows:
*   1. When the laser is enabled it casts a ray from the laser's 0 index toward the 1 index
*   2. If the ray hits a collider with the tag "reflector" it calculates the reflection vector
*   3. The reflection vector is then used to cast a new ray from the 1 index to the reflection point
*   4. If the ray hits another collider with the tag "reflector" it calculates the reflection vector again
*   5. This process continues until the maximum amount of lines is reached or the ray hits a collider without the tag "reflector"
*/

public class LaserReflection : MonoBehaviour
{
    [SerializeField] private int maxReflections = 10;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask reflectableLayers;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void ClearLaser()
    {
        lineRenderer.positionCount = 0;
    }

 public void ReflectLaser(int depth, GameObject initialMirror)
{
    if (depth >= maxReflections) return;

    Mirror mirror = initialMirror.GetComponent<Mirror>();
    if (mirror == null || mirror.getReflectionPoint() == null) return;

    Vector3 startPoint = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
    Vector3 direction = (mirror.getReflectionPoint().position - startPoint).normalized;

    // Ignore triggers when raycasting
    if (Physics.Raycast(startPoint, direction, out RaycastHit hit, maxDistance, reflectableLayers, QueryTriggerInteraction.Ignore))
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

        if (hit.collider.CompareTag("Reflector"))
        {
            ReflectLaser(depth + 1, hit.collider.gameObject);
        }
        else if (hit.collider.CompareTag("Generator"))
        {
            hit.collider.GetComponent<Generator>()?.onHit();
        }
        // Stop reflecting on any other collider
    }
    else
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, startPoint + direction * maxDistance);
    }
}

}