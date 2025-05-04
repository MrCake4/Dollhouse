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
            // Nach Landung zurück zu Idle, Walk oder Run je nach Input
            if (player.moveInput != Vector2.zero && player.isRunning)
            {
                player.SwitchState(player.runState);
            }
            else if (player.moveInput != Vector2.zero)
            {
                player.SwitchState(player.walkState);
            }
            else
            {
                player.SwitchState(player.idleState);
            }
        }
    }
    public override void onFixedUpdate(PlayerStateManager player)          //Physik
    {
        // Luftsteuerung (nur leichte Richtungsänderung)
        Vector3 airMove = player.moveDir * player.maxSpeed * player.airControlMultiplier;

        // Additiv statt überschreiben → verhindert Abbremsen
        player.rb.AddForce(new Vector3(airMove.x, 0f, airMove.z), ForceMode.Acceleration);

        player.RotateToMoveDirection();
    }
    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        
    }
}
