using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UIElements;

public class AIRoomScan : MonoBehaviour
{
    [SerializeField] Light spotlight;


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
    Vector3 targetOffset = Vector3.zero; // colliderOffset for the target position, so that the ray is not exactly at the center of the eye


    // SHOOT SEQUENCE
    bool shotAtPlayer = false;
    bool hitPlayer = false;
    [SerializeField] float laserBuildupTime = 1f;        // time in seconds for how long the laser needs to shoot at the player
    float resetTimer;

    // Particles
    [SerializeField] ParticleSystem implosionParticles; // particles that are spawned when the laser is shot

    // DEBUG
    [SerializeField] int rayCount = 30;

    private float initialYRotation;
    private Transform currentTarget;


    // Orientation of the eye, given in x y z coordinates
    // +x changes the view of the eye down, -x up
    // TODO: maybe change to a quaternion
    public Vector3 orientation;
    Transform playerPosition;   // This variable is used to get the player position for the laser, so that the laser stays on the players positon

    // Laser settings
    // gets the laser from the object
    LineRenderer laserLine;
    [SerializeField] float laserDrawResetTime = 0.1f; // time in seconds for how long the laser is visible
    float laserDrawReset;
    [SerializeField] Light implosionLight; // light that is spawned when the implosion particles are played

    private bool isReturningToCenter;
    private Quaternion centerRotation;

    private bool isSweeping = false;
    private float sweepStartTime;
    [SerializeField] float sweepDuration = 3f; // Dauer eines Sweeps in Sekunden

    // MANAGERS
    private PlayerStateManager player;

    Collider targetCollider;

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

        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;

        // get player state manager
        player = FindFirstObjectByType<PlayerStateManager>();

        targetCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
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
        else if (currentTarget != null && !hitPlayer)
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
                // we have to keep these between -0.9 and 0.9, else the raycast will not hit the target 
                Vector3[] heightOffsets = new Vector3[]
                {
                //Vector3.up * 0f,// Center
                Vector3.up * -0.9f, // feet
                //Vector3.up * 0.9f,    // head
                };

                foreach (Vector3 colliderOffset in heightOffsets)
                {
                    Vector3 rayOrigin = transform.position;
                    Vector3 targetPoint = target.bounds.center + colliderOffset;
                    Vector3 rayDirection = (targetPoint - rayOrigin).normalized;

                    Debug.DrawRay(rayOrigin, rayDirection * distanceToTarget, Color.red, 10f);

                    if (!Physics.Raycast(rayOrigin, rayDirection, distanceToTarget, obstacleMask))
                    {
                        currentTarget = target.transform;
                        targetOffset = colliderOffset; // Save the colliderOffset for the target position
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
        Vector3 direction = (targetCollider.bounds.center - transform.position).normalized;

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

    // triggers the implosion particles right before  the laser is shot
    void chargeSequence()
    {
        if(laserBuildupTime <=1f && implosionParticles != null && !implosionParticles.isPlaying)
        {
            implosionLight.enabled = true; // enable the implosion light
            implosionParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // ensure it's reset
            implosionParticles.Play();
        }
    }

    // activates when the laser sees the player
    // hits the player if there is no obstacle in the way, else it misses and continues to scan
    void ShootSequence()
    {
        // if player is detected by one of the rays, shoot at player, else if there is an obstacle between player and ray, shoot but miss
        laserBuildupTime -= Time.deltaTime;

        // play implosion particles when the laser is charging
        chargeSequence();

        // if timer runs out shoot at player
        if (laserBuildupTime < 0f)
        {
            // Vector3 to target added with the targetOffset
            Vector3 laserTarget = targetCollider.bounds.center + targetOffset;
            Vector3 directionToTarget = (laserTarget - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, laserTarget);

            RaycastHit hit;

            // send the player into oblivion if there is no obstacle in the way
            if (!Physics.Raycast(transform.position, directionToTarget, out hit, distanceToTarget, obstacleMask))
            {
                laserLine.enabled = true;
                shootLaser(transform.position, laserTarget);

                hitPlayer = true;
                Debug.Log("Shot at player and hit!");
                if (!player.isInvincible) player.SwitchState(player.deadState);
            }
            else
            {
                // Obstacle in the way — we hit something in the obstacleMask, uses hit point to get obstacle position
                laserLine.enabled = true;
                shootLaser(transform.position, hit.point);

                // if obstacle is tag "Destroyable" destroy it
                if (hit.collider.CompareTag("Destroyable")) Destroy(hit.collider.gameObject);
                // if obstacle has tag "Generator" call onHit method
                else if (hit.collider.CompareTag("Generator"))
                {
                    hit.collider.GetComponent<HitableObject>().onHit();
                }

                hitPlayer = false;
                Debug.Log("Shot at player but missed!");
            }
            // reset timer
            shotAtPlayer = true;
            laserBuildupTime = resetTimer;
            currentTarget = null; // reset target
            implosionLight.enabled = false; // disable the implosion light

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
            if (hitPlayer) shootLaser(transform.position, targetCollider.bounds.center + targetOffset);

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
    
    public void setHitPlayer(bool hitPlayer)
    {
        this.hitPlayer = hitPlayer;
    }
}
