using Unity.VisualScripting;
using UnityEngine;

public class IdleState : BasePlayerState
{
    public override void onEnter(PlayerStateManager player)
    {
        
    }
    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        Debug.Log("not walking anymore");
        // switch to WalkState
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) { player.SwitchState(player.walkState);}
    }
    public override void onFixedUpdate(PlayerStateManager player)          //Physik
    {

    }
    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        
    }
}
