//using System.Numerics;
using UnityEngine;
using TMPro;   // damit man Text benutzen kannst

public class PlayerMovement : MonoBehaviour
{                                                               //mit [SerializeField] --> private + gleichzeitig für den Editor sichtbar
    [SerializeField] private float moveSpeed = 5f;              // in Unity-Einheit per second
    [SerializeField] private float crouchSpeed = 2f;            // wie schnell beim crouchen
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float runSpeed = 10f;              // Run-Geschwindigkeit
    [SerializeField] private float pushSpeed = 2f;              // Push/Pull Geschwindigkeit
    [SerializeField] private float jumpForce = 5f;              // Sprungstärke
    [SerializeField] private float fallMultiplier = 2.5f;       // Für Schnelleres Fallen
    [SerializeField] private float airControlMultiplier = 0.1f; // Steuerung in der Luft nur halb so stark

    // LAYERS
    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;             // Was zählt als "Boden"? 
    [SerializeField] private LayerMask bigObject;               // Was zählt als "big Object" --> alle verschiebbaren (größeren) Objekte!   
    
    [Header("Push Settings")]
    [SerializeField] private float pushDistance = 0.01f;         // Abstand für Schieben erlauben
    //[SerializeField] private TextMeshProUGUI pushPromptText;    // UI-Text anzeigen ("Press E to Push")  --> verworfen, aber für UI maybe nochmal hilfreich



    private bool isGrounded;                                    // Ist Spieler gerade auf dem Boden? (jumping)
    private bool isOnBigObject;                                 // damit Spieler auch auf schiebbaren Objekten springen kann 
    private bool jumpInput;                                     // wird in Update gesetzt, in FixedUpdate benutzt (jumping)
    private bool isCrouching;
    private bool isRunning;
    private bool isPushing;
    
    private Rigidbody rb;                                       //rigid body reference
    private Rigidbody objectToPush;                             // Aktuelles bigObject, das gerade gepusht wird
    private Vector3 moveDir;



    //Bei Crouch den Collider halbieren
    private CapsuleCollider capsuleCollider;
    private float originalHeight;
    private Vector3 originalCenter;


    void Start()                                                // Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        rb = GetComponent<Rigidbody>();

