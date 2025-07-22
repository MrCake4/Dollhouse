using Unity.VisualScripting;
using UnityEngine;

public class PullUpState : BasePlayerState
{
    private Vector3 ledgePos;
    private Vector3 finalStandPos;
    public bool pullUpFinished = false;


    public void SetLedgeFromTransform(Transform triggerTransform, Vector3 playerWorldPos)
    {
        // Berechne die Position des Spielers RELATIV zum Trigger (lokales Koordinatensystem)
        Vector3 localPlayerPos = triggerTransform.InverseTransformPoint(playerWorldPos);

        // Wir setzen eine neue Position direkt auf der Ledge:
        // - Die Höhe (Y) nehmen wir vom Trigger
        // - Die Tiefe (Z) = 0 (genau auf der Kante)
        // - Die seitliche Position (X) bleibt erhalten
        Vector3 localTargetPos = new Vector3(localPlayerPos.x, 0f, 0f);

        // Umwandeln zurück in Weltkoordinaten
        ledgePos = triggerTransform.TransformPoint(localTargetPos);
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

        // Rigidbody und RootMotion deaktivieren VOR dem Umpositionieren
        player.rb.isKinematic = true;
        player.animator.applyRootMotion = true;

        // Setze sofort die Zielposition
            //player.transform.position = finalStandPos;

        // Animation triggern
        player.animator.SetTrigger("DoPullUp");

        // Hier kommt der Trick:
        // Forciere ein sofortiges Update der Animator-Pose,
        // damit die neue Pose im selben Frame aktiv ist.
            //player.animator.Update(0f);
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

    public void OnPullUpStart(PlayerStateManager player)
    {
        // Spieler exakt bei Animationsbeginn auf die Kante setzen
        player.transform.position = finalStandPos;

        // Falls du RootMotion nutzt und willst, dass er sofort korrekt startet:
        player.animator.Update(0f);

        //Debug.Log("PullUp Synchronization worked!");
    }



}
