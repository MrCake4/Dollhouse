using UnityEngine;

/*
*   ====
*   This script is used to increase or decrease the on screen brightness.
*   It should be added to a game object that has a Volume component which holds
*   a colorAdjustment setting.
*/

public class ToggleBrightness : MonoBehaviour {

    // The Volume is declared in the object inspector
    Volume brightnessVolume
    ColorAdjustments colorAdjustments;      // stores the colorAdjustment settings of the Volume

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brightnessVolume = GetComponent<Volume>();

        if(brightnessVolume != null && && brightnessVolume.profile.TryGet(out colorAdjustments))
        else Debug.LogError("Volume or ColorAdjustments not found");
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    // If user presses "+" or "-" increase or decrease the on screen brightness
    void checkInput(){
        if (Input.GetKey(KeyCode.Plus)){
            colorAdjustments.postExposure.value += 0.1f;
        }
        if (Input.GetKey(KeyCode.Minus)){
            colorAdjustments.postExposure.value -= 0.1f;
        }
    }
}