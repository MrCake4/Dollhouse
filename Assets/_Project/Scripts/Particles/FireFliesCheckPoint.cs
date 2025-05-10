using System;
using UnityEngine;

public class FireFlies : MonoBehaviour
{
    [SerializeField] ParticleSystem psFlies;
    Boolean touched = false;
    

    void OnTriggerEnter(Collider other)
    {
        if (!touched && other.CompareTag("Player"))
        {
            Debug.Log("TRIGGERED");

            var col = psFlies.colorOverLifetime;    // creates a copy of the particle system
            col.enabled = true;                     // checks if the colorOverLifetime is enabled
            col.color = new ParticleSystem.MinMaxGradient(new Color32(0, 0, 255, 255));     // TODO: This is just a demonstration color and needs to be changed later
            
            // TODO: ADD A CHECKPOINT MECHANIC HERE
            // ...

            touched = true;
        }
    }
}
