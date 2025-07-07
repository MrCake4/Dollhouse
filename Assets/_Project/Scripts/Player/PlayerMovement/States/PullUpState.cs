using Unity.VisualScripting;
using UnityEngine;

public class PullUpState : BasePlayerState
{
    private Vector3 ledgePos;
    private Vector3 finalStandPos;
    public bool pullUpFinished = false;


    public void SetLedgePos(Vector3 pos)
    {
        ledgePos = pos;
    }

    private void CalculateLedgePos(PlayerStateManager player)
    {
        ledgePos = new Vector3(ledgePos.x, ledgePos.y, ledgePos.z);
    }

    public override void onEnter(PlayerStateManager player)
    {
        CalculateLedgePos(player);


        Debug.Log("I AM IN PULLUP STATE");

        // Zielposition auf der oberen Kante, leicht vorgezogen
        finalStandPos = ledgePos + player.transform.forward.normalized * -0.08f;

        // Spieler sofort an die untere Griffposition bringen
        player.transform.position = finalStandPos;


        player.animator.applyRootMotion = true;
        player.rb.isKinematic = true;

        player.animator.SetTrigger("DoPullUp");
    }



    public override void onUpdate(PlayerStateManager player)
    {
        if (pullUpFinished)
        {
            onExit(player);
            player.SwitchState(player.idleState);
        }

    }
    public override void onExit(PlayerStateManager player)
    {
        player.animator.ResetTrigger("DoPullUp");

        //sauber auf finalStandPos setzen
        Vector3 forwardOffset = player.transform.forward.normalized * 0.3f; // weiter nach vorne
        player.transform.position = finalStandPos + forwardOffset;


        player.animator.applyRootMotion = false;
        player.rb.isKinematic = false;
        pullUpFinished = false;
    }

    public override void onFixedUpdate(PlayerStateManager player)
    {

    }


}
