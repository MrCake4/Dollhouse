using UnityEngine;              //Vorlage für Interactables like buttons etc

public abstract class Interactable : MonoBehaviour
{
    public abstract void interact();

    public abstract void onPowerOn();
    public abstract void onPowerOff();
}
