using UnityEngine;

public class PlayerStateManager : MonoBehaviour                 //Script direkt für den Player
{

    // every state declared here
    BasePlayerState currentState;
    public IdleState idleState = new IdleState();
    public WalkState walkState = new WalkState();
    public RunState runState = new RunState();
    public JumpState jumpState = new JumpState();
    public FallState fallState = new FallState();
    public PushState pushState = new PushState();
    public PullState pullState = new PullState();
    public GrabObjectState grabObjectState = new GrabObjectState();
    public CrouchState crouchState = new CrouchState();
    //private PlayerItemHandler PlayerItemHandler;                //CARRY
    public PullUpState pullUpState = new PullUpState();
    public HangState hangState = new HangState();
    public DeadState deadState = new DeadState();               //für den Fall, dass der Spieler stirbt


    //CHECK - COLLIDERS
    [SerializeField] public GroundCheck groundCheck;


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
    //[HideInInspector] public bool pullUpPressed;
    [HideInInspector] public bool holdPressed;      //Festhalten --> für PullUp, Hang, climb, etc
    [HideInInspector] public bool pickUpPressed;
    [HideInInspector] public bool is2DMode = false;         // 2.5D MOVEMENT


    //for the RayCasts
    public LayerMask bigObjectLayer;
    //public LayerMask smallObjectLayer;

    [Header("Player Speed")]
    //Speed
    public float walkSpeed = 2.5f;
    public float maxSpeed = 5f;
    public float crouchSpeed = 1f;


    //________________ANIMATION_________________
    public Animator animator;

    // ______________LEVEL REFERENCES _________________________
    [Header("Level References")]
    public Transform lowCrouchPoint;        // kann im Inspector gesetzt werden

    private float crouchBlend;              // interner Blendwert
    public void SetCrouchBlend(float value)
    {
        crouchBlend = Mathf.Clamp01(value);
    }
    public float GetCrouchBlend() => crouchBlend;




    // Debugging
    [Header("Debugging")]
    public bool isInvincible = false; // Spieler ist unverwundbar, z.B. während des Respawns

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

