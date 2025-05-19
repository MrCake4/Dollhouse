using UnityEngine;

public class CrouchState : BasePlayerState
{
    public override void onEnter(PlayerStateManager player)
    {
        // Collider halbieren
        player.capsuleCollider.height = player.originalHeight / 2f;
        player.capsuleCollider.center = new Vector3(
            player.originalCenter.x,
            player.originalCenter.y / 2f,
            player.originalCenter.z
        );

        Debug.Log("Crouching");
    }


    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        // Wenn Crouch losgelassen → Zustand neu evaluieren
        if (!player.isCrouching && player.HasHeadroom(player.originalHeight))
        {
            // Wenn keine Bewegung → Idle
            if (player.moveInput == Vector2.zero){                      //SWITCH Idle
                //onExit(player);
                player.SwitchState(player.idleState);
            }
            // Wenn Bewegung → Walk (kein Run möglich beim Crouch)
            else{
                //onExit(player);
                player.SwitchState(player.walkState);                   //SWITCH Walk
            }
        }

        if(player.IsFalling()){                                         //SWITCH Fall
            player.SwitchState(player.fallState);
            //Debug.Log("Switcherooo");
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
    }
}
