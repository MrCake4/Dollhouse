using Unity.VisualScripting;
using UnityEngine;

public class PullUpState : BasePlayerState
{
    private Vector3 ledgePos;
    private Vector3 finalStandPos;

    public void SetLedgePos(Vector3 pos)
    {
        ledgePos = pos;
    }

    private void CalculateLedgePos(PlayerStateManager player)
    {
        ledgePos = new Vector3(ledgePos.x, ledgePos.y, player.transform.position.z);
    }

    public override void onEnter(PlayerStateManager player)
    {
        CalculateLedgePos(player);

        // Zielposition auf der oberen Kante, leicht vorgezogen
        finalStandPos = new Vector3(
            ledgePos.x - player.ledgeOffset, 
            ledgePos.y,
            ledgePos.z
        );

        // Spieler sofort an die untere Griffposition bringen
        player.transform.position = new Vector3(
            ledgePos.x,
            ledgePos.y - 1.0f, // etwa 1m unterhalb
            ledgePos.z
        );

        player.animator.applyRootMotion = true;
        player.rb.isKinematic = true;

        player.animator.SetTrigger("DoPullUp");
    }

    public override void onExit(PlayerStateManager player)
    {
        player.animator.ResetTrigger("DoPullUp");
        
        // am Ende der Animation genau auf die finale Position setzen
        player.transform.position = finalStandPos;

        player.animator.applyRootMotion = false;
        player.rb.isKinematic = false;
    }

    public override void onUpdate(PlayerStateManager player)
    {

    }

    public override void onFixedUpdate(PlayerStateManager player)
    {

    }
}