        animator = GetComponentInChildren<Animator>();

    }




    // Update is called once per frame
    void Update()
    {
        //float inputX = Input.GetAxis("Horizontal"); // A/D oder Left Stick X
        //float inputY = Input.GetAxis("Vertical");   // W/S oder Left Stick Y

        // Eingaben zentral erfassen
        Vector2 keyboardInput = Vector2.zero;       //für Keyboard-Eingabe

        if (Input.GetKey(KeyCode.W)) keyboardInput.y = +1;
        if (Input.GetKey(KeyCode.S)) keyboardInput.y = -1;
        if (Input.GetKey(KeyCode.A)) keyboardInput.x = -1;
        if (Input.GetKey(KeyCode.D)) keyboardInput.x = +1;


        if (keyboardInput != Vector2.zero)                  // Normalisieren nur für Tastatur
        {
            moveInput = keyboardInput.normalized;
        }
        else
        {
            // Controller-Stick → analog übernehmen
            float stickX = Input.GetAxis("Horizontal");     // Controller Left Stick X
            float stickY = Input.GetAxis("Vertical");       // Controller Left Stick Y

            moveInput = new Vector2(stickX, stickY);        // NICHT normalisieren!
        }

        moveDir = new Vector3(moveInput.x, 0, moveInput.y);  // in Welt-Richtung

        if (is2DMode)
        {
            moveDir = new Vector3(moveDir.x, 0, 0); // kein Vor/zurück, nur links/rechts
        }


        /*Hinten links = ducken
        Hinten rechts = rennen
        B = Objekt aufheben/ fallen lassen          --> Input.GetKey(KeyCode.Joystick1Button1)
        X = Interact


            Joystick1Button3 = Y
        */


        // andere Eingaben  --> auskommentierte sind die für PS5 Controller
        //jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1);     // X!!!
        jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0);     // jetzt A
        //isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button10);          //Rennen mit reindrücken von L
        //isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button8);         //reindrücken von L
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button5);         //R1

        isCrouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.Joystick1Button4);      //L1        //gilt für PS5 & X-Box
        //interactPressed = Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.Joystick1Button0);        //Viereck
        interactPressed = Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button2);        //X
        pickUpPressed = Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Joystick1Button1);
        //holdPressed = Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Joystick1Button5);            //Trying to hold onto something?        //R1    
        //holdPressed = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Joystick1Button5);            //oben rechts R1
        holdPressed = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Joystick1Button3);            //Y


        //Zustand updaten
        currentState.onUpdate(this);                //beim aktuellen State Update() aufrufen

    }




    void FixedUpdate()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift ) || Input.GetKey(KeyCode.Joystick1Button8);        //Für JUMP & Fall --> damit man direkt weiterrennen kann
        currentState.onFixedUpdate(this);           //beim aktuellen State FixedUpdate() aufrufen

    }


    public void SwitchState(BasePlayerState state)
    {
        currentState.onExit(this);
        currentState = state;
        currentState.onEnter(this);                 //führt vom neuen State onEnter aus 

    }


    //Bewegung anwenden
    public void MovePlayer(float speed)
    {
        Vector3 velocity = moveDir * speed;
        velocity.y = rb.linearVelocity.y;           // Y-Achse (zB. durch Sprung) beibehalten

        if (is2DMode)
        {
            velocity.z = 0f; // endgültig Z-Achse kappen
        }

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
        // Position der Box: etwas über dem Kopf
        Vector3 boxOrigin = transform.position + Vector3.up * (capsuleCollider.height / 2f);
        
        // Box-Größe (halbiert = HalfExtents!)
        Vector3 boxHalfExtents = new Vector3(0.3f, 0.1f, 0.3f); // breit genug für Kopf + leichte Toleranz

        float castDistance = requiredHeight - (capsuleCollider.height / 2f);
        
        bool blocked = Physics.BoxCast(
            boxOrigin,
            boxHalfExtents,
            Vector3.up,
            out RaycastHit hit,
            Quaternion.identity,
            castDistance,
            ~0,
            QueryTriggerInteraction.Ignore
        );

        return !blocked;
    }

    // my BOOLEANS
    public bool IsFalling()
    {
        return !groundCheck.isGrounded && rb.linearVelocity.y <= 0f;
    }

    public bool HasLanded()
    {
        return groundCheck.isGrounded && rb.linearVelocity.y <= 0.01f;
    }

    public bool JumpAllowed()                                   //steht bei Idle, Walk und Run drinne!  --> damit man gleichzeitig Logik bearbeiten kann --> weniger copy paste
    {
        return jumpPressed
        && groundCheck.isGrounded
        && !isCrouching
        && HasHeadroom(1.2f);            //1.2f damit der ray länger ist als der Ray der schaut, ob man grounded ist --> dann kann man eigenntlich immer den FallState erreichen
    }




    //GRAB


    public void TryGrab()                   //nur für hochziehen und ranhängen für Dinge in der Luft --> nur beim SPRINGEN oder FALLEN
    {
        if (!holdPressed) return;

        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return;

        box.enabled = true;

        Collider[] hits = Physics.OverlapBox(
            box.bounds.center,
            box.bounds.extents,
            transform.rotation,
            ~0,
            QueryTriggerInteraction.Collide
        );

        box.enabled = false;

        foreach (Collider col in hits)
        { 
            if (col.CompareTag("HangOnto"))
            {
                Vector3 closestPoint = col.ClosestPoint(transform.position);
                hangState.SetHangPosition(closestPoint);
                SwitchState(hangState);
                return;
            }
        } 

        // Nichts gefunden
    }


    public void TryGrabObject()
    {
        if (!holdPressed) return;

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        float rayLength = 0.6f;

        if (Physics.Raycast(origin, transform.forward, out RaycastHit hit, rayLength, bigObjectLayer))
        {
            PushableObject pushable = hit.collider.GetComponentInParent<PushableObject>();
            if (pushable != null && pushable.IsGrabAllowed())
            {
                Transform grabPoint = pushable.GetGrabPoint();
                if (grabPoint != null)
                {
                    grabObjectState.SetTarget(pushable, grabPoint);
                    //Debug.Log("cannot switch m´lady");
                    SwitchState(grabObjectState); // später kannst du auch direkt holdState nutzen
                    return;
                }
            }
        }
    }


    public bool CanPullUp()
    {
        //schaue ich richtig auf die Ledge (max. 50 Grad Abweichung)
        //ist es im BoxCollider

        //wenn true dann PullUpstate.SetLedgePosition(Hit.Collider.transform.position)

        Debug.Log("I AM F***** TRYING");

        if (!jumpPressed) return false;

        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return false;

        box.enabled = true;

        Collider[] hits = Physics.OverlapBox(
            box.bounds.center,
            box.bounds.extents,
            transform.rotation,
            ~0,
            QueryTriggerInteraction.Collide
        );

        box.enabled = false;

        foreach (Collider col in hits)
        {
            if (col.CompareTag("mediumLedge"))
            {

                Debug.Log("FOUND A MEDIUM LEDGE");

                // Richtung prüfen
                Vector3 toLedge = col.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, toLedge);
                /*if (angle > 50f)
                {
                    Debug.Log("ANGLE IS SHIT");
                    return false;
                }*/

                pullUpState.SetLedgePos(col.transform.position);
                SwitchState(pullUpState);
                return true;
            }
        }

        return false;
    }




    // ============================ FOR ANIMATION ONLY ============================ 
    
    // Platzhalter für spätere Animationen



    //FALL / JUMP
    public Vector3 GetHorizontalVelocity()
    {
        return new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    }
    public float GetVerticalVelocity()
    {
        return rb.linearVelocity.y;
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

    //_____________________________ANIMATION___________________
    public void ResetAllAnimationBools()
    {
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsCrouching", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsPushing", false);
        animator.SetBool("IsPulling", false);
        animator.SetBool("IsGrabbing", false);
        animator.SetBool("IsHolding", false);
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsPullingUp", false);

        // füge hier alle deine Parameter ein
    }


    public BasePlayerState getCurrentState => currentState;          //Getter für den aktuellen State

}
