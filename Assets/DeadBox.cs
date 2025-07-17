using UnityEngine;

// The Dead Box is to be used as a boundary for the player
// If the player f.e. falls though a window the Dead Box will switch the player state to dead

public class DeadBox : MonoBehaviour
{
    PlayerStateManager player;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerStateManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            killPlayer();
        }
    }

    void killPlayer()
    {
        if (player != null) player.SwitchState(player.deadState);
    }
}
