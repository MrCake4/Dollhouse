using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
        inputVector = inputVector.normalized;               // Normalisieren (eigentlich eher wichtig, wenn man zwei Vektoren addiert --> hier aber nur rechts oder links bisher --> also egal)

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += (Vector3)inputVector;         // zu 3D Vektor casten für die Spielwelt


        // Debug.Log(Vector2.zero);
        if (inputVector == Vector2.zero) {Debug.Log(" - ");} else {Debug.Log(inputVector);}         //nur fürs Debugging!!!

    }
}
