//using System.Numerics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{                                                               //mit [SerializeField] --> private + gleichzeitig für den Editor sichtbar
    [SerializeField] private float moveSpeed = 7f;              // in Unity-Einheit per second
    [SerializeField] private float crouchSpeed = 3f;            // wie schnell beim crouchen
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;              // Sprungstärke
    [SerializeField] private LayerMask groundLayer;             // Was zählt als "Boden"?


    private bool isGrounded;                                    // Ist Spieler gerade auf dem Boden? (jumping)
    private bool jumpInput;                                     // wird in Update gesetzt, in FixedUpdate benutzt (jumping)
    private bool isCrouching;

    
    private Rigidbody rb;                                       //rigid body reference
    private Vector3 moveDir;

    void Start()                                                // Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()                                                   // Update is called once per frame
    {
        // Bodencheck
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
        
        Debug.Log("isGrounded: " + isGrounded);


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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching) {jumpInput = true;}                //Springen nur dann, wenn Spieler Boden berührt & NICHT crouched
        
        // CROUCH INPUT
        isCrouching = Input.GetKey(KeyCode.LeftControl);                  //crouch solange gedrückt ist
        Debug.Log("Crouching: " + isCrouching);                     

        if (inputVector != Vector2.zero)
        {
            Debug.Log("Input: " + inputVector);
        }
    }

    void FixedUpdate()
    {
        // Bewegung über Rigidbody
        //rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
        rb.MovePosition(rb.position + moveDir * currentSpeed * Time.fixedDeltaTime);

        // Rotation über Rigidbody
        if (moveDir != Vector3.zero)                                                                                //Damit Spieler in richtige Richtung schat beim laufen
        {
            Vector3 direction = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.fixedDeltaTime);       //Spieler dreht sich sonst zu schnell --> also SLERP (für rotation gut - sonst LERP) nutzen!!! --> Slerp interpoliert zwischen Punkt a & b ind Zeit t 
            rb.MoveRotation(Quaternion.LookRotation(direction));                                                    //Drehung über rigid Body anwenden    
        }

        // JUMP
        if (jumpInput)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Y-Velocity zurücksetzen
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpInput = false;                                                  // Zurücksetzen, damit nicht dauerhaft gesprungen wird
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2f);
    }
}
