using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{

    [Header("Camera Settings")]
    [SerializeField] Vector3 cameraPosition;
    [SerializeField] Transform player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // maybe transform.lookAt(Vector3) benutzen
        transform.position = player.transform.position + cameraPosition;
    }
}
