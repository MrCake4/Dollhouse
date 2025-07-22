using System;
using UnityEngine;

public class ButtonForSpotlight : Interactable
{

    enum leverPosition
    {
        up,
        down
    }

    [Header("Lever Settings")]
    [SerializeField] Light spotlight;
    private PlayerItemHandler player;
    [SerializeField] bool isOn = false;
    [SerializeField] Transform leverTransform;
    leverPosition lpos = leverPosition.down; // stores current lever position

    [Header("Lever Sound")]
    [SerializeField] AudioClip[] leverSounds;

    [Header("Glowing Light Object")]
    [SerializeField] GameObject lightObject;
    [SerializeField] private Material offMaterial;
    [SerializeField] private Material onMaterial;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerItemHandler>();
        if (!isOn && spotlight != null)
        {
            spotlight.enabled = false; // Ensure the spotlight is off at the start
        }

        pullLever(leverPosition.down);
        switchLightMaterial(false);
    }

    // Method to switch the lever position and toggle the spotlight
    public void leverSwitch()
    {
        switch (lpos)
        {
            case leverPosition.up:
                lpos = leverPosition.down;
                pullLever(lpos);
                if (isOn)
                {
                    spotlight.enabled = false; // Ensure the spotlight is turned off
                    switchLightMaterial(false);
                }
                break;
            case leverPosition.down:
                lpos = leverPosition.up;
                pullLever(lpos);
                if (isOn)
                {
                    spotlight.enabled = true; // Ensure the spotlight is turned on
                    switchLightMaterial(true);
                }
                break;
        }
    }

    // Method to pull the lever and change its rotation
    private void pullLever(leverPosition position)
    {
        if (leverTransform == null) return;

        if (leverSounds.Length > 0) SoundEffectsManager.instance.PlayRandomSoundEffect(leverSounds, transform, 0.3f);

        switch (position)
        {
            case leverPosition.up:
                leverTransform.rotation = Quaternion.Euler(leverTransform.eulerAngles.x, leverTransform.eulerAngles.y, -90f);
                break;
            case leverPosition.down:
                leverTransform.rotation = Quaternion.Euler(leverTransform.eulerAngles.x, leverTransform.eulerAngles.y, 90f);
                break;
        }
    }

    // Method to switch the light material based on the lever position
    void switchLightMaterial(bool mode)
    {
        if (lightObject != null)
        {
            switch (mode)
            {
                case true:
                    lightObject.GetComponent<MeshRenderer>().material = onMaterial;
                    break;
                case false:
                    lightObject.GetComponent<MeshRenderer>().material = offMaterial;
                    break;
            }
        }
    }

    // Override Methods from Interactable class
    public override void interact()
    {
        leverSwitch();
    }

    public override void onPowerOn()
    {
        isOn = true;
        if(lpos == leverPosition.up)
        {
            spotlight.enabled = true; // Ensure the spotlight is turned on when power is on
            switchLightMaterial(true);
        }
    }

    public override void onPowerOff()
    {
        lpos = leverPosition.down; // Reset lever position when power is off
        isOn = false;
        spotlight.enabled = false; // Ensure the spotlight is turned off when power is off
        pullLever(leverPosition.down);
        switchLightMaterial(false);
        Debug.Log("Spotlight turned off due to power loss.");
    }
}