using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class AIRoomScan : MonoBehaviour
{
    [SerializeField] Light spotlight;


    // SCAN VALUES
    float viewRadius = 20f;

    float viewAngle = 30f; // changes how big the cone is
    float minViewAngle = 8f;
    float viewAngleChangeAmount = 10f;
    [SerializeField] float rotationSpeed = 0.3f;
    [SerializeField] float maxRotationAngle = 90f;
    float returnToCenterSpeed = 3f;


    // DETECTION 
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] bool startScan;


    // SHOOT SEQUENCE
    bool shotAtPlayer = false;
    bool hitPlayer = false;
    [SerializeField] float laserBuildupTime = 1f;        // time in seconds for how long the laser needs to shoot at the player
    float resetTimer;


    // DEBUG
    [SerializeField] int rayCount = 30;

    private float initialYRotation;
    private Transform currentTarget;

    // Orientation of the eye, given in x y z coordinates
    // +x changes the view of the eye down, -x up
    // TODO: maybe change to a quaternion
    public Quaternion orientation;

    // Laser settings
    // gets the laser from the object
    LineRenderer laserLine;
    [SerializeField] float laserDrawResetTime = 5; // time in seconds for how long the laser is visible
    float laserDrawReset;

    private bool isReturningToCenter;
    private Quaternion centerRotation;

    private bool isSweeping = false;
    private float sweepStartTime;
    [SerializeField] float sweepDuration = 3f; // Dauer eines Sweeps in Sekunden

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // saves laser buildup time to reset it after shooting
        resetTimer = laserBuildupTime;
        // sets what the ray is looking for
        targetMask = LayerMask.GetMask("Player");
        // sets what blocks the ray
        obstacleMask = LayerMask.GetMask("Obstacle", "Ground");
        initialYRotation = transform.eulerAngles.y;

        // make laser invisible at start
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
        laserDrawReset = laserDrawResetTime;

        centerRotation = Quaternion.Euler(orientation.x, initialYRotation, 0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpotlight();
        UpdateLaserLine();
        if (currentTarget == null && startScan)
        {
            DrawDetectionRays();
            if (!isSweeping)
            {
                // Starte Sweep
                isSweeping = true;
                sweepStartTime = Time.time;
            }

            float elapsed = Time.time - sweepStartTime;
            if (elapsed <= sweepDuration)
            {
                // Aktiver Sweep
                float targetRotationAngle = initialYRotation + Mathf.Sin(elapsed * rotationSpeed * Mathf.PI * 2f / sweepDuration) * maxRotationAngle;
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.Euler(orientation.x, targetRotationAngle, 0),
                    Time.deltaTime * rotationSpeed
                );
                Scan();
            }
            else
            {
                // Sweep vorbei → zurück zur Mittelposition
                ReturnToCenterPosition();
                if (!isReturningToCenter)
                {
                    // Nach Rückkehr: beende Scan
                    isSweeping = false;
                    startScan = false;
                }
            }
        }
        else if (currentTarget != null)
        {
            FollowTarget();
            DrawDetectionRays();
            ShootSequence();
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

    private void ReturnToCenterPosition()
    {
        isReturningToCenter = true;
        transform.rotation = Quaternion.Slerp(transform.rotation, centerRotation, Time.deltaTime * returnToCenterSpeed);

        if (Quaternion.Angle(transform.rotation, centerRotation) < 0.5f)
        {
            transform.rotation = centerRotation;
            isReturningToCenter = false;
        }
    }

    void FollowTarget()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
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
            if (currentTarget != null)
            {
                // Narrow focus when targeting
                spotlight.colorTemperature = Mathf.Lerp(spotlight.colorTemperature, 15000, Time.deltaTime * 5f);
                viewAngle = Mathf.Max(viewAngle - (viewAngleChangeAmount * Time.deltaTime), minViewAngle);
            }
            else
            {
                // Return to normal when scanning
                spotlight.colorTemperature = Mathf.Lerp(spotlight.colorTemperature, 6000, Time.deltaTime * 5f);
                viewAngle = Mathf.Lerp(viewAngle, 30f, Time.deltaTime * 5f);
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

    // activates when the laser sees the player
    // hits the player if there is no obstacle in the way, else it misses and continues to scan
    // TODO: Update scan state to stay om scan until shotAtPlayer is true
    // TODO: if the player is hit, player dies
    void ShootSequence()
    {
        // if player is detected by one of the rays, shoot at player, else if there is an obstacle between player and ray, shoot but miss
        laserBuildupTime -= Time.deltaTime;
        // if timer runs out shoot at player
        if (laserBuildupTime < 0f)
        {
            Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

            RaycastHit hit;

            if (!Physics.Raycast(transform.position, directionToTarget, out hit, distanceToTarget, obstacleMask))
            {
                // No obstacle in the way — we can see the player
                laserLine.enabled = true;
                shootLaser(transform.position, currentTarget.position);

                hitPlayer = true;
                Debug.Log("Shot at player and hit!");
            }
            else
            {
                // Obstacle in the way — we hit something in the obstacleMask, uses hit point to get obstacle position
                laserLine.enabled = true;
                shootLaser(transform.position, hit.point);

                hitPlayer = false;
                Debug.Log("Shot at player but missed!");
            }
            // reset timer
            shotAtPlayer = true;
            laserBuildupTime = resetTimer;
            currentTarget = null; // reset target

            LaserReflection laserReflection = GetComponent<LaserReflection>();
            laserReflection.ClearLaser(); // clear laser
        }
    }

    private void shootLaser(Vector3 start, Vector3 end)
    {
        laserLine.SetPosition(0, start);
        laserLine.SetPosition(1, end);
    }

    // resets the laser after a certain time
    private void UpdateLaserLine()
    {
        if (laserLine.enabled)
        {
            laserDrawReset -= Time.deltaTime;
            if (laserDrawReset <= 0f)
            {
                laserLine.enabled = false;
                laserDrawReset = laserDrawResetTime; // reset time
            }
        }
    }

    public bool getShotAtPlayer => shotAtPlayer;
    public bool getHitPlayer => hitPlayer;
    public bool getLaserEnabled => laserLine.enabled;
    public bool getStartScan => startScan;
    public void setStartScan(bool startScan)
    {
        this.startScan = startScan;
    }
}
