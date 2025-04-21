using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    //public float moveSpeed = 7f;                      //public, damit man es im Editor sieht/ ändern kann (Inspector) --> kann man dann auch während runtime ändern  !!Nachteil: andere  Klassen haben Zugriff & können Variable verändern
    [SerializeField] private float moveSpeed = 7f;      //Alternativ mit [SerializeField] --> jetzt  private + gleichzeitig für den Editor sichtbar


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 inputVector = new Vector2(0, 0);            //Start Vektor

        if(Input.GetKey(KeyCode.A)){                        // wenn Taste ... gedrückt wird --> 
            
            inputVector.x = -1;
            //Debug.Log("A" + inputVector);
        }
        if(Input.GetKey(KeyCode.D)){
            
            inputVector.x = +1;
            //Debug.Log("D" + inputVector);
        }
        if(Input.GetKey(KeyCode.Space)){
            Debug.Log("Space");

        }

    //FIRST GET INPUT -- THEN ACTUALLY MOVE THE OBJECT

        inputVector = inputVector.normalized;               // Normalisieren (eigentlich eher wichtig, wenn man zwei Vektoren addiert --> hier aber nur rechts oder links bisher --> also egal)

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);             //keep input Vector separate fro the movement Vector
        transform.position += (Vector3)moveDir * moveSpeed * Time.deltaTime;         // zu 3D Vektor casten für die Spielwelt
        //damit speed vom Player nicht abhängig von der Frame Rate ist --> multiply by (delta) time   ==> weil das alleine ultra langsam ist --> *moveSpeed


        // Debug.Log(Vector2.zero);
        if (inputVector == Vector2.zero) {Debug.Log(" - ");} else {Debug.Log(inputVector);}         //nur fürs Debugging!!!

    }
}
