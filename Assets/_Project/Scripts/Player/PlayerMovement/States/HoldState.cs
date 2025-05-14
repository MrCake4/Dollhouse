using UnityEngine;

public class HoldState : BasePlayerState
{
    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("Entered HoldState");

        BoxCollider box = player.GetComponent<BoxCollider>();
        if (box == null)
        {
            Debug.LogWarning("Kein BoxCollider gefunden!");
            player.SwitchState(player.idleState);
            return;
        }

        box.enabled = true;

        Collider[] hits = Physics.OverlapBox(
            box.bounds.center,
            box.bounds.extents,
            player.transform.rotation,
            ~0,
            QueryTriggerInteraction.Collide
        );

        box.enabled = false;

        foreach (Collider col in hits)
        {
            if (col.CompareTag("Ledge"))
            {
                Debug.Log("Ledge gefunden → PullUp!");
                Vector3 closestPoint = col.ClosestPoint(player.transform.position);
                player.pullUpState.SetLedgePosition(closestPoint);
                player.SwitchState(player.pullUpState);
                return;
            }

            if (col.CompareTag("HangOnto"))
            {
                Debug.Log("Hangable gefunden → Hänge mich dran!");
                Vector3 closestPoint = col.ClosestPoint(player.transform.position);
                player.hangState.SetHangPosition(closestPoint);
                player.SwitchState(player.hangState);
                return;
            }
        }

        Debug.Log("Keine Ledge gefunden --> zurück zu Idle");
        player.SwitchState(player.idleState);
    }

    public override void onUpdate(PlayerStateManager player) { }
    public override void onFixedUpdate(PlayerStateManager player) { }
    public override void onExit(PlayerStateManager player) { }
}
