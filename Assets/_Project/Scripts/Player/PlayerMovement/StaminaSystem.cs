using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 5f;
    public float recoveryRate = 0.5f;
    public float penaltyDuration = 2f;

    [Header("Stamina Consumption")]
    public float staminaDrainRate = 1f;      // pro Sekunde beim Rennen
    public float jumpStaminaCost = 0.7f;     // fixer Abzug beim Springen

    [HideInInspector] public float currentStamina;
    [HideInInspector] public bool isPenalty;

    [HideInInspector] public float defaultStaminaDrainRate;
    [HideInInspector] public float defaultJumpStaminaCost;

    private float penaltyTimer;
    private PlayerStateManager player;

    private void Start()
    {
        currentStamina = maxStamina;
        player = GetComponent<PlayerStateManager>();
        defaultJumpStaminaCost = jumpStaminaCost;
        defaultStaminaDrainRate = staminaDrainRate;
    }

    private void FixedUpdate()
    {
        if (isPenalty)
        {
            penaltyTimer -= Time.fixedDeltaTime;
            if (penaltyTimer <= 0f)
            {
                isPenalty = false;
                currentStamina = maxStamina;
                Debug.Log("Stamina Penalty Ended");
            }
            return;
        }

        HandleStamina();
    }

    private void HandleStamina()
    {
        var state = player.getCurrentState;

        // Kein Verbrauch im Sprung oder Fall
        if (state == player.jumpState || state == player.fallState)
            return;

        // Verbrauch beim Rennen → prüfe, ob Spieler WIRKLICH im RunState ist
        if (state == player.runState && player.GetHorizontalSpeed() > player.walkSpeed + 0.5f)
        {
            currentStamina -= staminaDrainRate * Time.fixedDeltaTime;

            if (currentStamina <= 0f)
            {
                StartPenalty("Running");
            }
        }
        // Regeneration
        else if (currentStamina < maxStamina)
        {
            currentStamina += recoveryRate * Time.fixedDeltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }


    public bool CanRun()
    {
        return !isPenalty && currentStamina > 0f;
    }

    public void ConsumeJumpCost()
    {
        if (isPenalty) return;

        currentStamina -= jumpStaminaCost;

        if (currentStamina <= 0f)
        {
            StartPenalty("Jump");
        }
    }

    private void StartPenalty(string source)
    {
        isPenalty = true;
        currentStamina = 0f;
        penaltyTimer = penaltyDuration;
        Debug.Log($"Stamina Penalty Activated (by {source})");
    }
}
