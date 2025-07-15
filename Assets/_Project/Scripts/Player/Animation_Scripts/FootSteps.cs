using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FootstepTrigger : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;                    // Referenz zu deinem AudioSource
    public AudioClip[] footstepClips;                 // Verschiedene Schritt-Geräusche zur Auswahl
    [Range(0f, 1f)] public float volume = 0.8f;

    [Header("Einstellungen")]
    public float maxAllowedY = 1f;                   // Nur Trigger unterhalb dieser Höhe lösen Sound aus
    //public string groundTag = "Ground";                // Optional: nur bei Objekten mit diesem Tag

    private void OnTriggerEnter(Collider other)
    {
        // Höhe dieses Fußtriggers prüfen (z. B. Position des Fußes)
        //if (transform.position.y > maxAllowedY) return;

        if (!other.CompareTag("Player") || other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            PlayRandomFootstep();
        }
        


        // Optional: Nur reagieren, wenn das andere Objekt "Ground" getaggt ist
        //if (!string.IsNullOrEmpty(groundTag) && !other.CompareTag(groundTag)) return;

        // Schritt-Sound abspielen (zufällig aus Liste)
        //PlayRandomFootstep();
    }

    private void PlayRandomFootstep()
    {
        if (footstepClips.Length == 0 || audioSource == null) return;

        SoundEffectsManager.instance.PlayRandomSoundEffect(footstepClips, transform, 1f);
    }
}
