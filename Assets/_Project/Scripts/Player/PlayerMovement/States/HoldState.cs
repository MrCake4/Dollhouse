using UnityEngine;

public class HoldState : BasePlayerState
{

    private PushableObject targetObject;
    private Transform grabPoint;

    public void SetTarget(PushableObject obj, Transform point)
    {
        targetObject = obj;
        grabPoint = point;
    }

    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("currently Holding");
        
        if (targetObject == null || grabPoint == null)
        {
            Debug.LogWarning("GrabObjectState → Kein gültiges Grab-Ziel.");
            player.SwitchState(player.idleState);
            return;
        }

        // Physik aktivieren
        targetObject.SetPhysicsActive(true);

        // Spieler an Ziel bewegen
        Vector3 targetPos = grabPoint.position;
        targetPos.y = player.transform.position.y;

        player.transform.position = Vector3.Lerp(player.transform.position, targetPos, 1f);

        // Spieler schaut in Richtung des Objekts (von Trigger aus gesehen)
        player.transform.rotation = Quaternion.LookRotation(-grabPoint.forward);
    }

    public override void onUpdate(PlayerStateManager player)
    {
        // noch keine Logik für push/pull
        if (!player.holdPressed)
        {
            player.SwitchState(player.idleState);
        }
    }

    public override void onFixedUpdate(PlayerStateManager player) { }

    public override void onExit(PlayerStateManager player)
    {
        targetObject = null;
        grabPoint = null;
    }
}
