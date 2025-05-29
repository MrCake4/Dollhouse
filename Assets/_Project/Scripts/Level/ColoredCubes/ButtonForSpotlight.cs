using UnityEngine;

public class ButtonForSpotlight : Interactable
{

    [SerializeField] Light spotlight;
    private PlayerItemHandler player;
    //[SerializeField] float distanceToButton = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerItemHandler>();
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
        LightSwitch();
    }
}