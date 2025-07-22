using UnityEngine;


/*
*   This script is responsible for managing ambient sounds in the game.
*   It will handle playing various ambient sounds like house creaks and wind.
*   It randomly plays sounds at a random time around the player.
*/
public class AmbienceSoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] houseCreaks;
    [SerializeField] AudioClip[] windSounds;

    [SerializeField] float minTimeBetweenSounds = 5f;
    [SerializeField] float maxTimeBetweenSounds = 10f;
    float currentTimeBetweenSounds;
    Transform offset;

    void Awake()
    {
        offset = new GameObject("SoundOffset").transform;
        currentTimeBetweenSounds = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
    }

    void Update()
    {
        currentTimeBetweenSounds -= Time.deltaTime;

        if (currentTimeBetweenSounds <= 0f)
        {
            Transform playerOffset = getRandomPositionAroundPlayer();
            SoundEffectsManager.instance.PlayRandomSoundEffect(houseCreaks, playerOffset, 1f);
            currentTimeBetweenSounds = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
        }
    }

    Transform getRandomPositionAroundPlayer()
    {
        Transform playerPosition = FindAnyObjectByType<PlayerStateManager>().transform;
        Vector3 randomPosition = playerPosition.position + Random.insideUnitSphere * 10f; // Random position around the player
        randomPosition.y = 0f; // Keep it on the ground level
        
        offset.position = randomPosition;
        return offset;
    }

}
