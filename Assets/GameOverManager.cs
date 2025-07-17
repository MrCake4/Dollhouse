using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public Light spotlightAbovePlayer; // Assign in the Inspector
    private Light[] allLights;
    [SerializeField]private Light[] otherLights;
    [SerializeField] Light[] excludeLights; // Lights to exclude from toggling
    private bool lightsOn = false;

    void Start()
    {
        // Get all Light components in the scene
        allLights = FindObjectsByType<Light>(FindObjectsSortMode.None);

        // Filter out the spotlight
        otherLights = System.Array.FindAll(allLights, light => light != spotlightAbovePlayer);

        // Initial state: spotlight on, others off
    }

    public void SetLightStates(bool enableOthers)
    {
        foreach (Light light in otherLights)
        {
            if (excludeLights != null && System.Array.Exists(excludeLights, l => l == light)) continue; // Skip excluded lights
            light.enabled = enableOthers;
        }

        if (spotlightAbovePlayer != null)
        {
            spotlightAbovePlayer.enabled = !enableOthers;
        }
    }
}