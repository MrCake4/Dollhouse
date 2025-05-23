using UnityEngine;

public class WalkState : BasePlayerState
{

    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("walking");
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
            player.SwitchState(player.jumpState);
            return;
        }
        
        if (player.holdPressed)                             //SWITCH PULLUP or HANG
        {
            player.TryGrab();
            return;
        }

        if (player.moveInput != Vector2.zero && player.PushAllowed(out Rigidbody pushTarget))      //SWITCH PUSH
        {
            player.pushState.SetTarget(pushTarget);
            player.SwitchState(player.pushState);
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
        
    }
}
