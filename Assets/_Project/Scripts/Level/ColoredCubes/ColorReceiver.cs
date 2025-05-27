using UnityEngine;

public class ColorReceiver : MonoBehaviour
{
    private bool hitRed;
    private bool hitGreen;
    private bool hitBlue;

    private Renderer rend;
    private Color currentColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        currentColor = rend.material.color;
    }

    public void RegisterSpotlight(GameObject spotlight)
    {
        // Setze anhand des Namens, welches Spotlight getroffen hat
        bool changed = false;

        if (spotlight.name == "RedSpotlight" && !hitRed)
        {
            hitRed = true;
            changed = true;
        }
        else if (spotlight.name == "GreenSpotlight" && !hitGreen)
        {
            hitGreen = true;
            changed = true;
        }
        else if (spotlight.name == "BlueSpotlight" && !hitBlue)
        {
            hitBlue = true;
            changed = true;
        }

        if (changed)
        {
            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        currentColor = new Color(
            hitRed ? 1f : 0f,
            hitGreen ? 1f : 0f,
            hitBlue ? 1f : 0f
        );

        rend.material.color = currentColor;
    }
}
