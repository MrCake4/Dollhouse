using NavKeypad;
using UnityEngine;

public class PasswordHandler : MonoBehaviour
{
    int password;   // a pasword variable to store the generated password
    [SerializeField] Keypad keypad; // reference to the Keypad component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Generate a random 3-digit password
        password = Random.Range(100, 1000);
        Debug.Log("Generated Password: " + password);

        keypad.setKeyPadCombo(password); // Set the generated password in the Keypad component
    }
}
