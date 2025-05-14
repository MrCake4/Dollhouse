using UnityEngine;

/*
*
*   Checks if the player has entered a checkpoint. Then activates the checkpoint.
*
*/

public class Checkpoint : MonoBehaviour
{
    bool isActive = false;
    [SerializeField] CheckpointHandler checkpointHandler;

    // when player enters the trigger, activate the checkpoint
    void OnTriggerEnter(Collider other)
    {
        if(checkpointHandler != null){
            if (other.CompareTag("Player"))
            {
                if (!isActive)
                {
                    isActive = true;
                    checkpointHandler.ActivateCheckpoint(this);
                }
            }
        }
    }

    public bool getIsActive => isActive;

    public void setIsActive(bool value)
    {
        isActive = value;
    }
}
