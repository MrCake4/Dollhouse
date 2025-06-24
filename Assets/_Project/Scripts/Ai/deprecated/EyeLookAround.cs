using UnityEngine;

public class EyeLookAround : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationRadius = 30f;        // Max degrees from center
    public float timeBetweenMoves = 3f;        // Time between new look directions
    public float rotationSpeed = 5f;           // How fast it moves to the target

    private Quaternion targetRotation;         // The next rotation
    private float timer;                        // Timer to track movement

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenMoves)
        {
            timer = 0f;
            PickNewTargetRotation();
        }

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void PickNewTargetRotation()
    {
        // Pick random x and y angles within the allowed radius
        float randomX = Random.Range(-rotationRadius, rotationRadius);
        float randomY = Random.Range(-rotationRadius, rotationRadius);

        // Keep Z rotation at 0 (no rolling)
        targetRotation = Quaternion.Euler(randomX, randomY, 0f);
    }
}