using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillBar;         // Sichtbare Leiste
    [SerializeField] private StaminaSystem staminaSystem;   // Referenz zum Player
    [SerializeField] private float lerpSpeed = 5f;           // Geschwindigkeit der „Glättung“

    private Vector3 initialScale;
    private float currentDisplayedRatio = 1f;                // aktuell sichtbare Länge (0.0 – 1.0)

    void Start()
    {
        if (fillBar == null || staminaSystem == null)
        {
            Debug.LogWarning("StaminaBar: fillBar oder staminaSystem nicht zugewiesen.");
            enabled = false;
            return;
        }

        initialScale = fillBar.localScale;
        currentDisplayedRatio = staminaSystem.currentStamina / staminaSystem.maxStamina;
    }

    void Update()
    {
        float targetRatio = staminaSystem.currentStamina / staminaSystem.maxStamina;
        targetRatio = Mathf.Clamp01(targetRatio);

        // Sanft interpolieren zwischen aktueller Anzeige und Zielwert
        currentDisplayedRatio = Mathf.Lerp(currentDisplayedRatio, targetRatio, Time.deltaTime * lerpSpeed);

        fillBar.localScale = new Vector3(initialScale.x * currentDisplayedRatio, initialScale.y, initialScale.z);
    }
}
