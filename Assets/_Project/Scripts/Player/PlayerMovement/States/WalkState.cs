using UnityEngine;

public class WalkState : BasePlayerState
{

    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("walking");
        //player.animator.SetBool("IsWalking", true);
    }
    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        // switch State
        //if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) { player.SwitchState(player.idleState);}          // + GroundCheck muss auch true sein! (um still stehen von "Fall" zu unterscheiden)

        if(player.IsFalling()){                        //Damit man wirklich von jedem State aus auch nach Falling wechseln könnte
            player.SwitchState(player.fallState);
            //Debug.Log("Switcherooo");
        }

        if (player.JumpAllowed())                               //SWITCH JUMP
        {
            player.jumpPressed = false;

            //IF PLAYER HAS NO LOW LEDGE OR MEDIUM LEDGE in his BoxCollider-Trigger --> Jump State  --> sonst in den PullUpState mit der passenden Ledge description
            player.SwitchState(player.jumpState);
            //return;
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

        //Keine Bewegung → Idle
        else if (player.moveInput == Vector2.zero)          //SWITCH Idle
        {
            player.SwitchState(player.idleState);
        }

        //Shift gedrückt → Wechsel in Run
        else if (player.isRunning)                          //SWITCH Run
        {
            player.SwitchState(player.runState);
        }
        
        //wenn crouch true
        else if (player.isCrouching)                        //SWITCH Crouch
        {
            player.SwitchState(player.crouchState);
        }


    }
    public override void onFixedUpdate(PlayerStateManager player)          //Physik
    {
        player.MovePlayer(player.walkSpeed);
        player.RotateToMoveDirection();
    }
    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        //player.animator.SetBool("IsWalking", false);
    }
}
