using System;
using UnityEngine;
using UnityEngine.Animations;

public class RoomContainer : MonoBehaviour
{

    [Header("AnkerPoints in Room")]
    public Transform[] windowAnchorPoints;
    public Boolean triggered = false;
    [SerializeField] AIStateManager ai;
    public int windowCount => windowAnchorPoints.Length;

    [Header("Lights in Room")]
    public Light[] roomLights;  // in no lights in array this will be skipped
    float[] baseLightIntensities;
    bool flicker = false;

    // Flicker parameters
    [SerializeField] float flickerSpeed = 8; // how fast the flicker is
    [SerializeField]float flickerAmount = 10f; // intensity variation

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find all lights in the room and store their intensities, later for flickering
        if (roomLights.Length == 0) return;

        // store the base light intensities
        baseLightIntensities = new float[roomLights.Length];
        for (int i = 0; i < roomLights.Length; i++)
        {
            if (roomLights[i] != null)
            {
                baseLightIntensities[i] = Mathf.RoundToInt(roomLights[i].intensity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // when triggered the AI will switch to hunt state
        if (triggered)
        {
            activicateAIHuntState();
            return;
        }

        checkDistanceToWindows();
    }

    // Checks the distance between the AI and the windows in the room
    // If the AI is close to a window, it will flicker the lights
    void checkDistanceToWindows()
    {
        if (roomLights.Length == 0) return;

        bool aiNearWindow = false;
        foreach (Transform window in windowAnchorPoints)
        {
            if (Vector3.Distance(window.position, ai.transform.position) < 5f)
            {
                aiNearWindow = true;
                flicker = true;
                lightFlicker();
                break;
            }
        }

        if (!aiNearWindow && flicker)
        {
            flicker = false;
            resetLights();
        }
    }

    // Activates the AI hunt state and sets the current target room
    void activicateAIHuntState()
        {
            ai.setCurrentTargetRoom(this);
            ai.setLastKnownRoom(this);
            ai.eye.setStartScan(false);
            ai.isPatroling = false;
            ai.switchState(ai.huntState, false);
        }

    // Resets the lights to their base intensities
    void resetLights()
    {
        // Reset the lights to their base intensities
        for (int i = 0; i < roomLights.Length; i++)
        {
            if (roomLights[i] != null)
            {
                roomLights[i].intensity = baseLightIntensities[i];
            }
        }
    }

    // Flickers each light in the roomLight array using Perlin noise for smooth randomness
    void lightFlicker()
    {

        for (int i = 0; i < roomLights.Length; i++)
        {
            if (roomLights[i] != null)
            {
                float baseIntensity = baseLightIntensities[i];

                // Flicker using Perlin noise for smooth randomness
                float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, roomLights[i].transform.position.x);
                roomLights[i].intensity = baseIntensity + (noise - 0.5f) * flickerAmount;
            }
        }
    }
}
