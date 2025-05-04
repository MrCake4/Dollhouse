using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class AIRoomScan : MonoBehaviour
{
    // sets the length of the cone
    float viewRadius = 20f;
    // changes how big the cone is
    float viewAngle = 30f;

    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] Boolean startScan = false;
    [SerializeField] Light spotlight;
    [SerializeField] int rayCount = 30;
    [SerializeField] float rotationSpeed = 0.3f;
    [SerializeField] float maxRotationAngle = 90f;
    private float initialYRotation;
    private Transform currentTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // sets what the ray is looking for
        targetMask = LayerMask.GetMask("Player");
        // sets what blocks the ray
        obstacleMask = LayerMask.GetMask("Obstacle", "Ground");
        initialYRotation = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpotlight();
       

        if (currentTarget == null && startScan)
        {
            
            // Calculates the rotation angle
            float targetRotationAngle = initialYRotation + Mathf.Sin(Time.time * rotationSpeed) * maxRotationAngle;
            // Rotates the object
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotationAngle, 0), Time.deltaTime * rotationSpeed);
             DrawDetectionRays();
            Scan();
        }
        else if (currentTarget != null)
        {
             DrawDetectionRays();
            FollowTarget();
        }
    }

    void Scan()
    {   
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            // Filters if things are inside the cone
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                // Checks if there are obstacles
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    currentTarget = target.transform;
                    break;
                }
            }
        }
    }

    void FollowTarget()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void UpdateSpotlight()
    {
        if (spotlight != null)
        {
            spotlight.type = LightType.Spot;
            spotlight.spotAngle = viewAngle;
            spotlight.range = viewRadius;
            spotlight.intensity = 40000;
            if(currentTarget != null) 
            {
                spotlight.colorTemperature -= 100;
            } else {
                spotlight.colorTemperature = 6000;
            }
        }
    }

    void DrawDetectionRays()
    {
        float halfAngle = viewAngle / 2f;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -halfAngle + (viewAngle / (rayCount - 1)) * i;

            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, viewRadius, targetMask | obstacleMask))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
            }
            else
            {
                Debug.DrawRay(transform.position, direction * viewRadius, Color.green);
            }
        }
    }

    void ShootSequence()
    {
        
    }

    public bool getStartScan => startScan;
    
    public void setStartScan(bool startScan)
    {
        this.startScan = startScan;
    }
}
