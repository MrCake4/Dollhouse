using UnityEngine;

public class StaminaBarDual : MonoBehaviour
{
    [SerializeField] private RectTransform instantFill;
    [SerializeField] private RectTransform barBG;
    [SerializeField] private StaminaSystem staminaSystem;
    [SerializeField] private float lerpSpeed = 5f;

    [Header("Visibility Settings")]
    [SerializeField] private CanvasGroup canvasGroup;       // CanvasGroup für sanftes Ein-/Ausblenden
    [SerializeField] private float hideDelay = 0.6f;          // Sekunden bis Ausblenden
    [SerializeField] private float fadeSpeed = 7f;          // Geschwindigkeit für Fading

    private float fullWidth;
    private float currentWidth;
    private float hideTimer = 0f;
    private float targetAlpha = 1f;

    void Start()
    {
        if (!instantFill || !barBG || !staminaSystem || !canvasGroup)
        {
            Debug.LogWarning("StaminaBarDual: Nicht alle Felder korrekt zugewiesen.");
            enabled = false;
            return;
        }

        fullWidth = barBG.rect.width;
        currentWidth = fullWidth * (staminaSystem.currentStamina / staminaSystem.maxStamina);
        barBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullWidth);

        canvasGroup.alpha = 1f; // Start sichtbar
    }

    void Update()
    {
        float ratio = Mathf.Clamp01(staminaSystem.currentStamina / staminaSystem.maxStamina);
        float targetWidth = fullWidth * ratio;

        currentWidth = Mathf.Lerp(currentWidth, targetWidth, Time.deltaTime * lerpSpeed);
        instantFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);

        HandleVisibility(ratio);
        FadeVisibility();
    }

    private void HandleVisibility(float ratio)
    {
        if (ratio < 1f)
        {
            hideTimer = 0f;
            targetAlpha = 1f; // Sichtbar
        }
        else
        {
            hideTimer += Time.deltaTime;
            if (hideTimer >= hideDelay)
            {
                targetAlpha = 0f; // Unsichtbar
            }
        }
    }

    private void FadeVisibility()
    {
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }
}
