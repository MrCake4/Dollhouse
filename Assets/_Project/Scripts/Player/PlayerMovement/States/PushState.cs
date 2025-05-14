using UnityEngine;

public class PushState : BasePlayerState
{
    private Rigidbody targetRb;
    private float basePushSpeed = 1.5f; // kann im Editor einstellbar gemacht werden
    private RigidbodyConstraints originalConstraints; // merken, was vorher gesetzt war

    public void SetTarget(Rigidbody rb)
    {
        if (rb == null)
        {
            Debug.LogWarning("PushState: Versuch, null als Ziel zu setzen!");
            return;
        }
        
        targetRb = rb;
    }


    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("now pushing");
        if (targetRb != null)
        {
            // Merke ursprüngliche Constraints
            originalConstraints = targetRb.constraints;

            // Optional: nur Rotation einfrieren, Bewegung erlauben? --> man kann auch Rotation mit erlauben sonst
            targetRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        if (player.moveInput == Vector2.zero)                               //SWITCH Idle
        {
            player.SwitchState(player.idleState);     
            return;
        }

        // Prüfe Surface-Winkel erneut
        Vector3 rayOrigin = player.transform.position + Vector3.up * (player.capsuleCollider.height / 3f);
        if (Physics.Raycast(rayOrigin, player.transform.forward, out RaycastHit hit, 0.6f, player.bigObjectLayer))
        {
            float angle = Vector3.Angle(-hit.normal, player.transform.forward);
            if (angle > 25f)
            {
                player.SwitchState(player.idleState);
                return;
            }
        }
        else
        {
            // Kein Kontakt mehr
            player.SwitchState(player.idleState);
            return;
        }

        if(player.IsFalling()){                        //SWITCH Fall
            player.SwitchState(player.fallState);
            //Debug.Log("Switcherooo");
        }

    }
    public override void onFixedUpdate(PlayerStateManager player)          //Physik
    {
        if (targetRb == null) return; 

        Vector3 pushDir = player.transform.forward;
        float force = basePushSpeed / Mathf.Max(1f, targetRb.mass);


        targetRb.AddForce(pushDir * force, ForceMode.Force);

        // Optional: Langsameres Player-Movement
        player.MovePlayer(force); // Oder fixen Wert z. B. 0.5f
        player.RotateToMoveDirection();
    }
    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        if (targetRb != null)
        {
            // Setze ursprüngliche Constraints wieder zurück
            targetRb.constraints = originalConstraints;
        }
    }
}
