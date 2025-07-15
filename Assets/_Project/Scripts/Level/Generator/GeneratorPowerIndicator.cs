using UnityEngine;

public class GeneratorPowerIndicator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Generator generator;             // Reference to your Generator script
    [SerializeField] private MeshRenderer[] bulbMeshes;       // 4 bulbs in order (from first to last)

    [Header("Materials")]
    [SerializeField] private Material glowingMaterial;
    [SerializeField] private Material offMaterial;
    [SerializeField] Light roomLight; // Optional: Reference to a room light to control its color

    void Awake()
    {
        if (generator == null)
            generator = GetComponent<Generator>();
    }

    private void Update()
    {
        if (generator == null || bulbMeshes == null || bulbMeshes.Length != 4)
            return;

        float powerPercent = Mathf.Clamp01(generator.GetCurrentPower() / generator.GetMaxPower());

        for (int i = 0; i < bulbMeshes.Length; i++)
        {
            bool shouldGlow = false;

            if (i == 0 && powerPercent > 0f)
                shouldGlow = true;
            else if (i == 1 && powerPercent >= 0.25f)
                shouldGlow = true;
            else if (i == 2 && powerPercent >= 0.5f)
                shouldGlow = true;
            else if (i == 3 && powerPercent >= 0.75f)
                shouldGlow = true;

            if (bulbMeshes[i] != null)
                bulbMeshes[i].material = shouldGlow ? glowingMaterial : offMaterial;
        }

        if (roomLight != null)
        {
            // Change the room light color based on the generator's power level
            if (powerPercent > 0f)
            {
                roomLight.color = Color.Lerp(Color.red, Color.white, 3f); // Smooth transition from red to white
            }
            else
            {
                roomLight.color = Color.red;
            }
        }
    }
}