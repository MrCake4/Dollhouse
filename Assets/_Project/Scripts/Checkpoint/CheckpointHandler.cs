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
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer(GameObject.FindGameObjectWithTag("Player").transform);
        }
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

    public void RespawnPlayer(Transform player)
    {
        // Respawn the player at the last active checkpoint
        for (int i = checkpoints.Length - 1; i >= 0; i--)
        {
            if (checkpoints[i].getIsActive)
            {
                player.position = checkpoints[i].transform.position;
                break;
            }
        }
    }
}
