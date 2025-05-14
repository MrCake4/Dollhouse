using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{

    // stores all checkpoints in the scene
    Checkpoint[] checkpoints;
    
    GameObject[] respawnableObjects;
    Vector3[] objectSpawnPoints;

    void Start()
    {
        // Find all checkpoints in the scene
        checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);

        respawnableObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        objectSpawnPoints = new Vector3[respawnableObjects.Length];	
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

        // set current positions of objects as the spawn points
        for(int i = 0; i < respawnableObjects.Length; i++)
        {
            Vector3 spawnPoint = respawnableObjects[i].transform.position;
            objectSpawnPoints[i] = spawnPoint;
        }
    }

    // "Respawns" aka. "teleports" the player to the last active checkpoint
    public void RespawnPlayer(Transform player)
    {
        // Respawn the player at the last active checkpoint
        for (int i = checkpoints.Length - 1; i >= 0; i--)
        {
            if (checkpoints[i].getIsActive)
            {
                RespawnObjects();
                player.position = checkpoints[i].transform.position;
                // TODO: reset Player State to idle instead of dying
                break;
            }
        }
    }

    public void RespawnObjects(){
        // Respawn all objects at their last active spawn points
        for (int i = 0; i < respawnableObjects.Length; i++)
        {
            respawnableObjects[i].transform.position = objectSpawnPoints[i];
        }
    }

    // resets the AI to the idle state
    public void ResetAI(AIStateManager ai){
        ai.switchState(ai.idleState, false);
    }
}
