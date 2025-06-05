using UnityEngine;

// Requieres PlayerItemHandler.cs to be attached to the player
// Requieres a Light component as child of the candle object

public class Candle : Interactable
{
    PlayerItemHandler playerItemHandler;
    bool isLit = false;
    Light candleLight;
    float minIntensity;
    float maxIntensity;

    float currentTargetIntensity;
    float lerpSpeed = 2f; // How fast the light flickers
    float nextTargetTime;
    float targetChangeInterval = 0.5f; // How often to change the intensity target

    void Start()
    {
        playerItemHandler = FindFirstObjectByType<PlayerItemHandler>();
        candleLight = GetComponentInChildren<Light>();

        if (candleLight != null)
        {
            candleLight.enabled = false;
            minIntensity = candleLight.intensity * 0.5f;
            maxIntensity = candleLight.intensity * 1.5f;
            currentTargetIntensity = candleLight.intensity;
        }
    }

    public override void interact()
    {
        // if the player is carrying a candle and it is not lit, light it
        if (playerItemHandler.GetCarriedObject() != null &&
            playerItemHandler.GetCarriedObject().CompareTag("Candle") &&
            !isLit)
        {
            isLit = true;
            candleLight.enabled = true;
            currentTargetIntensity = Random.Range(minIntensity, maxIntensity);
            nextTargetTime = Time.time + targetChangeInterval;
        }
    }

    void Update()
    {
        if (isLit && candleLight != null)
        {
            // Smoothly interpolate intensity
            candleLight.intensity = Mathf.Lerp(candleLight.intensity, currentTargetIntensity, Time.deltaTime * lerpSpeed);

            // Change target intensity periodically
            if (Time.time >= nextTargetTime)
            {
                currentTargetIntensity = Random.Range(minIntensity, maxIntensity);
                nextTargetTime = Time.time + targetChangeInterval;
            }
        }
    }
}