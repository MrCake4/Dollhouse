using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

/*
*   ====
*   This script is used to increase or decrease the on screen brightness.
*   It should be added to a game object that has a Volume component which holds
*   a ColorAdjustments setting.
*/

public class ToggleBrightness : MonoBehaviour
{
    // The Volume is declared in the object inspector
    Volume brightnessVolume;
    ColorAdjustments colorAdjustments; // stores the colorAdjustment settings of the Volume

    void Start()
    {
        brightnessVolume = GetComponent<Volume>();

        if (brightnessVolume != null && brightnessVolume.profile.TryGet(out colorAdjustments))
        {
            // Found ColorAdjustments
            colorAdjustments.postExposure.value = 0f;
        }
        else
        {
            Debug.LogError("Volume or ColorAdjustments not found");
        }
    }

    void Update()
    {
        checkInput();
    }

    // If user presses "O" or "L" increase or decrease the on screen brightness
    void checkInput()
    {
        // Plus key: Shift + Equals (main keyboard) or KeypadPlus (numpad)
        if (Input.GetKey(KeyCode.I)|| Input.GetKey(KeyCode.KeypadPlus))
        {
            if (colorAdjustments != null && colorAdjustments.postExposure.value <= 3)
                colorAdjustments.postExposure.value += 0.1f;
        }
        // Minus key: Minus (main keyboard) or KeypadMinus (numpad)
        if (Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.KeypadMinus))
        {
            if (colorAdjustments != null && colorAdjustments.postExposure.value >= -1)
                colorAdjustments.postExposure.value -= 0.1f;
        }
    }
}