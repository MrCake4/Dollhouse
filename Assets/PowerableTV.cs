using UnityEngine;

public class PowerableTV : Interactable
{
    [SerializeField] bool isOn = false;
    [SerializeField] GameObject tvScreen;
    [SerializeField] AudioClip loopedTvAudioSound;
    AudioSource loopedTvSound;
    PlayerStateManager player;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerStateManager>();
    }

    public override void interact() { }

    public override void onPowerOff()
    {
        isOn = false;
        SoundEffectsManager.instance.StopSoundEffect(loopedTvSound);
    }

    public override void onPowerOn()
    {
        isOn = true;
        loopedTvSound = SoundEffectsManager.instance.PlayLoopedSoundEffect(loopedTvAudioSound, transform, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // if player is far away, turn off the tv even if it is on
        if (Vector3.Distance(player.transform.position, transform.position) > 20f)
        {
            if (isOn) tvScreen.SetActive(false);
            return;
        }

        if (isOn) tvScreen.SetActive(true);
        else tvScreen.SetActive(false);
    }
}
