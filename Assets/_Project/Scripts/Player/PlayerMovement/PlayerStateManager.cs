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
    public float jumpForce = 2f;
    public float airControlMultiplier = 0.3f;              //um mitten im Jump noch Richtung steuern zu können

    //crouch
    [HideInInspector] public CapsuleCollider capsuleCollider;
    [HideInInspector] public float originalHeight;
    [HideInInspector] public Vector3 originalCenter;
    
    // Booleans
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isCrouching;
    [HideInInspector] public bool interactPressed;

    //RayCasts
    //public bool isGrounded;                 
            //ob auf Boden, oder frei fallend?   --> wird jetzt als Methode genutzt, also egal

    //Speed
    public float walkSpeed = 2.5f;
    public float maxSpeed = 5f;
    public float crouchSpeed = 1f;


    //JUST DEBUGGING!!!!
    

    
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();             // <- Rigidbody Referenz setzen
        capsuleCollider = GetComponent<CapsuleCollider>();

        originalHeight = capsuleCollider.height;
        originalCenter = capsuleCollider.center;

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
        currentState.onExit(this);
        currentState = state;
        currentState.onEnter(this);                 //führt vom neuen State onEnter aus 

    }


    //Bewegung anwenden
    public void MovePlayer(float speed)
    {
        Vector3 velocity = moveDir * speed;
        velocity.y = rb.linearVelocity.y;           // Y-Achse (zB. durch Sprung) beibehalten
        rb.linearVelocity = velocity;
    }

    //Rotation zur Blickrichtung
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

    //um zu schauen, wie viel Platz an der Position vom Spieler ist --> z.B. bei crouch, ob man überhaupt aufstehen kann, oder ob da kein Platz mehr ist (oder maybe sogar bei jump anwenden)
    public bool HasHeadroom(float requiredHeight)
    {
        Vector3 rayOrigin = transform.position + Vector3.up * (capsuleCollider.height / 2f);
        float rayLength = requiredHeight - (capsuleCollider.height / 2f);

        Debug.DrawRay(rayOrigin, Vector3.up * rayLength, Color.red, 0.2f); // Zum Debuggen

        return !Physics.Raycast(
            rayOrigin,
            Vector3.up,
            rayLength,
            ~0,                                 // = Alle Layer
            QueryTriggerInteraction.Ignore      // = Trigger-Collider werden ignoriert
        );
    }

    public bool IsGrounded()                                                    //für Fall & Jump
    {
        float rayLength = 0.1f;
        Vector3 origin = transform.position + Vector3.up * 0.01f;

        Debug.DrawRay(origin, Vector3.down * rayLength, Color.green, 0.1f);     // Debug

        return Physics.Raycast(
            origin,
            Vector3.down,
            rayLength,
            ~0,
            QueryTriggerInteraction.Ignore                                      //wieder ignorieren, wenn Triggerbox 
        );
    }


    // my BOOLEANS
    public bool IsFalling()
    {
        return !IsGrounded() && rb.linearVelocity.y < 0f;
    }

    public bool JumpAllowed()                                   //steht bei Idle, Walk und Run drinne!  --> damit man gleichzeitig Logik bearbeiten kann --> weniger copy paste
    {
        return jumpPressed && IsGrounded() && !isCrouching;
    }

}
