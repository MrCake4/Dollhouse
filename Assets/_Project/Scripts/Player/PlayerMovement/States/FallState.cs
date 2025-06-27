using UnityEngine;

public class FallState : BasePlayerState                                //dann, wenn Y-Velocity negativ ist!
{
    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("Falling");
        player.animator.SetBool("IsFalling", true);
        player.animator.SetBool("ReachedJumpPeak", true);
    }
    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        if (player.HasLanded())
        {
            float speed = player.GetHorizontalSpeed();

            if (speed >= player.walkSpeed)
            {
                player.SwitchState(player.isRunning ? player.runState : player.walkState);      //SWITCH Run / Walk
                return;
            }
            else
            {
                Debug.Log(player.GetVerticalVelocity());
                player.SwitchState(player.idleState);                                           //SWITCH Idle
                return;
            }
        }

        /*if (player.holdPressed)                                             //SWITCH PULLUP or HANG
        {
            player.TryGrab();
            return;
        }*/
        if (player.holdPressed)
        {
            player.TryGrab();
            return;
        }
        
    }
    public override void onFixedUpdate(PlayerStateManager player)          //Physik
    {
        player.ApplyAirControl(player);
        player.RotateToMoveDirection();
    }
    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        Vector3 vel = player.rb.linearVelocity;

        //Restgeschwindigkeit limitieren
        float maxLandSpeed = player.maxSpeed * 1.1f;

        Vector3 horizontal = new Vector3(vel.x, 0f, vel.z);
        if (horizontal.magnitude > maxLandSpeed)
        {
            horizontal = horizontal.normalized * maxLandSpeed;
            vel.x = horizontal.x;
            vel.z = horizontal.z;
            player.rb.linearVelocity = vel;
        }

        player.animator.SetBool("IsFalling", false);
        player.animator.SetBool("ReachedJumpPeak", false);
    }
    
}
