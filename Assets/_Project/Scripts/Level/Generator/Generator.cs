using System;
using UnityEngine;

public class Generator : HitableObject
{

    [SerializeField] Interactable[] powerableObjects;

    [Header("Generator Settings")]
    [SerializeField] float currentPower = 0f;
    [SerializeField] bool charged = false;
    [SerializeField] bool loosesPowerOverTime = true;   // if true, the generator will lose power over time
    [SerializeField] float maxPower = 100f;
    [SerializeField] int lossOverTime = 1;
    [SerializeField] GameObject antenna;

    [Header("Audio")]
    [SerializeField] AudioClip generatorSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (currentPower > 0 && !charged) charged = true;    // checks if the generator has power at the start
    }

    // On Hit turn all powerable objects inside the array on
    public override void onHit()
    {
        currentPower = maxPower;
        charged = true;
        activatePowerableObjects();
    }

    // turn all powerable objects on
    void activatePowerableObjects()
    {
        foreach (Interactable interactable in powerableObjects)
        {
            powerObject(interactable); // turn on all powered objects
        }
    }

    // Update is called once per frame
    void Update()
    {
        updatePower();
        updateAntenna();
    }

    // rotate antenna if the generator is charged, 
    // and flicker the light intensity based on the current power level
    void updateAntenna()
    {
        if (antenna == null) return;

        Light antennaLight = antenna.GetComponentInChildren<Light>();

        if (charged)
        {
            // Rotate antenna visually
            antenna.transform.Rotate(Vector3.up, 100 * Time.deltaTime);

            if (antennaLight != null)
            {
                antennaLight.enabled = true;

                // Flicker speed increases as power decreases
                float normalizedPower = Mathf.Clamp01(currentPower / maxPower);
                float flickerSpeed = Mathf.Lerp(8f, 2f, normalizedPower); // 8 = fast, 2 = slow

                // Flicker intensity (sin wave)
                float baseIntensity = 500f;
                float flickerAmount = Mathf.Sin(Time.time * flickerSpeed) * 300f; // subtle wobble

                antennaLight.intensity = baseIntensity + flickerAmount;
            }
        }
        else
        {
            if (antennaLight != null)
            {
                antennaLight.enabled = false;
            }
        }
    }


    // turn power on for a specific object
    void powerObject(Interactable o)
    {
        if (o != null && charged)
        {
            o.onPowerOn();
        }
    }

    void dePowerObject(Interactable o)
    {
        if (o != null && charged)
        {
            o.onPowerOff();
        }
    }

    // update the power of the generator
    void updatePower()
    {
        if (currentPower > 0 && loosesPowerOverTime && charged)
        {
            currentPower -= lossOverTime * Time.deltaTime; // lose power over time
        }
        else if (currentPower <= 0 && charged)
        {
            Debug.Log("Generator power depleted, turning off all powered objects.");

            foreach (Interactable interactable in powerableObjects)
            {
                dePowerObject(interactable); // turn off all powered objects
                Debug.Log("Deactivated: " + interactable.name);
            }
            charged = false; // if power is depleted, set charged to false
        }
    }
    
    public float GetCurrentPower()
    {
        return currentPower;
    }

public float GetMaxPower()
    {
        return maxPower;
    }
}
