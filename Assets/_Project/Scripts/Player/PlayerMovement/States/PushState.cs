using UnityEngine;

public class PushState : BasePlayerState
{
    private Rigidbody targetRb;
    private Transform grabPoint;
    private float pushSpeed = 1.5f; // Einfluss durch Masse später skalierbar

    public void SetTarget(Rigidbody rb, Transform point)
    {
        targetRb = rb;
        grabPoint = point;
    }

    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("→ PUSH START");
        //player.animator.SetBool("IsPushing", true);
        //player.animator.SetBool("IsGrabbing", true);
        player.animator.SetFloat("PushPullSpeed", 0f, 0.1f, Time.deltaTime);
        player.animator.SetTrigger("DoPushPullGrab");


        if (targetRb != null)
        {
            targetRb.isKinematic = false;
            targetRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public override void onFixedUpdate(PlayerStateManager player)
    {
        if (targetRb == null || grabPoint == null) return;

        // Spieler bleibt exakt am GrabPoint
        Vector3 targetPos = grabPoint.position;
        targetPos.y = player.transform.position.y;
        player.transform.position = targetPos;

        Quaternion lookRot = Quaternion.LookRotation(grabPoint.forward);
        player.transform.rotation = lookRot;

        if (!player.holdPressed)
        {
            targetRb.linearVelocity = Vector3.zero;
            player.SwitchState(player.idleState);
            return;
        }

        if (player.moveInput == Vector2.zero)
        {
            player.grabObjectState.SetTarget(
                targetRb.GetComponent<PushableObject>(),
                grabPoint
            );
            player.SwitchState(player.grabObjectState);
            return;
        }



        Vector3 input = new Vector3(player.moveInput.x, 0f, player.moveInput.y);
        float dot = Vector3.Dot(player.transform.forward, input);

        if (dot < -0.1f)
        {
            player.pullState.SetTarget(targetRb, grabPoint);
            player.SwitchState(player.pullState);
            return;
        }

        // Richtung des Push
        Vector3 moveDir = player.transform.forward * dot * pushSpeed;

        // Nur XZ behalten, Y nicht anfassen
        Vector3 newVelocity = new Vector3(moveDir.x, targetRb.linearVelocity.y, moveDir.z);
        targetRb.linearVelocity = newVelocity;

        //player.SetPushPullAnimationSpeed(new Vector3(newVelocity.x, 0f, newVelocity.z).magnitude);


        // ANIMATION ---- geschwindigkeit berechnen (negativ = rückwärts, positiv = vorwärts)
        Vector3 flatVel = new Vector3(newVelocity.x, 0f, newVelocity.z);
        float speed = flatVel.magnitude;
        float direction = Mathf.Sign(Vector3.Dot(player.transform.forward, flatVel));
        float pushPullSpeed = direction * speed;

        player.animator.SetFloat("PushPullSpeed", pushPullSpeed, 0.1f, Time.deltaTime);


        //Debug.Log(speed);

    }

    public override void onUpdate(PlayerStateManager player)
    { 
        player.soundManager.PlaySingleRandomSoundEffect(player.soundManager.pullSounds, player.transform, 0.1f);
    }

    public override void onExit(PlayerStateManager player)
    {
        Debug.Log("→ PUSH ENDE");
        //player.animator.SetBool("IsPushing", false);
        //player.animator.SetBool("IsGrabbing", false);
        player.animator.ResetTrigger("DoPushPullGrab");

        if (targetRb != null)
        {
            targetRb.linearVelocity = Vector3.zero;

            PushableObject pushable = targetRb.GetComponent<PushableObject>();
            if (pushable != null)
            {
                pushable.ResetPhysics(); // <- HIER EINBAUEN!
            }
        }

        targetRb = null;
        grabPoint = null;
    }

}
