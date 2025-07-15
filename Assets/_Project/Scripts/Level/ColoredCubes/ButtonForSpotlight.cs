using System;
using UnityEngine;

public class ButtonForSpotlight : Interactable
{

    enum leverPosition
    {
        up,
        down
    }

    [SerializeField] AudioClip[] leverSounds;

    [SerializeField] Light spotlight;
    private PlayerItemHandler player;
    [SerializeField] bool isOn = false;

    [Header("Glowing Light Object")]
    [SerializeField] GameObject lightObject;
    [SerializeField] private Material offMaterial;
    [SerializeField] private Material onMaterial;

    [SerializeField] Transform leverTransform;
    //[SerializeField] float distanceToButton = 3f;

    void Awake()
    {
    }

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

    public void LightSwitch()
    {
        switch (spotlight.enabled)
        {
            case true:
                spotlight.enabled = false;
                pullLever(leverPosition.down);
                switchLightMaterial(false);
                break;
            case false:
                spotlight.enabled = true;
                pullLever(leverPosition.up);
                switchLightMaterial(true);
                break;
        }
        
    }

    private void pullLever(leverPosition position)
    {
        if (leverTransform == null) return;

        if (leverSounds.Length > 0) SoundEffectsManager.instance.PlayRandomSoundEffect(leverSounds, transform, 0.5f);

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
    public override void interact()
    {
        if (isOn) LightSwitch();
    }

    public override void onPowerOn()
    {
        isOn = true;
    }

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

    public override void onPowerOff()
    {
        isOn = false;
        spotlight.enabled = false; // Ensure the spotlight is turned off when power is off
        pullLever(leverPosition.down);
        switchLightMaterial(false);
        Debug.Log("Spotlight turned off due to power loss.");
    }
}