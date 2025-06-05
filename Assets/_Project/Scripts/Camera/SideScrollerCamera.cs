using UnityEngine;

public class SideScrollerCamera : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;

    public float minCameraX;
    public float maxCameraX;

    void LateUpdate()
    {
        // Follow player but clamp camera position
        float targetX = Mathf.Clamp(player.position.x, minCameraX, maxCameraX);
        Vector3 desiredPosition = new Vector3(targetX, 3f, -10f);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
