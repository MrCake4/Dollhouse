using System;
using UnityEngine;

public class JumpState : BasePlayerState
{
    public float jumpHeight;
    public override void onEnter(PlayerStateManager player)
    {
        player.animator.SetTrigger("DoJump");

        // === STAMINA-Verbrauch beim Springen ===
        player.GetComponent<StaminaSystem>()?.ConsumeJumpCost();


        // Vertikale Sprungkraft berechnen
        float baseY = Mathf.Sqrt(2f * Physics.gravity.magnitude * jumpHeight);
        if (player.isRunning) Debug.Log("run-JUMP");
        //baseY *= 1.1f;

        // Aktuelle horizontale Geschwindigkeit beibehalten
        Vector3 currentHorizontal = player.GetHorizontalVelocity();

        // Begrenze horizontale Sprunggeschwindigkeit
        float maxJumpHorizontalSpeed = player.isRunning ? player.maxSpeed : player.walkSpeed;
        if (currentHorizontal.magnitude > maxJumpHorizontalSpeed)
        {
            currentHorizontal = currentHorizontal.normalized * maxJumpHorizontalSpeed;
        }

        // Kombinierter Impuls
        Vector3 jumpImpulse = currentHorizontal;
        jumpImpulse.y = baseY;

        player.rb.linearVelocity = jumpImpulse; // Direkt setzen statt AddForce → präziser

        Debug.Log($"Jumping | Horizontal: {currentHorizontal.magnitude:F2} | Vertical: {baseY:F2}");


    }
    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        if (player.TryAutoPullUp())
        return;


        if (player.GetVerticalVelocity() <= 0.1 && player.groundCheck.isGrounded)                                            // Wenn der Jump physisch nicht gezündet hat (z. B. wegen Blockade)
        {
            Debug.Log("somehow I think I am grounded - lol");
            float speed = player.GetHorizontalSpeed();
            if (speed >= player.walkSpeed)
            {
                player.SwitchState(player.isRunning ? player.runState : player.walkState);
                //return;
            }
            else
            {
                player.SwitchState(player.idleState);                           // oder Run/Walk je nach Input, wenn gewünscht
                //return;
            }

        }
        else if (player.IsFalling())                                        // Wenn man wirklich abspringt --> falling
        {
            player.SwitchState(player.fallState);
            //return;
        }

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
        //nix weiter nötig lol
        //player.animator.SetBool("IsJumping", false);
        player.animator.ResetTrigger("DoJump");

    }

    
}
