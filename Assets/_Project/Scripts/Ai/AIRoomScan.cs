using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using MilkShake;

[RequireComponent(typeof(LineRenderer))]
public class AIRoomScan : MonoBehaviour
{
    // Spotlight
    [SerializeField] private Light spotlight;

    // Scan and Sweep
    [SerializeField] private float viewRadius = 20f;
    [SerializeField] private float viewAngle = 30f;
    [SerializeField] private float minViewAngle = 10f;
    [SerializeField] private float viewAngleChangeAmount = 30f;
    [SerializeField] private float rotationSpeed = 0.3f;
    [SerializeField] private float sweepDuration = 3f;
    [SerializeField] private float maxRotationAngle = 90f;
    [SerializeField] private float returnToCenterSpeed = 3f;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    [SerializeField] private ParticleSystem implosionParticles;
    [SerializeField] private Light implosionLight;

    [Header("Laser Settings")]
    [SerializeField] private float laserBuildupTime = 1f;
    [SerializeField] private float laserDrawResetTime = 0.1f;

    [Header("Camera Shake")]
    public Shaker shaker;
    public ShakePreset shakePreset;

    private LineRenderer laserLine;
    private Collider playerCollider;
    private PlayerStateManager player;

    private Quaternion centerRotation;
    private float initialYRotation;

    private Transform currentTarget;
    private Vector3 targetOffset;
    private float laserTimer;
    private float laserDrawTimer;
    private bool hitPlayer;

    public bool IsDoneSweeping { get; private set; }

    /// Audio
    [Header("Audio")]
    [SerializeField] private AudioClip[] turnLightOnOffClip;
    [SerializeField] private AudioClip ScanSweepClip;
    AudioSource sweepAudioSource;


    private void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;

        playerCollider = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Collider>();
        player = FindFirstObjectByType<PlayerStateManager>();

        centerRotation = Quaternion.Euler(Vector3.right * transform.eulerAngles.x + Vector3.up * transform.eulerAngles.y);
        initialYRotation = transform.eulerAngles.y;

        laserTimer = laserBuildupTime;
        laserDrawTimer = laserDrawResetTime;

