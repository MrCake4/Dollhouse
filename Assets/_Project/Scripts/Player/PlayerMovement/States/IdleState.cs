using Unity.VisualScripting;
using UnityEngine;

public class IdleState : BasePlayerState
{
    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("not walking anymore");
        player.ResetAllAnimationBools();
        player.animator.SetBool("IsIdle", true);

    }
    public override void onUpdate(PlayerStateManager player) //pro Frame
    {

        if (player.JumpAllowed())                                           //SWITCH JUMP
        {
            player.jumpPressed = false;
            player.SwitchState(player.jumpState);
            return; //retrun, damit Code direkt aufhört und zu JumpState switched
        }

        /*if (player.moveInput != Vector2.zero && player.PushAllowed(out Rigidbody pushTarget))      //SWITCH PUSH
        {
            player.pushState.SetTarget(pushTarget);
            player.SwitchState(player.pushState);
            return;
        }*/

        if (player.holdPressed)                                             //SWITCH PUSH/PULL
        {
            player.TryGrabObject(); // greift auf nahes pushableObject zu
            
            return;
        }

        //wenn Crouching true
        else if (player.isCrouching)                                        //SWITCH Crouch
        {
            player.SwitchState(player.crouchState);
        }
        // Wenn Shift + WASD → RunState
        else if (player.moveInput != Vector2.zero && player.isRunning)      //SWITCH Run
        {
            player.SwitchState(player.runState);
        }
        // Wenn nur WASD → WalkState
        else if (player.moveInput != Vector2.zero)                          //SWITCH Walk
        {
            player.SwitchState(player.walkState);
        }

        if(player.IsFalling()){                                             //SWITCH Fall
            player.SwitchState(player.fallState);
            //Debug.Log("Switcherooo");
        }



        //SWITCH Pull Up

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
        player.animator.SetBool("IsIdle", false);
    }
}
