using UnityEngine;

public class JumpState : BasePlayerState
{
    
    public override void onEnter(PlayerStateManager player)
    {
        Vector3 forwardVelocity = new Vector3(player.rb.linearVelocity.x, 0f, player.rb.linearVelocity.z);      // Horizontale Geschwindigkeit auslesen (ohne y)


        player.rb.linearDamping = 1f;       //!!!!!!!!!!player.rb.drag = 1f; // höherer Luftwiderstand nur in Jump
        

        // Cap horizontal speed
        if (forwardVelocity.magnitude > player.maxSpeed)
            forwardVelocity = forwardVelocity.normalized * player.maxSpeed;

        Vector3 jumpVector = forwardVelocity + Vector3.up * player.jumpForce;           // Sprung-Vektor erstellen

        player.rb.linearVelocity = jumpVector;                                          //Anwenden

        Debug.Log("Jumping");
        Debug.Log("Initial Speed: " + forwardVelocity.magnitude);
    }
    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        // Falls der Sprung physikalisch nicht ausgeführt wurde → zurücksetzen
        if (player.rb.linearVelocity.y <= 0f)
        {
            player.SwitchState(player.fallState);
            return;
        }
    // Standard: Wenn Spieler fällt → FallState
        if (player.IsFalling())
        {
            player.SwitchState(player.fallState);
        }
    }
    public override void onFixedUpdate(PlayerStateManager player)          //Physik
    {
        // Bewegung in Blickrichtung (während des Sprungs)
        Vector3 airMove = player.moveDir * player.maxSpeed * player.airControlMultiplier;

        // Statt direkt überschreiben → addiere nur die gewünschte Änderung
        player.rb.AddForce(new Vector3(airMove.x, 0f, airMove.z), ForceMode.Acceleration);

        player.RotateToMoveDirection();
    }
    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        //nix weiter nötig lol
        player.rb.linearDamping = 0f; // !!!!!!!!!!!!!!!zurücksetzen
    }
}
