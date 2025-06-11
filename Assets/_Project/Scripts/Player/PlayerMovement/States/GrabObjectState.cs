using UnityEngine;

public class GrabObjectState : BasePlayerState
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
        if (targetObject == null || grabPoint == null)
        {
            Debug.LogWarning("GrabObjectState → Kein gültiges Ziel");
            player.SwitchState(player.idleState);
            return;
        }

        Debug.Log("GrabObjectState → Spieler hält Objekt fest");

        // Fixe Position einnehmen
        Vector3 targetPos = grabPoint.position;
        targetPos.y = player.transform.position.y;
        player.transform.position = targetPos;

        Quaternion rot = Quaternion.LookRotation(grabPoint.forward);
        player.transform.rotation = rot;

        // Keine Bewegung erlaubt solange hier
        player.rb.linearVelocity = Vector3.zero;
        player.rb.angularVelocity = Vector3.zero;

        targetObject.SetPhysicsActive(false); // Nur aktiv in Push/Pull
    }

    public override void onUpdate(PlayerStateManager player)
    {
        // R1 losgelassen → zurück zu Idle
        if (!player.holdPressed)
        {
            Debug.Log("GrabObjectState → R1 losgelassen → zurück zu Idle");
            player.SwitchState(player.idleState);
            return;
        }

        // Position erzwingen
        Vector3 lockedPos = grabPoint.position;
        lockedPos.y = player.transform.position.y;
        player.transform.position = Vector3.Lerp(player.transform.position, lockedPos, Time.deltaTime * 15f);

        // Rotation fixieren
        Quaternion lookRot = Quaternion.LookRotation(grabPoint.forward);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, lookRot, Time.deltaTime * 15f);

        // Keine Bewegung oder Rotation erlauben
        player.rb.linearVelocity = Vector3.zero;
        player.rb.angularVelocity = Vector3.zero;

        // Bewegung prüfen → Push/Pull starten
        Vector3 inputDir = new Vector3(player.moveInput.x, 0f, player.moveInput.y);
        float dot = Vector3.Dot(player.transform.forward, inputDir);

        if (dot > 0.1f)
        {
            player.pushState.SetTarget(targetObject.GetRigidbody(), grabPoint);
            player.SwitchState(player.pushState);
        }
        else if (dot < -0.1f)
        {
            player.pullState.SetTarget(targetObject.GetRigidbody(), grabPoint);
            player.SwitchState(player.pullState);
        }

    }

    public override void onFixedUpdate(PlayerStateManager player)
    {
        // Sicherheitshalber alles auf 0 halten
        player.rb.linearVelocity = Vector3.zero;
        player.rb.angularVelocity = Vector3.zero;
    }

    public override void onExit(PlayerStateManager player)
    {
        Debug.Log("GrabObjectState → verlassen");
        targetObject = null;
        grabPoint = null;
    }
}
