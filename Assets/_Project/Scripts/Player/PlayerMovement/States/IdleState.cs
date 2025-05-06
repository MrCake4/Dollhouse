using Unity.VisualScripting;
using UnityEngine;

public class IdleState : BasePlayerState
{
    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("not walking anymore");
    }
    public override void onUpdate(PlayerStateManager player)                        //pro Frame
    {

        if (player.JumpAllowed())       //JUMP
        {
            player.jumpPressed = false;
            player.SwitchState(player.jumpState);
            return;                                                                 //retrun, damit Code direkt hier aufhört und zu JumpState switched
        }

        if (player.moveInput != Vector2.zero && player.PushAllowed(out Rigidbody pushTarget))      //PUSH
        {
            player.pushState.SetTarget(pushTarget);
            player.SwitchState(player.pushState);
            return;
        }
        
        //wenn Crouching true
        else if (player.isCrouching)
        {
            player.SwitchState(player.crouchState);
        }
        // Wenn Shift + WASD → RunState
        else if (player.moveInput != Vector2.zero && player.isRunning)
        {
            player.SwitchState(player.runState);
        }
        // Wenn nur WASD → WalkState
        else if (player.moveInput != Vector2.zero)
        {
            player.SwitchState(player.walkState);
        }
    }
    public override void onFixedUpdate(PlayerStateManager player)                   //Physik
    {
        // Stoppe alle horizontalen Bewegungen
        Vector3 stoppedVelocity = player.rb.linearVelocity;
        stoppedVelocity.x = 0f;
        stoppedVelocity.z = 0f;

        player.rb.linearVelocity = stoppedVelocity;

    }
    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        
    }
}
