using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Light References")]
    public Light spotlightAbovePlayer;
    [SerializeField] private Light[] importantLights; // Lights whose state should persist
    private bool[] importantLightStates;              // Stores their previous states

    [SerializeField] Light[] excludedLights; // Lights that should not be affected by the game over state

    private Light[] allLights;
    private Light[] otherLights;

    void Start()
    {
        // Cache all lights in the scene
        allLights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        otherLights = System.Array.FindAll(allLights, light => light != spotlightAbovePlayer);

        // find Light with tag "deadLight"
        spotlightAbovePlayer = GameObject.FindGameObjectWithTag("DeadLight").GetComponent<Light>();

        // Initialize the state array
        importantLightStates = new bool[importantLights.Length];

        // Save initial states
        SaveImportantLightStates();
    }

    public void SetLightStates(bool enableOthers)
    {
        foreach (Light light in otherLights)
        {
            // if light is in excludedLights, skip it
            if (System.Array.Exists(excludedLights, excludedLight => excludedLight == light)) continue;
            light.enabled = enableOthers;
        }

        if (spotlightAbovePlayer != null)
        {
            spotlightAbovePlayer.enabled = !enableOthers;
        }
    }

    public void SaveImportantLightStates()
    {
        for (int i = 0; i < importantLights.Length; i++)
        {
            if (importantLights[i] != null)
                importantLightStates[i] = importantLights[i].enabled;
        }
    }

    public void RestoreImportantLightStates()
    {
        for (int i = 0; i < importantLights.Length; i++)
        {
            if (importantLights[i] != null)
                importantLights[i].enabled = importantLightStates[i];
        }
    }
}
