using UnityEngine;

public class InteractableDoor : Interactable
{
    [SerializeField]bool leftDoor = false; // true = left door, false = right door
    public override void interact()
    {
    }

    public override void onPowerOff(){}

    public override void onPowerOn()
    {
        Debug.Log("Door interacted with!");
        if (leftDoor)
        {
            transform.localRotation = Quaternion.Euler(0, 90, 0); // Rotate left door
            // Add logic for left door interaction
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, -90, 0); // Rotate right door
            // Add logic for right door interaction
        }
    }
}