        if (spotlight != null)
            spotlight.enabled = false;
    }

    public void BeginScanSweep()
    {
        StartCoroutine(SweepRoutine());
    }

    private IEnumerator SweepRoutine()
    {
        SetSpotlight(true);

        if (ScanSweepClip != null)
            sweepAudioSource = SoundEffectsManager.instance.PlaySoundEffect(ScanSweepClip, transform, 1f);

        IsDoneSweeping = false;

        float sweepStart = Time.time;
        while (Time.time - sweepStart < sweepDuration && currentTarget == null)
        {
            float t = (Time.time - sweepStart) * rotationSpeed * Mathf.PI * 2f / sweepDuration;
            float angle = initialYRotation + Mathf.Sin(t) * maxRotationAngle;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, angle, 0), Time.deltaTime * rotationSpeed);

            ScanForPlayer();
            UpdateSpotlight();
            yield return null;
        }

        yield return ReturnToCenter();
        SetSpotlight(false);
        IsDoneSweeping = true;
    }

    public void ScanForPlayer()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (var target in targets)
        {
            Vector3 dirToTarget = (target.bounds.center - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2f)
            {
                float distance = Vector3.Distance(transform.position, target.bounds.center);

                Vector3[] offsets = { Vector3.up * -0.9f };

                foreach (var offset in offsets)
                {
                    Vector3 rayOrigin = transform.position;
                    Vector3 rayTarget = target.bounds.center + offset;
                    Vector3 rayDir = (rayTarget - rayOrigin).normalized;

                    if (!Physics.Raycast(rayOrigin, rayDir, distance, obstacleMask))
                    {
                        currentTarget = target.transform;
                        targetOffset = offset;
                        return;
                    }
                }
            }
        }
    }

    public void FollowAndShoot()
    {
        if (currentTarget == null) return;

        Vector3 direction = (playerCollider.bounds.center - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        laserTimer -= Time.deltaTime;
        ChargeSequence();

        if (laserTimer <= 0f)
        {
            ShootLaser();
            ResetScan();
        }

        UpdateLaserLine();
        UpdateSpotlight();
    }

    private void ShootLaser()
    {
        Vector3 laserTarget = playerCollider.bounds.center + targetOffset;
        Vector3 direction = (laserTarget - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, laserTarget);

        if (!Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, obstacleMask))
        {
            laserLine.enabled = true;
            SetLaser(transform.position, laserTarget);
            hitPlayer = true;

            if (!player.isInvincible)
                player.SwitchState(player.deadState);
        }
        else
        {
            laserLine.enabled = true;
            SetLaser(transform.position, hit.point);
            hitPlayer = false;

            if (hit.collider.CompareTag("Destroyable"))
                Destroy(hit.collider.gameObject);
            else if (hit.collider.CompareTag("Generator"))
                hit.collider.GetComponent<HitableObject>()?.onHit();
        }

        if (GetComponent<LaserReflection>() is LaserReflection reflect)
            reflect.ClearLaser();

        setImplosionLight(false);
    }

    private void ResetScan()
    {
        currentTarget = null;
        laserTimer = laserBuildupTime;
    }

    private void UpdateLaserLine()
    {
        if (!laserLine.enabled) return;

        if (hitPlayer && currentTarget != null)
            SetLaser(transform.position, playerCollider.bounds.center + targetOffset);

        laserDrawTimer -= Time.deltaTime;
        if (laserDrawTimer <= 0f)
        {
            laserLine.enabled = false;
            laserDrawTimer = laserDrawResetTime;
        }
    }

    private IEnumerator ReturnToCenter()
    {
        Quaternion target = centerRotation;
        while (Quaternion.Angle(transform.rotation, target) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * returnToCenterSpeed);
            yield return null;
        }
        transform.rotation = target;
    }

    private void ChargeSequence()
    {
        if (laserTimer <= 1f && implosionParticles != null && !implosionParticles.isPlaying)
        {
            rumbleController();
            shaker?.Shake(shakePreset);

            setImplosionLight(true);
            implosionParticles.Play(true);
        }
    }

    private void rumbleController()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.75f, 1.0f);
            StartCoroutine(StopRumbleAfterSeconds(1.5f));
        }
    }

    private IEnumerator StopRumbleAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Gamepad.current?.SetMotorSpeeds(0f, 0f);
    }

    private void UpdateSpotlight()
    {
        if (spotlight == null) return;

        spotlight.type = LightType.Spot;
        spotlight.range = viewRadius;
        spotlight.intensity = 40000;
        spotlight.spotAngle = viewAngle;

        if (currentTarget != null)
        {
            spotlight.colorTemperature = Mathf.Lerp(spotlight.colorTemperature, 800, Time.deltaTime * 5f);
            viewAngle = Mathf.Max(viewAngle - viewAngleChangeAmount * Time.deltaTime, minViewAngle);
        }
        else
        {
            spotlight.colorTemperature = Mathf.Lerp(spotlight.colorTemperature, 6000, Time.deltaTime * 5f);
            viewAngle = Mathf.Lerp(viewAngle, 30f, Time.deltaTime * 5f);
        }
    }

    private void setImplosionLight(bool on)
    {
        if (implosionLight != null)
            implosionLight.enabled = on;
    }

    private void SetLaser(Vector3 start, Vector3 end)
    {
        laserLine.SetPosition(0, start);
        laserLine.SetPosition(1, end);
    }

    // Enables/Disable Spotlight
    public void SetSpotlight(bool enabled)
    {
        if (spotlight != null)
            spotlight.enabled = enabled;
        if (enabled && turnLightOnOffClip != null)
            SoundEffectsManager.instance.PlayRandomSoundEffect(turnLightOnOffClip, transform, 1f);
        else if (!enabled && turnLightOnOffClip != null)
        {
            SoundEffectsManager.instance.PlayRandomSoundEffect(turnLightOnOffClip, transform, 1f);
            if (sweepAudioSource != null)
            {
                SoundEffectsManager.instance.StopSoundEffect(sweepAudioSource);
            }
        }
    }

    public void SetHitPlayer(bool value)
    {
        hitPlayer = value;
    }

    // Public access for state machine
    public bool TargetAcquired => currentTarget != null;
    public bool IsLaserEnabled => laserLine.enabled;
    public bool PlayerWasHit => hitPlayer;
    public void UpdateLaser() => UpdateLaserLine();
}
