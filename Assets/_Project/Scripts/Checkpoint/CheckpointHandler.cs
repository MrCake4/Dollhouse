using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{

    // stores all checkpoints in the scene
    public Checkpoint[] checkpoints;

    void Start()
    {
        // Find all checkpoints in the scene
        checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // checks every checkpoint and deactives the false ones
    public void ActivateCheckpoint(Checkpoint checkpoint)
    {
        // Activate the checkpoint
        for(int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i] == checkpoint)
            {
                continue;
            }
            else
            {
                checkpoints[i].setIsActive(false);
            }
        }
    }
}
