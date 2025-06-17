using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance;
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundEffect(AudioClip clip, Transform spawnTransform, float volume)
    {
        if (clip != null)
        {
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();

            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength); // Add a small buffer to ensure the sound finishes playing before destroying
        }
    }

    public void PlayRandomSoundEffect(AudioClip[] clips, Transform spawnTransform, float volume)
    {
        if (clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

            audioSource.clip = clips[randomIndex];
            audioSource.volume = volume;
            audioSource.Play();

            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength); // Add a small buffer to ensure the sound finishes playing before destroying
        }
    }
}
