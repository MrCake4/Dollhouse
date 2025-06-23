using UnityEngine;

public class PullUpState : BasePlayerState
{
    public enum PullUpType
    {
        Medium,
        High
    }

    private PullUpType pullUpType = PullUpType.Medium;
    private float pullSpeed = 3f;
    private Vector3 ledgePosition;
    private Vector3 finalStandPosition;

    public void SetLedgePosition(Vector3 ledgePos)
    {
        ledgePosition = ledgePos;
    }

    public void SetPullUpType(PullUpType type)
    {
        pullUpType = type;
    }

    public override void onEnter(PlayerStateManager player)
    {
        player.rb.useGravity = false;
        player.rb.linearVelocity = Vector3.zero;

        float verticalOffset = player.verticalPullUp;
        float backOffset = player.horizontalPullUp;

        // Offsets je nach Typ anpassen
        switch (pullUpType)
        {
            case PullUpType.Medium:
                verticalOffset = 0.8f;
                backOffset = -0.3f;
                break;
            case PullUpType.High:
                verticalOffset = 1.2f;
                backOffset = -0.4f;
                break;
        }

        finalStandPosition = ledgePosition + Vector3.up * verticalOffset - player.transform.forward * backOffset;

        Debug.Log($"PullUp → Typ: {pullUpType} | Zielposition: {finalStandPosition:F2}");
    }

    public override void onFixedUpdate(PlayerStateManager player)
    {
        Vector3 newPos = Vector3.MoveTowards(
            player.transform.position,
            finalStandPosition,
            pullSpeed * Time.fixedDeltaTime
        );

        player.rb.MovePosition(newPos);

        if (Vector3.Distance(player.transform.position, finalStandPosition) < 0.05f)
        {
            player.rb.useGravity = true;
            player.SwitchState(player.idleState);
        }
    }

    public override void onUpdate(PlayerStateManager player)
    {
        Debug.DrawLine(player.transform.position, finalStandPosition, Color.green);
    }

    public override void onExit(PlayerStateManager player)
    {
        player.rb.useGravity = true;
    }
}
