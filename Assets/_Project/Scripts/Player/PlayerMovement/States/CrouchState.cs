using Unity.VisualScripting;
using UnityEngine;

public class CrouchState : BasePlayerState
{
    float distanceToLowCrouchPoint;

    public override void onEnter(PlayerStateManager player)
    {
        //distanceToLowCrouchPoint berechnen (wenn es denn einen gibt!)
        //if distanceToLowCrouchPoint < xy --> dann Collider noch kleiner machen

        // Collider halbieren
        player.capsuleCollider.height = player.originalHeight / 1.55f;
        player.capsuleCollider.center = new Vector3(
            player.originalCenter.x,
            player.originalCenter.y / 1.55f,
            player.originalCenter.z
        );

        Debug.Log("Crouching");
        //player.animator.SetBool("IsCrouching", true);
        player.animator.SetTrigger("DoCrouch");
    }


    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        // Wenn Crouch losgelassen → Zustand neu evaluieren
        if (!player.isCrouching && player.HasHeadroom(player.originalHeight))
        {
            // Wenn keine Bewegung → Idle
            if (player.moveInput == Vector2.zero)
            {                      //SWITCH Idle
                //onExit(player);
                player.SwitchState(player.idleState);
            }
            // Wenn Bewegung → Walk (kein Run möglich beim Crouch)
            else
            {
                //onExit(player);
                player.SwitchState(player.walkState);                   //SWITCH Walk
            }
        }

        if (player.IsFalling())
        {                                         //SWITCH Fall
            player.SwitchState(player.fallState);
            //Debug.Log("Switcherooo");
        }

        //___________________ANIMATION_____________________
        float speed = player.moveInput.magnitude;
        player.animator.SetFloat("CrouchSpeed", speed, 0.1f, Time.deltaTime);



        // Blend-Kalkulation für LowCrouch _________________ ANIMATION

        if (player.lowCrouchPoint != null)
        {
            //Distance ausrechnen und Methode aus Animator aufrufen, die einstellt, wie weit sich der Kopf senken soll
            //bisherige Animation soll dann an dem Kopf-Part überschrieben werden --> aber an sich der saubere Übergang von z.B. walk/idle etc soll zu crocuh dann trotzdem sauber rüberlaufen, wie es im Animator wäre
        }
        else
        {

        }

    }


    public override void onFixedUpdate(PlayerStateManager player)          //Physik
    {
        player.MovePlayer(player.crouchSpeed);
        player.RotateToMoveDirection();
    }


    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        // Collider zurücksetzen
        player.capsuleCollider.height = player.originalHeight;
        player.capsuleCollider.center = player.originalCenter;

        //player.animator.SetBool("IsCrouching", false);
        player.animator.ResetTrigger("DoCrouch");
    }
}
