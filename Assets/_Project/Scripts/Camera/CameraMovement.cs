using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/*
*   How to use the camera:
*       Each room has a "Room Collider" (a collision box) and an "Anchor Point" (an empty game object)
*       The camera checks which room collider currently collides with the player
*       If the player enters a new room the camera will clip itself to the current rooms anchor point
*/

public class CameraMovement : MonoBehaviour
{

    [Header("Camera Settings")]
    [SerializeField] Vector3 cameraPosition;
    [SerializeField] Transform player;
    [SerializeField] float floatAmplitude = 1f;
    [SerializeField] float floatFrequency = 0.2f;
    private Vector3 playerPosition;

    [Header("Room to room behaviour")]
    [SerializeField] Room[] rooms;
    [SerializeField] Room currentRoom;
    Vector3 targetCameraPos;
    [SerializeField] float cameraSpeed = 3f;
    private Vector3 velocity = Vector3.zero;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetCameraPos = currentRoom.cameraAnchorPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;

        foreach(Room room in rooms){
            if(room.roomCollider.bounds.Contains(playerPosition)){
                if (room != currentRoom){
                    currentRoom = room;
                    targetCameraPos = room.cameraAnchorPoint.position;
                    Debug.Log("Room changed");
                }
            }
        }

        // TODO: IMPROVE THE FOLLOWING CODE
        
        // Camera stays at player position and adds it by the camera position
        // Step 1: Get the desired position based on the player and offset
        Vector3 desiredCameraPos = playerPosition + cameraPosition + new Vector3(0, (float) Mathf.Sin(Time.time * floatAmplitude) * floatFrequency,0);

        // Step 2: Blend between the room's anchor and the desired position
        float followStrength = 1; // 0 = stick to room, 1 = stick to player
        Vector3 blendedTarget = Vector3.Lerp(currentRoom.cameraAnchorPoint.position, desiredCameraPos, followStrength);

        // Step 3: Clamp the blended position inside the room bounds
        Bounds roomBounds = currentRoom.roomCollider.bounds;

        float clampedX = Mathf.Clamp(blendedTarget.x, roomBounds.min.x, roomBounds.max.x);
        float clampedY = Mathf.Clamp(blendedTarget.y, roomBounds.min.y, roomBounds.max.y);
        float clampedZ = Mathf.Clamp(blendedTarget.z, roomBounds.min.z, roomBounds.max.z);

        targetCameraPos = new Vector3(clampedX, clampedY, clampedZ);

        // Step 4: Smooth camera move
        transform.position = Vector3.Lerp(transform.position, cameraPosition + targetCameraPos, Time.deltaTime * cameraSpeed);


        transform.LookAt(playerPosition, Vector3.up);
    }
}
