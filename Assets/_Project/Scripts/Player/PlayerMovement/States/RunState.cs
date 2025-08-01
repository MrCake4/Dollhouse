using UnityEngine;

public class RunState : BasePlayerState
{
    private float currentSpeed;
    // Wie schnell man sich an das Ziel annähert (0 = kein Effekt, 1 = sofort)
    private float speedLerpFactor = 2f;                                     // kann man später im Editor einstellbar machen

    public override void onEnter(PlayerStateManager player)
    {
        Vector3 horizontalVelocity = player.rb.linearVelocity;              // aktuelle Geschwindigkeit auf Basis der realen Rigidbody-Bewegung
        horizontalVelocity.y = 0f;                                          // y ignorieren (nur Bodenbewegung)
        currentSpeed = horizontalVelocity.magnitude;

        Debug.Log("Running - Startspeed: " + currentSpeed.ToString("F2"));
        //player.animator.SetBool("IsMoving", true);
        //player.animator.SetBool("IsRunning", true);
        player.animator.SetTrigger("ReturnToMoving");
    }

    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        if (player.JumpAllowed())
        {
            if (player.CanPullUp())
            {
                // handled intern den State-Switch
                return;
            }
            else
            {
                player.jumpPressed = false;
                player.jumpState.jumpHeight = player.RunJumpHeight;
                player.SwitchState(player.jumpState);
                return;
            }
        }

        if (player.IsFalling())
        {                             //SWICTH Fall
            player.SwitchState(player.fallState);
            //Debug.Log("Switcherooo");
            return;
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

        // Wenn keine Eingabe → Idle
        else if (player.moveInput == Vector2.zero)
        {
            player.SwitchState(player.idleState);               //SWITCH Idle
        }
        // Wenn Shift losgelassen → Walk
        else if (!player.isRunning)
        {
            player.SwitchState(player.walkState);               //SWICTH Walk
        }
    }

    public override void onFixedUpdate(PlayerStateManager player)          //Physik
    {
        // Lerp zur Zielgeschwindigkeit (smoothes Anlaufen)
        currentSpeed = Mathf.Lerp(currentSpeed, player.maxSpeed, speedLerpFactor * Time.fixedDeltaTime);

        // Bewegung & Rotation
        player.MovePlayer(currentSpeed);
        player.RotateToMoveDirection();
    }

    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        Debug.Log("Exit Run");
        //player.animator.SetBool("IsRunning", false);
        //player.animator.SetBool("IsMoving", false);
        player.animator.ResetTrigger("ReturnToMoving");
    }
}
