using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 2f;         // Max Laufzeit in Sekunden
    public float recoveryRate = 0.5f;     // Regeneration pro Sekunde
    public float penaltyDuration = 3f;    // Timeout nach kompletter Erschöpfung

    [HideInInspector] public float currentStamina;
    private float penaltyTimer = 0f;

    [HideInInspector] public bool isPenalty = false;

    private PlayerStateManager player;

    private void Start()
    {
        currentStamina = maxStamina;
        player = GetComponent<PlayerStateManager>();
    }

    private void Update()
    {
        // Penalty aktiv
        if (isPenalty)
        {
            penaltyTimer -= Time.deltaTime;
            if (penaltyTimer <= 0f)
            {
                isPenalty = false;
                currentStamina = maxStamina; // vollständig aufladen nach Strafe
            }
            return;
        }

        // Im Jump- oder Fallstate -> Stamina bleibt gleich
        var current = player.getCurrentState;
        if (current == player.jumpState || current == player.fallState) return;

        // Wenn Spieler rennt
        if (player.isRunning && player.moveInput != Vector2.zero)
        {
            currentStamina -= Time.deltaTime;

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                isPenalty = true;
                penaltyTimer = penaltyDuration;
            }
        }
        // Wenn Spieler nicht rennt
        else
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += recoveryRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
            }
        }
    }

    public bool CanRun()
    {
        return !isPenalty && currentStamina > 0f;
    }
}
