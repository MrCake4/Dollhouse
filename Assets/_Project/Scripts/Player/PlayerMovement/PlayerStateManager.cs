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
    //private PlayerItemHandler PlayerItemHandler;                //CARRY
    public PullUpState pullUpState = new PullUpState();


        // important variables
    //Eingaben - Bewegung + Ausrichtung
    [HideInInspector] public Vector2 moveInput;             // WASD als Vector2
    [HideInInspector] public Vector3 moveDir;               // Richtung im 3D-Raum
    [HideInInspector] public Rigidbody rb;                  //rigid body reference
    public float rotateSpeed = 10f;
    public float jumpForce = 2f;
    public float jumpHeight = 1.5f;                         // gewünschte konstante Sprunghöhe

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
    [HideInInspector] public bool pullUpPressed;

    //for the RayCasts
    public LayerMask bigObjectLayer;
    //public LayerMask smallObjectLayer;
        

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
        pullUpPressed = Input.GetMouseButtonDown(0);

        //if(pullUpPressed){Debug.Log("PullUpTry");}

        //Zustand updaten
        currentState.onUpdate(this);                //beim aktuellen State Update() aufrufen

    }




    void FixedUpdate(){
        isRunning = Input.GetKey(KeyCode.LeftShift);        //Für JUMP & Fall --> damit man direkt weiterrennen kann
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
        return jumpPressed 
        && IsGrounded() 
        && !isCrouching
        &&HasHeadroom(1.2f);            //1.2f damit der ray länger ist als der Ray der schaut, ob man grounded ist --> dann kann man eigenntlich immer den FallState erreichen
    }

    public bool PushAllowed(out Rigidbody pushTarget)
    {
        pushTarget = null;

        Vector3 rayOrigin = transform.position + Vector3.up * (capsuleCollider.height / 3f); // Mitte der Figur
        Vector3 direction = transform.forward;
         float rayLength = 0.6f;

        // 🔧 Zeichne Ray zur visuellen Kontrolle
        Debug.DrawRay(rayOrigin, direction * rayLength, Color.blue, 0.1f);
        

        if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, 0.6f, bigObjectLayer, QueryTriggerInteraction.Ignore))
        {
            // Prüfe Winkel der Normale
            float angle = Vector3.Angle(-hit.normal, direction);
            if (angle < 25f) // ± einstellbarer Toleranzwinkel
            {
                Rigidbody hitRb = hit.collider.attachedRigidbody;
                if (hitRb != null && !hitRb.isKinematic)
                {
                    pushTarget = hitRb;
                    Debug.Log("congratulations, you can push!");
                    return true;
                } else{ Debug.Log("Hit, aber kein Rigidbody oder ist kinematic."); }
            } else{ Debug.Log("Winkel zu steil: " + angle); }
        }

        return false;
    }


    //FALL / JUMP
    public Vector3 GetHorizontalVelocity()
    {
        return new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    }

    public float GetHorizontalSpeed()
    {
        return GetHorizontalVelocity().magnitude;
    }

    public void ApplyAirControl(PlayerStateManager player)                                         //Damit man mit WASD noch leicht umlenken kann in der Luft. ohne den  Fall nach unten zu beeinflussen
        {
            Vector3 airMove = player.moveDir * player.maxSpeed * player.airControlMultiplier;
            Vector3 currentVel = player.rb.linearVelocity;

            currentVel.x = Mathf.Lerp(currentVel.x, airMove.x, Time.fixedDeltaTime * 2f);
            currentVel.z = Mathf.Lerp(currentVel.z, airMove.z, Time.fixedDeltaTime * 2f);

            player.rb.linearVelocity = currentVel;
        }


}