        // Bei Crouch Collider halbieren
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalHeight = capsuleCollider.height;
        originalCenter = capsuleCollider.center;
    }

    void Update()                                                   // Update is called once per frame
    {
        // Bodencheck
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);
        isOnBigObject = Physics.Raycast(transform.position, Vector3.down, 0.1f, bigObject);
        
        
        // Debug.Log("isGrounded: " + isGrounded);                  //Debugging


        // INPUT  Keyboard
        Vector2 inputVector = new Vector2(0, 0);                    //Start Vektor --> erstmal nur 2D, weil die Eingabetasten W-A-S-D auf Tastatur auch nur für 2D sind

        if (Input.GetKey(KeyCode.W)) inputVector.y = +1;            //Debug.Log("W" + inputVector);     //TODO: ignorieren, wenn in 2D Movement wechselt
        if (Input.GetKey(KeyCode.S)) inputVector.y = -1;            //TODO: ignorieren, wenn in 2D Movement wechselt
        if (Input.GetKey(KeyCode.A)) inputVector.x = -1;
        if (Input.GetKey(KeyCode.D)) inputVector.x = +1;
        
                
        //FIRST GET INPUT -- THEN ACTUALLY MOVE THE OBJECT
        inputVector = inputVector.normalized;                       // Normalisieren, damit man diagonal nicht plötzlich schneller ist
        moveDir = new Vector3(inputVector.x, 0, inputVector.y);     //keep input Vector separate from the movement Vector

        // JUMP INPUT
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || isOnBigObject && !isCrouching) {jumpInput = true;}                //Springen nur dann, wenn Spieler Boden berührt & NICHT crouched  (+ auf verschiebbaren bigObjects geht Springen auch)
        
        // CROUCH INPUT
        isCrouching = Input.GetKey(KeyCode.LeftControl);                  //crouch solange gedrückt ist
        //Debug.Log("Crouching: " + isCrouching);                     

        // RUN INPUT
        isRunning = Input.GetKey(KeyCode.LeftShift) && !isCrouching;            // Rennen nur, wenn kein Crouch!
        //Debug.Log("is running " + isRunning);

        /*if (inputVector != Vector2.zero)
        {
            Debug.Log("Input: " + inputVector);
        } */

        HandlePushPrompt();                                         // Text einblenden
    }

    private void HandlePushPrompt()
    {
        //Ray ray = new Ray(transform.position, transform.forward);                 
        Vector3 rayOrigin = transform.position + new Vector3(0, originalCenter.y, 0);
        Ray ray = new Ray(rayOrigin, transform.forward);            //Ray ray = new Ray(rayOrigin, transform.forward * pushDistance);

        Debug.DrawRay(rayOrigin, transform.forward * pushDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, pushDistance, bigObject))
        {
            isPushing = true;
            objectToPush = hit.rigidbody;
            objectToPush.transform.parent = this.transform;         // <-- Object anhängen!

            Debug.Log("Start Pushing: " + objectToPush.name);       // just DEBUGGING
        }
        else
        {
            if (objectToPush != null)
            {
                Debug.Log("Stop Pushing: " + objectToPush.name);    // just DEBUGGING

                objectToPush.linearVelocity = Vector3.zero;        // Geschwindigkeit auf 0 setzen, auch wenn LOSGELASSEN!!!
                objectToPush.angularVelocity = Vector3.zero;

                objectToPush.transform.parent = null;               // <--- Objekt wieder ablösen!
            }

        isPushing = false;
        objectToPush = null;
        
        }
            
    }



    void FixedUpdate()
    {
        
        /*float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;                               //MovePosition(), pusht stumpf durch Wände = kein Stop bei Kollision --> niedrige Hindernisse = Buggy :(
        rb.MovePosition(rb.position + moveDir * currentSpeed * Time.fixedDeltaTime);                // Bewegung über Rigidbody*/

        //float currentSpeed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : moveSpeed);                        //wenn crouch = langsam, wenn rennen = schnell, sonst normal 
        float currentSpeed = isCrouching ? crouchSpeed : (isPushing ? pushSpeed : (isRunning ? runSpeed : moveSpeed));      //noch verwirrender + langsamer wenn pushing

        
        float controlMultiplier = isGrounded ? 1f : airControlMultiplier;                                           // Steuerung, wenn Spieler in der Luft ist beschränken!
        
        Vector3 velocity = moveDir * currentSpeed * controlMultiplier;
        velocity.y = rb.linearVelocity.y;                                                                           // y-Velocity behalten (damit Jump/Gravity noch funktioniert)
        rb.linearVelocity = velocity;


        // Rotation über Rigidbody
        if (moveDir != Vector3.zero)                                                                                //Damit Spieler in richtige Richtung schat beim laufen
        {
            Vector3 direction = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.fixedDeltaTime);       //Spieler dreht sich sonst zu schnell --> also SLERP (für rotation gut - sonst LERP) nutzen!!! --> Slerp interpoliert zwischen Punkt a & b ind Zeit t 
            rb.MoveRotation(Quaternion.LookRotation(direction));                                                    //Drehung über rigid Body anwenden    
        }

        // JUMP
        if (jumpInput)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);                          // Y-Velocity zurücksetzen
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpInput = false;                                                                                      // Zurücksetzen, damit nicht dauerhaft gesprungen wird
        }

        // Bei Crouch Collider halbieren
        if (isCrouching)
        {
            capsuleCollider.height = originalHeight / 2f;
            capsuleCollider.center = new Vector3(
                originalCenter.x,
                originalCenter.y / 2f,
                originalCenter.z
            );
        }
        else
        {
            capsuleCollider.height = originalHeight;
            capsuleCollider.center = originalCenter;
        }

        // Objekt mitschieben, wenn Pushing aktiv
        if (isPushing && objectToPush != null)
        {
            float distanceToObject = Vector3.Distance(transform.position, objectToPush.position);       // Überprüfe Abstand

            if (distanceToObject < pushDistance)                                        // nur wenn wirklich nah genug!
            {
                Vector3 pushMovement = moveDir * pushSpeed * Time.fixedDeltaTime;

                objectToPush.MovePosition(objectToPush.position + pushMovement);        // Position ändern

                Debug.Log("Pushing Movement: " + pushMovement);                         //Just DEBUGGING

                objectToPush.linearVelocity = Vector3.zero;                             // Bewegung auf 0 --> verhindert komisches Rutschen
                objectToPush.angularVelocity = Vector3.zero;                            // falls nötig: auch Stop für Drehen
            } 
            else 
            {
                Debug.Log("Push abgebrochen: Zu weit weg");                             // Zu weit weg → Push abbrechen
                isPushing = false;
                objectToPush = null;
            }
        }


        // Fallbeschleunigung erhöhen
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            float gravityStrength = Mathf.Lerp(1f, fallMultiplier, -rb.linearVelocity.y / 10f);                         // je schneller er fällt, desto stärker zieht es den Spieler zum Boden runter --> kann man dann noch clippen, wenn man bestimmte Höhen im Level kennt
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityStrength - 1) * Time.fixedDeltaTime;
        }

    }

    /* void OnDrawGizmos()  //tried to see something for debugging - didn´t work lol
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2f);
    } */    
}
