using UnityEngine;

public class StaminaBarDual : MonoBehaviour
{
    [SerializeField] private RectTransform instantFill;
    [SerializeField] private RectTransform barBG;
    [SerializeField] private StaminaSystem staminaSystem;
    [SerializeField] private float lerpSpeed = 5f;

    [Header("Visibility Settings")]
    [SerializeField] private GameObject staminaBarParent;   // Gesamte Leiste (Canvas-Child)
    [SerializeField] private float hideDelay = 2f;          // Sekunden bis Ausblenden

    private float fullWidth;
    private float currentWidth;
    private float hideTimer = 0f;
    private bool isVisible = true;

    void Start()
    {
        if (!instantFill || !barBG || !staminaSystem || !staminaBarParent)
        {
            Debug.LogWarning("StaminaBarDual: Nicht alle Felder korrekt zugewiesen.");
            enabled = false;
            return;
        }

        fullWidth = barBG.rect.width;
        currentWidth = fullWidth * (staminaSystem.currentStamina / staminaSystem.maxStamina);

        barBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullWidth);
        staminaBarParent.SetActive(true); // Immer aktiv zu Beginn
    }

    void Update()
    {
        float ratio = Mathf.Clamp01(staminaSystem.currentStamina / staminaSystem.maxStamina);
        float targetWidth = fullWidth * ratio;

        currentWidth = Mathf.Lerp(currentWidth, targetWidth, Time.deltaTime * lerpSpeed);
        instantFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);

        HandleVisibility(ratio);
    }

    private void HandleVisibility(float ratio)
    {
        if (ratio < 1f)
        {
            // Stamina ist nicht voll → sofort zeigen
            hideTimer = 0f;
            if (!isVisible)
            {
                staminaBarParent.SetActive(true);
                isVisible = true;
            }
        }
        else
        {
            // Stamina ist voll → Timer hochzählen
            hideTimer += Time.deltaTime;

            if (hideTimer >= hideDelay && isVisible)
            {
                staminaBarParent.SetActive(false);
                isVisible = false;
            }
        }
    }
}
