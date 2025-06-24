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
    LineRenderer laserLine;
    AIRoomScan aiRoomScan;
    int maxLines = 10;  // max amount of lines that can be reflected  from a single laser

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        aiRoomScan = GetComponent<AIRoomScan>();
    }

    // Update is called once per frame
    void Update()
    {
        // Cast a ray from the laser
        if(aiRoomScan.IsLaserEnabled && laserLine.positionCount == 2){
            ReflectLaser(0);
        }
        else if(!aiRoomScan.IsLaserEnabled && laserLine.positionCount > 2){
            ClearLaser();
        }

        Debug.DrawRay(transform.position + transform.forward, transform.forward * 10f, Color.red);
    }

    private void ReflectLaser(int i)
    {
        if (i + 1 >= laserLine.positionCount || i >= maxLines) return; // Prevent out of bounds

        Vector3 origin = laserLine.GetPosition(i);
        Vector3 direction = laserLine.GetPosition(i + 1) - origin;

        RaycastHit hit;
        Debug.DrawRay(origin, direction.normalized, Color.red, 1f);

        if (Physics.Raycast(origin, direction.normalized, out hit))                                                    
        {
            // Check if the hit object is a reflector
            if (hit.collider.CompareTag("Reflector"))
            {
                
                Debug.Log("Reflected off Collider " + i);
                // Get the normal of the surface
                Vector3 normal = hit.normal;

                // Calculate the reflection vector
                Vector3 reflection = Vector3.Reflect(direction.normalized, normal);                                     

                // Add the hit point to the LineRenderer
                RaycastHit hit2;
                 Vector3 origin2 = laserLine.GetPosition(i+1);
                if(Physics.Raycast(origin2, reflection, out hit2))                                              
                {
                    if (hit2.collider.CompareTag("Reflector")){
                        laserLine.positionCount++;
                        laserLine.SetPosition(laserLine.positionCount - 1, hit2.point);
                    }
                    else
                    {
                        laserLine.positionCount++;
                        laserLine.SetPosition(laserLine.positionCount - 1, reflection * 10f);
                    }
                    Debug.DrawRay(hit.point, reflection * 10f, Color.blue, 1f);
                }
                ReflectLaser(i + 1);
            }
        }
    }

    public void ClearLaser()
    {
        laserLine.positionCount = 2;
    }
}
