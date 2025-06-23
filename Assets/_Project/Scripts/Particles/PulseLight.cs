using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulseController : MonoBehaviour
{
    public List<Light> lights; // Or Light2D
    public float pulseDuration = 1f;
    public float delayBetweenLights = 0.2f;
    public float cooldownBetweenRotations = 2f;
    public float minIntensity = 0f;
    public float maxIntensity = 1f;

    void Start()
    {
        StartCoroutine(PulseLights());
    }

    IEnumerator PulseLights()
    {
        while (true)
        {
            foreach (var light in lights)
            {
                StartCoroutine(PulseLight(light));
                yield return new WaitForSeconds(delayBetweenLights);
            }
            yield return new WaitForSeconds(cooldownBetweenRotations);
        }
    }

    IEnumerator PulseLight(Light light)
    {
        float t = 0;
        while (t < pulseDuration)
        {
            t += Time.deltaTime;
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.Sin((t / pulseDuration) * Mathf.PI));
            light.intensity = intensity;
            yield return null;
        }
        light.intensity = minIntensity;
    }
}
