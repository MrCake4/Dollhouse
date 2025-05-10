using UnityEngine;

public class JumpState : BasePlayerState
{
    
    public override void onEnter(PlayerStateManager player)
    {
        /*
        // Spieler kann aus Idle mit WASD schräg springen
        Vector3 direction = player.moveDir != Vector3.zero
            ? player.moveDir.normalized
            : Vector3.zero; // Falls keine Eingabe

        float horizontalSpeed = player.isRunning ? player.maxSpeed : player.walkSpeed;

        Vector3 horizontalVelocity = player.GetHorizontalVelocity();

        // Sprungkraft
        float baseY = Mathf.Sqrt(2f * Physics.gravity.magnitude * player.jumpHeight);   // 2f wegen physikalischer Formel v = sqrt(2∗g∗h)
        if (player.isRunning)
            baseY *= 1.1f; // Bonushöhe beim Rennen

        // Zusammensetzen
        Vector3 jumpImpulse = horizontalVelocity + Vector3.up * baseY;

        player.rb.AddForce(jumpImpulse, ForceMode.VelocityChange); 

        Debug.Log($"Jumping | Direction: {direction} | Speed: {horizontalSpeed:F2} | Vertical: {baseY:F2}"); */


        // Vertikale Sprungkraft berechnen
        float baseY = Mathf.Sqrt(2f * Physics.gravity.magnitude * player.jumpHeight);
        if (player.isRunning)  Debug.Log("run-JUMP");       
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
        if (player.IsGrounded())                                            // Wenn der Jump physisch nicht gezündet hat (z. B. wegen Blockade)
        {
            float speed = player.GetHorizontalSpeed();
            if (speed >= player.walkSpeed)
                player.SwitchState(player.isRunning ? player.runState : player.walkState);
            else
                player.SwitchState(player.idleState);                           // oder Run/Walk je nach Input, wenn gewünscht
        }
        else if (player.IsFalling())                                        // Wenn man wirklich abspringt --> falling
        {
            player.SwitchState(player.fallState);
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
    }

    
}
