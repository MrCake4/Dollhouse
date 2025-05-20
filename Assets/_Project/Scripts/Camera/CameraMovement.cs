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
    private Vector3 cameraLookAt;                   // The position the camera should look at
    private Vector3 currentLookAt;                  // The current position the camera is looking at
    bool lookAtPlayer;                              // Whether the camera should look at the player or not

    [Header("Room to room behaviour")]
    [SerializeField] Room[] rooms;
    [SerializeField] Room currentRoom;
    Vector3 targetCameraPos;
    [SerializeField] float cameraSpeed = 3f;
    private float currentCameraAngle; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetCameraPos = currentRoom.cameraAnchorPoint.position;
        currentCameraAngle = currentRoom.getCameraAngle;
        lookAtPlayer = currentRoom.getLookAtPlayer;
        currentLookAt = player.position;
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
                    lookAtPlayer = currentRoom.getLookAtPlayer;
                    Debug.Log("Room changed");
                }
            }
        }

        // Smoothly interpolate the camera angle
        currentCameraAngle = Mathf.Lerp(currentCameraAngle, currentRoom.getCameraAngle, Time.deltaTime * cameraSpeed);

        // Apply the smoothed camera angle to the player's Y-position
        playerPosition.y += currentCameraAngle; // Add the camera angle to the player position
        
        if (lookAtPlayer)
            cameraLookAt = playerPosition;
        else
            cameraLookAt = new Vector3(currentRoom.transform.position.x, currentCameraAngle, 0);

        // TODO: IMPROVE THE FOLLOWING CODE
        
        // Camera stays at player position and adds it by the camera position
        // Step 1: Get the desired position based on the player and offset
        Vector3 desiredCameraPos = playerPosition + cameraPosition + new Vector3(0, (float) Mathf.Sin(Time.time * floatAmplitude) * floatFrequency,0);

        // Step 2: Blend between the room's anchor and the desired position
        float followStrength = currentRoom.getFollowStrength; // 0 = stick to room, 1 = stick to player
        Vector3 blendedTarget = Vector3.Lerp(currentRoom.cameraAnchorPoint.position, desiredCameraPos, followStrength);

        // Step 3: Clamp the blended position inside the room bounds
        Bounds roomBounds = currentRoom.roomCollider.bounds;

        float clampedX = Mathf.Clamp(blendedTarget.x, roomBounds.min.x, roomBounds.max.x);
        float clampedY = Mathf.Clamp(blendedTarget.y, roomBounds.min.y, roomBounds.max.y);
        float clampedZ = Mathf.Clamp(blendedTarget.z, roomBounds.min.z, roomBounds.max.z);

        targetCameraPos = new Vector3(clampedX, clampedY, clampedZ);

        // Step 4: Smooth camera move
        transform.position = Vector3.Lerp(transform.position, cameraPosition + targetCameraPos, Time.deltaTime * cameraSpeed);
        
        // Step 5: Smooth camera look at
        currentLookAt = Vector3.Lerp(currentLookAt, cameraLookAt, Time.deltaTime * cameraSpeed);
        transform.LookAt(currentLookAt, Vector3.up);
    }
}
