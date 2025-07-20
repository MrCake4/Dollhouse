using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadManager : MonoBehaviour
{
    public static GamepadManager instance;

        private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Optional: prevent duplicates
        }
    }

    public void rumbleController(float lowFrequency, float highFrequency, float duration)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
            StartCoroutine(StopRumbleAfterSeconds(duration));
        }
    }

    private IEnumerator StopRumbleAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Gamepad.current?.SetMotorSpeeds(0f, 0f);
    }
}
