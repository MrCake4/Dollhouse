using System.Collections;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [Header("Foot Transforms")]
    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;

    [Header("Footstep Settings")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float raycastLength = 0.2f;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float stepCooldown = 0.5f;

    private bool leftFootDown = false;
    private bool rightFootDown = false;
    private float leftTimer = 0f;
    private float rightTimer = 0f;

    [Header("Death Sounds")]
    public AudioClip[] deathSounds;

    [Header("Pull Sounds")]
    public AudioClip[] pullSounds;

    void Update()
    {
        leftTimer += Time.deltaTime;
        rightTimer += Time.deltaTime;

        CheckFootstep(leftFoot, ref leftFootDown, ref leftTimer);
        CheckFootstep(rightFoot, ref rightFootDown, ref rightTimer);
    }

    void CheckFootstep(Transform foot, ref bool isDown, ref float timer)
    {
        if (foot == null || footstepSounds.Length == 0) return;

        Vector3 rayOrigin = foot.position + Vector3.up * 0.05f; // Slightly above foot
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastLength, groundLayers))
        {
            if (!isDown && timer >= stepCooldown)
            {
                // Foot just made contact — play footstep
                SoundEffectsManager.instance.PlayRandomSoundEffect(footstepSounds, foot, 0.3f);
                isDown = true;
                timer = 0f;
            }
        }
        else
        {
            isDown = false;
        }
    }
private bool isSoundPlaying = false;

public void PlaySingleRandomSoundEffect(AudioClip[] clips, Transform spawnTransform, float volume)
{
    if (clips == null || clips.Length == 0 || isSoundPlaying) return;

    AudioClip selectedClip = clips[Random.Range(0, clips.Length)];
    isSoundPlaying = true;

    // Use SoundEffectsManager and measure clip duration to wait
    SoundEffectsManager.instance.PlaySoundEffect(selectedClip, spawnTransform, volume);
    StartCoroutine(ResetSoundPlayingAfterDelay(selectedClip.length));
}

private IEnumerator ResetSoundPlayingAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    isSoundPlaying = false;
}
}
