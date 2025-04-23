//using System.Numerics;
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

        UnityEngine.Vector2 inputVector = new UnityEngine.Vector2(0, 0);            //Start Vektor --> nur 2D, weil die Eingabetasten W-A-S-D auf Tastatur auch nur für 2D sind

        
        if(Input.GetKey(KeyCode.W)){                //TODO: ignorieren, wenn in 2D Movement wechselt
            
            inputVector.y = +1;
            //Debug.Log("W" + inputVector);
        }
        if(Input.GetKey(KeyCode.A)){                        // wenn Taste ... gedrückt wird --> 
            
            inputVector.x = -1;
            //Debug.Log("A" + inputVector);
        }
        if(Input.GetKey(KeyCode.S)){                //TODO: ignorieren, wenn in 2D Movement wechselt
            
            inputVector.y = -1;
            //Debug.Log("S" + inputVector);
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

        UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(inputVector.x, 0, inputVector.y);             //keep input Vector separate fro the movement Vector
        transform.position += (UnityEngine.Vector3)moveDir * moveSpeed * Time.deltaTime;         // zu 3D Vektor casten für die Spielwelt
        //damit speed vom Player nicht abhängig von der Frame Rate ist --> multiply by (delta) time   ==> weil das alleine ultra langsam ist --> *moveSpeed


        // damit Spieler in richtige Richtung schaut beim laufen, gibt es versch. Möglichkeiten: transform.rotation (arbeitet mit Quarternions); transform.eulerAngles(); oder transform.LookAt(Vector3 worldPosition) (rotate to look at a given poin --> müste man halt erst vorm Spieler berechnen);
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed) ;          //forward vector --> basically  die Move Direction    
        //Spieler dreht sich sonst zu schnell --> also SLERP (für rotation gut - sonst nur LERP) nutzen!!! --> Slerp interpoliert zwischen Punkt a & b ind Zeit t   


        // Debug.Log(Vector2.zero);
        if (inputVector == UnityEngine.Vector2.zero) {Debug.Log(" - ");} else {Debug.Log(inputVector);}         //nur fürs Debugging!!!

    }
}
