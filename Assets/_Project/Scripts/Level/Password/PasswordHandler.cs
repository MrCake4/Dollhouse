using NavKeypad;
using UnityEngine;

public class PasswordHandler : MonoBehaviour
{
    int password;   // a pasword variable to store the generated password
    [SerializeField] Keypad keypad; // reference to the Keypad component
    [SerializeField] NumberRenderer[] numberQuads; // array of NumberRenderer components to display the digits
    [SerializeField] Candle[] candles; // array of Candle components to interact with

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Generate a random 3-digit password with unique digits
        do
        {
            password = Random.Range(100, 1000);
        } while (!HasUniqueDigits(password));

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

    bool HasUniqueDigits(int number)
    {
        string s = number.ToString();
        return s[0] != s[1] && s[0] != s[2] && s[1] != s[2];
    }
}
