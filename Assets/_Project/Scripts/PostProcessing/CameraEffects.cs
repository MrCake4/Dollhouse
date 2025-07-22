using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private Volume globalVolume; // Assign your HDRP Global Volume here

    private Vignette vignette;
    private ColorAdjustments colorAdjustments;

    private float normalVignette = 0.2f;
    private float intenseVignette = 0.4f;

    private float normalSaturation = 0f;
    private float intenseSaturation = -50f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (globalVolume.profile.TryGet(out Vignette v))
            vignette = v;
        else
            Debug.LogError("No Vignette override found in the assigned Volume!");

        if (globalVolume.profile.TryGet(out ColorAdjustments ca))
            colorAdjustments = ca;
        else
            Debug.LogError("No ColorAdjustments override found in the assigned Volume!");

        if (vignette != null) vignette.intensity.value = normalVignette;
        if (colorAdjustments != null) colorAdjustments.saturation.value = normalSaturation;
    }

    public void ApplyIntenseEffect(float duration = 0.5f)
    {
        StartSmoothTransition(intenseVignette, intenseSaturation, duration);
    }
    public void RevertEffect(float duration = 0.5f)
    {
        StartSmoothTransition(normalVignette, normalSaturation, duration);
    }

    private void StartSmoothTransition(float targetVignette, float targetSaturation, float duration)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(SmoothTransitionRoutine(targetVignette, targetSaturation, duration));
    }

    private IEnumerator SmoothTransitionRoutine(float targetVignette, float targetSaturation, float duration)
    {
        if (vignette == null || colorAdjustments == null) yield break;

        float startVignette = vignette.intensity.value;
        float startSaturation = colorAdjustments.saturation.value;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            vignette.intensity.value = Mathf.Lerp(startVignette, targetVignette, t);
            colorAdjustments.saturation.value = Mathf.Lerp(startSaturation, targetSaturation, t);

            yield return null;
        }

        // Final snap to exact target values
        vignette.intensity.value = targetVignette;
        colorAdjustments.saturation.value = targetSaturation;
        currentRoutine = null;
    }

    public void Reset()
    {
        vignette.intensity.value = 0.2f;
        colorAdjustments.saturation.value = 0f;
    }
}
