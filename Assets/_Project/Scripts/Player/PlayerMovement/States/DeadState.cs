using UnityEngine;

public class DeadState : BasePlayerState
{

    Rigidbody playerRigidbody;
    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("Player is dead!");

        playerRigidbody = player.GetComponent<Rigidbody>();
        
        playerRigidbody.freezeRotation = false;
    }

    public override void onUpdate(PlayerStateManager player)
    {

    }

    public override void onFixedUpdate(PlayerStateManager player)
    {

    }

    public override void onExit(PlayerStateManager player)
    {
        playerRigidbody.freezeRotation = true;
        playerRigidbody.rotation = Quaternion.identity; // Reset rotation to prevent weird physics behavior
    }
}
