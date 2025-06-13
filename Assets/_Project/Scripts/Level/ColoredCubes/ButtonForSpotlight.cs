using UnityEngine;

public class ButtonForSpotlight : Interactable
{

    [SerializeField] Light spotlight;
    private PlayerItemHandler player;
    [SerializeField] bool isOn = false;
    //[SerializeField] float distanceToButton = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerItemHandler>();
        if(!isOn && spotlight != null)
        {
            spotlight.enabled = false; // Ensure the spotlight is off at the start
        }
    }

    public void LightSwitch()
    {
        switch (spotlight.enabled)
        {
            case true:
                spotlight.enabled = false;
                break;
            case false:
                spotlight.enabled = true;
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

    public override void onPowerOff()
    {
        isOn = false;
        spotlight.enabled = false; // Ensure the spotlight is turned off when power is off
        Debug.Log("Spotlight turned off due to power loss.");
    }
}