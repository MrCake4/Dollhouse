using UnityEngine;

public class WalkState : BasePlayerState
{
    private Vector3 moveDir;

    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("walking");
    }
    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        // switch State
        //if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) { player.SwitchState(player.idleState);}          // + GroundCheck muss auch true sein! (um still stehen von "Fall" zu unterscheiden)
        
        //Keine Bewegung → Idle
        if (player.moveInput == Vector2.zero)
        {
            player.SwitchState(player.idleState);
        }

        //Shift gedrückt → Wechsel in Run
        else if (player.isRunning)
        {
            player.SwitchState(player.runState);
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
