using Unity.VisualScripting;
using UnityEngine;

public class PullUpState : BasePlayerState
{
    private Vector3 ledgePos;
    private Vector3 finalStandPos;
    public bool pullUpFinished = false;


    //extra Kollider an Ledge erzeugen mit Player-Layer, wenn clib läuft --> kann dann angeschossen werden!!!!!!


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

        Debug.Log("I AM IN PULLUP STATE");
        

        // Zielposition (oberhalb der Ledge, etwas nach hinten versetzt)
        finalStandPos = new Vector3(
            ledgePos.x, // - player.ledgeOffset,
            ledgePos.y,
            ledgePos.z
        );

        // 👉 Spieler-Collider SOFORT auf Zielposition setzen
        player.transform.position = finalStandPos;

        // Optional: Wenn der Spieler zu hoch in der Luft hängt (z. B. visuell),
        // dann animiere eine kleine Fallbewegung oder spiele eine kurze PullUp-Animation,
        // aber physikalisch ist der Spieler schon korrekt.

        player.animator.applyRootMotion = false;
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

        // Finalstandposition leicht nach vorne schieben (z. B. 0.2f)
        Vector3 forwardOffset = new Vector3(0.2f, 0f, 0f); // nach rechts
        Vector3 correctedPos = finalStandPos + forwardOffset;

        player.transform.position = correctedPos;

        player.animator.applyRootMotion = false;
        player.rb.isKinematic = false;
        pullUpFinished = false;
    }


    public override void onFixedUpdate(PlayerStateManager player)
    {

    }


}
