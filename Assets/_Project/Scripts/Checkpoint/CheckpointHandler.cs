using UnityEngine;
using System.Collections;

public class CheckpointHandler : MonoBehaviour
{

    // stores all checkpoints in the scene
    Checkpoint[] checkpoints;

    PlayerStateManager player;

    AIRoomScan eye;

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
        if (Input.GetKeyDown(KeyCode.R))
        {

            StartCoroutine(RespawnPlayer(GameObject.FindWithTag("Player").transform));

            // reset AI state to idle
            AIStateManager ai = FindFirstObjectByType<AIStateManager>();
            if (ai != null) ResetAI(ai);
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
        // Respawn the player at the last active checkpoint
        for (int i = checkpoints.Length - 1; i >= 0; i--)
        {
            if (checkpoints[i].getIsActive)
            {
                // reset player position and state
                playerObject.position = checkpoints[i].transform.position;
                SceneFadeManager.instance.StartFadeIn();
                this.player.SwitchState(player.idleState);
            }
        }
    }

    // resets the AI to the idle state
    public void ResetAI(AIStateManager ai)
    {
        ai.switchState(ai.idleState, false);
        eye.setHitPlayer(false);
    }
}
