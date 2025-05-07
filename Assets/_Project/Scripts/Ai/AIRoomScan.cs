using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class AIRoomScan : MonoBehaviour
{


    /*  LIGHT SETTINGS  */
    // sets the length of the cone
    float viewRadius = 20f;
    // changes how big the cone is
    float viewAngle = 30f;
    float maxViewAngle; // max angle of the cone, takes the resets to view angle after shot
    float minViewAngle = 8f;
    float viewAngleChangeAmount = 10f;

    [Range(0.1f, 10f), SerializeField] float laserBuildupTime = 3f;        // time in seconds for how long the laser needs to shoot at the player

    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] bool startScan = false;
    bool shotAtPlayer = false;
    bool hitPlayer = false;
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

        maxViewAngle = viewAngle;
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
                if (viewAngle > minViewAngle){      // update size of cone each second
                    viewAngle -= viewAngleChangeAmount * Time.deltaTime;
                }
            } else {
                spotlight.colorTemperature = 6000;
                viewAngle = 30f; // reset to default value
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
        laserBuildupTime -= 1* Time.deltaTime;
        // if timer runs out shoot at player
        if(laserBuildupTime < 0f){
            Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))    // (casts a ray from eye position to target position and checks if its hits an obstacle mask)
            {
                // No obstacle, hit the player
                hitPlayer = true;
                Debug.Log("Shot at player and hit!");
            }
            else
            {
                // Obstacle in the way, miss the player
                hitPlayer = false;
                Debug.Log("Shot at player but missed!");
            }
            // reset timer
            shotAtPlayer = true;
            laserBuildupTime = 3f;
            currentTarget = null; // reset target
        }
    }

    public bool getStartScan => startScan;
    public bool getShotAtPlayer => shotAtPlayer;
    public bool getHitPlayer => hitPlayer;
    
    public void setStartScan(bool startScan)
    {
        this.startScan = startScan;
    }
}
