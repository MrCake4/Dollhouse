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
    [Range(0, 9), SerializeField, Tooltip("Maximum distance the camera can shift towards mouse")] float maxOffset = 3f;          // Maximum distance the camera can shift toward the mouse

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
        // Step 1: Get the desired position based on the player and offset (without float yet)
        Vector3 desiredCameraPos = playerPosition + cameraPosition;

        // Step 2: Blend between the room's anchor and the desired position
        float followStrength = currentRoom.getFollowStrength; // 0 = stick to room, 1 = stick to player
        Vector3 blendedTarget = Vector3.Lerp(currentRoom.cameraAnchorPoint.position, desiredCameraPos, followStrength);

        // Step 2.5: Slight camera follow towards mouse when right mouse button is held

        // Gamepad Support right here
        
        if (Input.GetMouseButton(1)) // Right mouse button pressed
        {
            Vector3 mouseOffset = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);  // division by 2 so that the player does not have to move his mouse to the edge of the screen

            // Normalize and scale the offset (feel free to tweak the divisor for sensitivity)
            Vector3 offsetDirection = new Vector3(mouseOffset.x, mouseOffset.y, 0).normalized;
            Vector3 mouseCameraOffset = offsetDirection * Mathf.Min(mouseOffset.magnitude / 200f, maxOffset);

            // Convert the offset to world space relative to the camera
            Vector3 worldOffset = Camera.main.transform.right * mouseCameraOffset.x +
                                Camera.main.transform.up * mouseCameraOffset.y;

            // Add this small offset to the blended target
            blendedTarget += worldOffset;
        }

        // Step 3: Clamp the blended position inside the room bounds
        Bounds roomBounds = currentRoom.roomCollider.bounds;
        float clampedX = Mathf.Clamp(blendedTarget.x, roomBounds.min.x, roomBounds.max.x);
        float clampedY = Mathf.Clamp(blendedTarget.y, roomBounds.min.y, roomBounds.max.y);
        float clampedZ = Mathf.Clamp(blendedTarget.z, roomBounds.min.z, roomBounds.max.z);
        Vector3 clampedTarget = new Vector3(clampedX, clampedY, clampedZ);

        // Step 4: Add floating effect AFTER clamping
        float floatOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        clampedTarget.y += floatOffset;

        // Step 5: Smooth camera move
        transform.position = Vector3.Lerp(transform.position, clampedTarget, Time.deltaTime * cameraSpeed);

        // Step 6: Smooth camera look at
        currentLookAt = Vector3.Lerp(currentLookAt, cameraLookAt, Time.deltaTime * cameraSpeed);
        transform.LookAt(currentLookAt, Vector3.up);
    }
}
