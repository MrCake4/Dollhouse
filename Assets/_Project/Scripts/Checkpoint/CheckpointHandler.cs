using UnityEngine;
using System.Collections;

public class CheckpointHandler : MonoBehaviour
{

    // stores all checkpoints in the scene
    Checkpoint[] checkpoints;

    PlayerStateManager player;

    AIRoomScan eye;
    bool respawning = false;
    void Start()
    {
        // Find all checkpoints in the scene
        checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);

        player = FindFirstObjectByType<PlayerStateManager>();

        eye = FindFirstObjectByType<AIRoomScan>();
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !respawning)
        {
            respawning = true;
            StartCoroutine(RespawnPlayer(GameObject.FindWithTag("Player").transform));
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

    // "Respawns" aka. "teleports" the player to the last active checkpoint
    public IEnumerator RespawnPlayer(Transform playerObject)
    {
        SceneFadeManager.instance.StartFadeOut();
        yield return new WaitUntil(() => !SceneFadeManager.instance.isFadingOut);

        if (checkpoints.Length <= 0)
        {
            player.PlayerItemHandler.DropItem();
            playerObject.position = FindAnyObjectByType<SceneEntry>().transform.position;
        }
        else
        {
            // Respawn at last active checkpoint
            for (int i = checkpoints.Length - 1; i >= 0; i--)
            {
                if (checkpoints[i].getIsActive)
                {
                    player.PlayerItemHandler.DropItem();
                    playerObject.position = checkpoints[i].transform.position;
                    break;
                }
            }
        }

        AIStateManager ai = FindFirstObjectByType<AIStateManager>();
        if (ai != null) ResetAI(ai);
        player.SwitchState(player.idleState);
        SceneFadeManager.instance.StartFadeIn();
        yield return new WaitUntil(() => !SceneFadeManager.instance.isFadingIn);
        respawning = false;
    }


    // resets the AI to the idle state
    public void ResetAI(AIStateManager ai)
    {
        ai.switchState(ai.idleState);
        eye.SetHitPlayer(false);
    }
}
