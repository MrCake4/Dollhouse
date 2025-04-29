using System;
using UnityEngine;

public class AIRoomScan : MonoBehaviour
{
    [SerializeField] private Boolean detected;
    [SerializeField]float viewRadius = 20f;
    [SerializeField]float viewAngle = 30f;
    [SerializeField]LayerMask targetMask;
    [SerializeField]LayerMask obstacleMask;
    [SerializeField]Boolean startScan = false;
    [SerializeField]Light spotlight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetMask = LayerMask.GetMask("Player");
        obstacleMask = LayerMask.GetMask("Obstacle", "Ground");
    }

    // Update is called once per frame
    void Update()
    {
        updateSpotlight();
        if(startScan == true)
        {
            scanRoom();
        }
    }

    void scanRoom()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                // Sichtlinie prüfen (ob Hindernis dazwischen)
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    Debug.Log("Ziel sichtbar: " + target.name);
                    Debug.DrawLine(transform.position, target.transform.position, Color.green);
                    detected = true;
                }
                else
                {
                    Debug.DrawLine(transform.position, target.transform.position, Color.yellow);
                    detected = false;
                }
            }
        }

    }

    void updateSpotlight()
    {
        if (spotlight != null)
        {
            spotlight.type = LightType.Spot;
            spotlight.spotAngle = viewAngle;
            spotlight.range = viewRadius;
        }
    }
}
