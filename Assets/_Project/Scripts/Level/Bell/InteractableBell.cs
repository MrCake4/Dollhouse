using UnityEngine;

public class InteractableBell : Interactable
{
    [SerializeField] AudioClip bellSound;

    public override void interact()
    {
        Debug.Log("Bell interacted with!");
        if (SoundEffectsManager.instance != null)
        {
            SoundEffectsManager.instance.PlaySoundEffect(bellSound, transform, 1f);
        }
    }

    public override void onPowerOff()
    {
        throw new System.NotImplementedException();
    }

    public override void onPowerOn()
    {
        throw new System.NotImplementedException();
    }
}
