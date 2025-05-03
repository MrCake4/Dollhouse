using UnityEngine;

public class PlayerStateManager : MonoBehaviour                 //Script direkt für den Player
{

    /// every state declared here
     BasePlayerState currentState;
     public IdleState idleState = new IdleState();
     public WalkState walkState = new WalkState();
     public RunState runState= new RunState();
     public JumpState jumpState = new JumpState();
     public FallState fallState = new FallState();
     public PushState pushState = new PushState();
     public CrouchState crouchState = new CrouchState();
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = idleState;
        currentState.onEnter(this);                 //this = alle Variablen/ Methoden aus dieser Klasse hier
    }

    // Update is called once per frame
    void Update()
    {
        currentState.onUpdate(this);                //beim aktuellen State Update() aufrufen
    }

    void FixedUpdate(){
        currentState.onFixedUpdate(this);           //beim aktuellen State FixedUpdate() aufrufen
    }

    public void SwitchState(BasePlayerState state){
        currentState = state;
        currentState.onEnter(this);                 //führt vom neuen State onEnter aus 


    }
}
