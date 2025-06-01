using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UIElements;

public class AIRoomScan : MonoBehaviour
{
    // SPOTLIGHT
    [SerializeField] Light spotlight;

    // AI
    private AIStateManager ai;
    private float initialYRotation;
    private Transform currentTarget;

    // SCAN VALUES
    float viewRadius = 20f;
    float viewAngle = 30f; // changes how big the cone is
    float minViewAngle = 10f;
    float viewAngleChangeAmount = 30f; // how fast the cone gets smaller
    [SerializeField] float rotationSpeed = 0.3f;
    [SerializeField] float maxRotationAngle = 90f;
    float returnToCenterSpeed = 3f;


    // DETECTION 
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] bool startScan;

    // SHOOT SEQUENCE
    bool shotAtPlayer = false;                          // Boolean to check if the laser has shot at the player
    bool hitPlayer = false;                             // Boolean to check if the laser has hit the player     
    [SerializeField] float laserBuildupTime = 1f;       // time in seconds for how long the laser needs to shoot at the player
    float resetTimer;                                   // saves the laser buildup time to reset it after shooting  

    // DEBUG
    [SerializeField] int rayCount = 30;


    public Vector3 orientation;                         // sets the normal eye orientation, however each Window Anchor point has its own orientation controlled with its x-rotation axis
    Transform playerPosition;   // This variable is used to get the player position for the laser, so that the laser stays on the players positon

    // LASER SETTINGS
    // gets the laser from the laser Component
    LineRenderer laserLine;
    [SerializeField] float laserDrawResetTime = 0.1f; // time in seconds for how long the laser is visible
    float laserDrawReset;

    // LASER SWEEP
    private bool isReturningToCenter;
    private Quaternion centerRotation;

    private bool isSweeping = false;
    private float sweepStartTime;
    [SerializeField] float sweepDuration = 3f; // Dauer eines Sweeps in Sekunden

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get AI State manager from parent object
        ai = GetComponentInParent<AIStateManager>();

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

        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpotlight();
        UpdateLaserLine();
        UpdateOrientation();

        if (currentTarget == null && startScan)
        {
            DrawDetectionRays();
            if (!isSweeping)
            {
                // Starte Sweep
                isSweeping = true;
                sweepStartTime = Time.time;
            }

            Scan();

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
        else
        {
            ReturnToCenterPosition();
            isSweeping = false;
        }
    }

    private void UpdateOrientation()
    {
        orientation = ai.currentTargetRoom.windowAnchorPoints[ai.currentWindowIndex].transform.rotation.eulerAngles;
    }

    void Scan()
{
    Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

    foreach (Collider target in targetsInViewRadius)
    {
        Vector3 directionToTarget = (target.bounds.center - transform.position).normalized;

        // Checks if the target is inside the cone
        if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.bounds.center);

            // Offsets for different hights
            Vector3[] heightOffsets = new Vector3[]
            {
                Vector3.up * 0.5f,
                Vector3.up * 1.0f,
                Vector3.up * 1.5f
            };

            foreach (Vector3 offset in heightOffsets)
            {
                Vector3 rayOrigin = transform.position + offset;
                Vector3 targetPoint = target.bounds.center + offset;
                Vector3 rayDirection = (targetPoint - rayOrigin).normalized;

                if (!Physics.Raycast(rayOrigin, rayDirection, distanceToTarget, obstacleMask))
                {
                    currentTarget = target.transform;
                    return;
                }
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
                spotlight.colorTemperature = Mathf.Lerp(spotlight.colorTemperature, 800, Time.deltaTime * 5f);
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

                // if obstacle is tag "Destroyable" destroy it
                if (hit.collider.CompareTag("Destroyable")) Destroy(hit.collider.gameObject);

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
            // if player is hit, draw laser to current player position, if not he will just hit the obstacle 
            if (hitPlayer) shootLaser(transform.position, playerPosition.transform.position);

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