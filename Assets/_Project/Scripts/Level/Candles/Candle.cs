using NavKeypad;
using UnityEngine;

// Requieres PlayerItemHandler.cs to be attached to the player
// Requieres a Light component as child of the candle object

[RequireComponent(typeof(KeypadButton))]
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

    // Password bindings
    KeypadButton keypadButton;
    [SerializeField]bool isNormalCandle = true; // If false, the candle will not trigger the keypad button
    [SerializeField] GameObject numberQuad; // The light object that will be enabled when the candle is lit
    MeshRenderer meshRendererNumberQuad;

    [Header("Sound Effects")]
    [SerializeField] AudioClip candleLightEffect; // Sound effect when the candle is lit

    void Awake()
    {
        if (isNormalCandle && numberQuad != null)
        {
            // make mesh of numberQuad invisible at start, dont use setactive(false) because it will break the number quad
            meshRendererNumberQuad = numberQuad.GetComponent<MeshRenderer>();
            if (meshRendererNumberQuad != null)
            {
                meshRendererNumberQuad.enabled = false; // Disable the mesh renderer to make it invisible
            }
        }
    }

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

        keypadButton = GetComponent<KeypadButton>();
    }

    public override void interact()
    {
        // if the player is carrying a candle and it is not lit, light it
        if (playerItemHandler.GetCarriedObject() != null &&
            playerItemHandler.GetCarriedObject().CompareTag("Candle") &&
            !isLit)
        {
            lightCandle();
            if (meshRendererNumberQuad != null) meshRendererNumberQuad.enabled = true; // Enable the number quad when the candle is lit
            // Play sound effect if it is a normal candle
            if (isNormalCandle && candleLightEffect != null) SoundEffectsManager.instance.PlaySoundEffect(candleLightEffect, transform, 1f);

            // if candle is not a normal candle,
            if (!isNormalCandle)
            {
                keypadButton.PressButton();
                keypadButton.GetKeypad().litCandleCount++;
            }
        }
    }

    public void lightCandle()
    {
        isLit = true;
        candleLight.enabled = true;
        currentTargetIntensity = Random.Range(minIntensity, maxIntensity);
        nextTargetTime = Time.time + targetChangeInterval;
    }

    public void extinguishCandle()
    {
        isLit = false;
        candleLight.enabled = false;
        currentTargetIntensity = 0f; // Reset intensity when extinguished
    }

    void updateCandleLight()
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

    void Update()
    {
        // update the candle light if it is lit
        if (isLit && candleLight != null)
        {
            updateCandleLight();
        }
    }

    public override void onPowerOn()
    {
        throw new System.NotImplementedException();
    }

    public override void onPowerOff()
    {
        throw new System.NotImplementedException();
    }
}