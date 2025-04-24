using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{

    [Header("Camera Settings")]
    [SerializeField] Vector3 cameraPosition;
    [SerializeField] Transform player;
    private Vector3 playerPosition;

    [Header("Room to room behaviour")]
    [SerializeField] Room[] rooms;
    [SerializeField] Room currentRoom;
    [SerializeField] Vector3 targetCameraPos;
    [SerializeField] float cameraSpeed = 3f;
    
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
        // maybe transform.lookAt(Vector3) benutzen

        // Camera stays at player position and adds it by the camera position
        transform.position = Vector3.Lerp(transform.position, targetCameraPos + cameraPosition, Time.deltaTime * cameraSpeed);
    }
}
