using UnityEngine;

public class InteractableDuck : Interactable
{
    [SerializeField] AudioClip[] duckSounds;

    // Trigger options
    [SerializeField] RoomContainer roomToTrigger;
    [SerializeField] AIStateManager ai;
    [SerializeField] bool triggered = false;
    float coolDown = 2f; // Cooldown to prevent multiple triggers in a short time
    float coolDownTimer = 0f;

    void Awake()
    {
        coolDownTimer = coolDown; // Initialize cooldown timer
    }

    public override void interact()
    {
        if (SoundEffectsManager.instance != null)
        {
            SoundEffectsManager.instance.PlayRandomSoundEffect(duckSounds, transform, 0.2f);
        }
        TriggerAI();
    }

    void TriggerAI()
    {
        if (!ai.enabled) ai.enabled = true;
        if (roomToTrigger != null && !roomToTrigger.triggered && ai.getCurrentState == ai.idleState)
        {
            triggered = true;
            roomToTrigger.triggered = true;
        }
    }

    public override void onPowerOff()
    {
        throw new System.NotImplementedException();
    }

    public override void onPowerOn()
    {
        throw new System.NotImplementedException();
    }

    void Update()
    {
        if (triggered)
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer <= 0)
            {
                triggered = false;
                coolDownTimer = coolDown; // Reset cooldown timer
            }
        }
    }
}
