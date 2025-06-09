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
        generatePassword();
        keypad.setKeyPadCombo(password); // Set the generated password in the Keypad component
        setNumberQuads(password); // Set the password digits in the NumberRenderer components
        keypad = GetComponentInChildren<Keypad>(); // Get the Keypad component from the child GameObject
    }

    void generatePassword()
    {
        // Generate a random 3-digit password with unique digits
        do
        {
            password = Random.Range(100, 1000);
        } while (!HasUniqueDigits(password));

        
    }

    void setNumberQuads(int password)
    {
        string passwordStr = password.ToString();

        // Extract digits
        for (int i = 0; i < numberQuads.Length; i++)
        {
            numberQuads[i].UpdateUVs(int.Parse(passwordStr[i].ToString()));
        }
    }

    bool HasUniqueDigits(int number)
    {
        string s = number.ToString();
        return s[0] != s[1] && s[0] != s[2] && s[1] != s[2];
    }
}
