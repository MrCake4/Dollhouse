using UnityEngine;

public class HangState : BasePlayerState
{
    private Vector3 hangPosition;
    private bool isHanging = false;

    [SerializeField] private float hangOffsetY = 1.0f; // Offset, wie tief der Spieler hängt (Kopf ≈ 1.0)

    public void SetHangPosition(Vector3 pos)
    {
        hangPosition = pos;
    }

    public override void onEnter(PlayerStateManager player)
    {
        player.rb.useGravity = false;
        player.rb.linearVelocity = Vector3.zero;

        Vector3 offset = Vector3.down * hangOffsetY;
        player.rb.MovePosition(hangPosition + offset);

        isHanging = true;
        Debug.Log("Hänge an Objekt.");
    }

    public override void onUpdate(PlayerStateManager player)
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Joystick1Button5))
        {
            Debug.Log("Losgelassen!");
            player.SwitchState(player.fallState);
        }
    }

    public override void onFixedUpdate(PlayerStateManager player)
    {
        if (isHanging)
        {
            player.rb.MovePosition(hangPosition + Vector3.down * hangOffsetY);
        }
    }

    public override void onExit(PlayerStateManager player)
    {
        player.rb.useGravity = true;
    }
}
