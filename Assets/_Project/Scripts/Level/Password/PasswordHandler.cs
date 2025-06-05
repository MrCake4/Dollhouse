using NavKeypad;
using UnityEngine;

public class PasswordHandler : MonoBehaviour
{
    int password;   // a pasword variable to store the generated password
    [SerializeField] Keypad keypad; // reference to the Keypad component
    [SerializeField] NumberRenderer[] numberQuads; // array of NumberRenderer components to display the digits

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Generate a random 3-digit password
        password = Random.Range(100, 1000);
        Debug.Log("Generated Password: " + password);

        keypad.setKeyPadCombo(password); // Set the generated password in the Keypad component
        setNumberQuads(password); // Set the password digits in the NumberRenderer components
    }

    void setNumberQuads(int password)
    {
        string passwordStr = password.ToString();

        // Extract digits
        // int firstDigit = int.Parse(passwordStr[0].ToString());
        for (int i = 0; i < numberQuads.Length; i++)
        {
            numberQuads[i].UpdateUVs(int.Parse(passwordStr[i].ToString()));
        }
    }
}
