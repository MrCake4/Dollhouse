using UnityEngine;

public class FallState : BasePlayerState                                //dann, wenn Y-Velocity negativ ist!
{
    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("Falling");
    }
    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        if (player.IsGrounded())
        {
            float speed = player.GetHorizontalSpeed();

            if (speed >= player.walkSpeed)
            {
                player.SwitchState(player.isRunning ? player.runState : player.walkState);
            }
            else
            {
                player.SwitchState(player.idleState);
            }
        }
    }
    public override void onFixedUpdate(PlayerStateManager player)          //Physik
    {
        player.ApplyAirControl(player);
        player.RotateToMoveDirection();
    }
    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        
    }
    
}
