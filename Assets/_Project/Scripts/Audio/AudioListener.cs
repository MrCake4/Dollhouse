using UnityEngine;


// This Script just makes the Audio Listener always face the global forward direction.

[RequireComponent(typeof(AudioListener))]
public class AudioListener : MonoBehaviour
{
    void Update()
    {
        // rotation always global forward
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

    }
}
