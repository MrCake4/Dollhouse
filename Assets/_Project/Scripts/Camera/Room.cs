using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Collider roomCollider;
    public Transform cameraAnchorPoint;
    [Range (0, 1)]public float followStrength = 1; // 0 = stick to room, 1 = stick to player
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
