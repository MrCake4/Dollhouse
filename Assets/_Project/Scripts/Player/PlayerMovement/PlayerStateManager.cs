using UnityEngine;

public class PlayerStateManager : MonoBehaviour                 //Script direkt für den Player
{

    // every state declared here
     BasePlayerState currentState;
     public IdleState idleState = new IdleState();
     public WalkState walkState = new WalkState();
     public RunState runState= new RunState();
     public JumpState jumpState = new JumpState();
     public FallState fallState = new FallState();
     public PushState pushState = new PushState();
     public CrouchState crouchState = new CrouchState();


        // important variables
    //Eingaben - Bewegung + Ausrichtung
    [HideInInspector] public Vector2 moveInput;             // WASD als Vector2
    [HideInInspector] public Vector3 moveDir;               // Richtung im 3D-Raum
    [HideInInspector] public Rigidbody rb;                  //rigid body reference
    public float rotateSpeed = 10f;
    
    // Booleans
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isCrouching;
    [HideInInspector] public bool interactPressed;

    //RayCasts
    public bool isGrounded;                 //auf Boden, oder frei fallend?

    //Speed
    public float walkSpeed = 4f;
    public float maxSpeed = 7f;
    public float crouchSpeed = 2f;

    

    
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();             // <- Rigidbody Referenz setzen
        currentState = idleState;
        currentState.onEnter(this);                 //this = alle Variablen/ Methoden aus dieser Klasse hier
    }




    // Update is called once per frame
    void Update()
    {
        // Eingaben zentral erfassen
        moveInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) moveInput.y = +1;
        if (Input.GetKey(KeyCode.S)) moveInput.y = -1;
        if (Input.GetKey(KeyCode.A)) moveInput.x = -1;
        if (Input.GetKey(KeyCode.D)) moveInput.x = +1;

        moveInput = moveInput.normalized;
        moveDir = new Vector3(moveInput.x, 0, moveInput.y);  // in Welt-Richtung

        // andere Eingaben
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        isRunning = Input.GetKey(KeyCode.LeftShift);
        isCrouching = Input.GetKey(KeyCode.LeftControl);
        interactPressed = Input.GetKeyDown(KeyCode.E);

        //Zustand updaten
        currentState.onUpdate(this);                //beim aktuellen State Update() aufrufen
    }




    void FixedUpdate(){
        currentState.onFixedUpdate(this);           //beim aktuellen State FixedUpdate() aufrufen

    }




    public void SwitchState(BasePlayerState state){
        currentState = state;
        currentState.onEnter(this);                 //führt vom neuen State onEnter aus 


    }


    //Neue Methode: Bewegung anwenden
    public void MovePlayer(float speed)
    {
        Vector3 velocity = moveDir * speed;
        velocity.y = rb.linearVelocity.y;           // Y-Achse (zB. durch Sprung) beibehalten
        rb.linearVelocity = velocity;
    }

    //Neue Methode: Rotation zur Blickrichtung
    public void RotateToMoveDirection()
    {
        if (moveDir == Vector3.zero) return;                        //nur return = macht nix 

        Vector3 direction = Vector3.Slerp(                          //dreht sonst zu schnell --> SLERP nutzen!!! --> Slerp interpoliert zwischen Punkt a & b in Zeit t
            transform.forward,
            moveDir,
            rotateSpeed * Time.fixedDeltaTime
        );

        rb.MoveRotation(Quaternion.LookRotation(direction));        //Drehung über rigid Body anwenden
    }

}
