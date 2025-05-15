using System.ComponentModel;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Collider roomCollider;
    public Transform cameraAnchorPoint;

    [Header("Camera Settings")]
    [Tooltip("The strength of the camera's follow behaviour. 0 = stick to room, 1 = stick to player")]
    [Range (0, 1), SerializeField] float followStrength = 1; // 0 = stick to room, 1 = stick to player
    [Range (-30, 30), SerializeField] float cameraAngle = 0; // The angle of the camera in the room
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getFollowStrength => followStrength;
    public float getCameraAngle => cameraAngle;
}
