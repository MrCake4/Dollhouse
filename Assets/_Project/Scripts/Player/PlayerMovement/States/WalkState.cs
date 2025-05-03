using UnityEngine;

public class WalkState : BasePlayerState
{
    private Vector3 moveDir;

    public override void onEnter(PlayerStateManager player)
    {
        Debug.Log("walking");
    }
    public override void onUpdate(PlayerStateManager player)               //pro Frame
    {
        Vector2 inputVector = new Vector2(0, 0);                    //Start Vektor --> 2D, da Tasten W-A-S-D auch nur 2D sind
        
        if (Input.GetKey(KeyCode.W)) inputVector.y = +1;            //Debug.Log("W" + inputVector);     //TODO: ignorieren, wenn in 2D Movement wechselt
        if (Input.GetKey(KeyCode.S)) inputVector.y = -1;            //TODO: ignorieren, wenn in 2D Movement wechselt
        if (Input.GetKey(KeyCode.A)) inputVector.x = -1;
        if (Input.GetKey(KeyCode.D)) inputVector.x = +1;

        inputVector = inputVector.normalized;                       // Normalisieren, damit man diagonal nicht plötzlich schneller ist
        moveDir = new Vector3(inputVector.x, 0, inputVector.y);     //keep input Vector separate from the movement Vector


        // switch State
        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) { player.SwitchState(player.idleState);}          // + GroundCheck muss auch true sein! (um still stehen von "Fall" zu unterscheiden)
    }
    public override void onFixedUpdate(PlayerStateManager player)          //Physik
    {
        
    }
    public override void onExit(PlayerStateManager player)                 //was passiert, wenn aus State rausgeht
    {
        
    }
}
