using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepSounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void footStep()
    {
        SoundEffectsManager.instance.PlayRandomSoundEffect(footstepSounds, transform, 0.5f);
        }
}
