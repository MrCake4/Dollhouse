using System;
using UnityEngine;

public class AIRoomScan : MonoBehaviour
{
    // sets the length of the cone
    float viewRadius = 20f;
    // changes how big the cone is
    float viewAngle = 30f;

    [SerializeField]LayerMask targetMask;
    [SerializeField]LayerMask obstacleMask;
    [SerializeField]Boolean startScan = true;
    [SerializeField]Light spotlight;

    private Transform currentTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // sets what the ray is looking for
        targetMask = LayerMask.GetMask("Player");
        // sets what blocks the ray
        obstacleMask = LayerMask.GetMask("Obstacle", "Ground");
    }

    // Update is called once per frame
    void Update()
    {
        updateSpotlight();
        if(startScan == true)
        {
            scan();
        }

        if (currentTarget != null)
        {
            FollowTarget();
        }
    }

    void scan()
    {   
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            // Filters if things are in a
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                // Checks if there are obstacles
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    Debug.DrawLine(transform.position, target.transform.position, Color.green);
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


    void updateSpotlight()
    {
        if (spotlight != null)
        {
            spotlight.type = LightType.Spot;
            spotlight.spotAngle = viewAngle;
            spotlight.range = viewRadius;
            spotlight.intensity = 40000;
            if(currentTarget != null) 
            {
                spotlight.colorTemperature -= 500;
            } else {
                spotlight.colorTemperature = 6000;
            }
        }
    }
}
