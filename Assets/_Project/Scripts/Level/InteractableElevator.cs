using UnityEngine;

public class InteractableElevator : Interactable
{
    public override void interact()
    {
    }

    public override void onPowerOff(){}

    public override void onPowerOn()
    {
        this.gameObject.SetActive(false);
    }
}
