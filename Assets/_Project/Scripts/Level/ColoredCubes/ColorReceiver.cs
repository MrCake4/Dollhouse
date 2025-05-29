using UnityEngine;
using static SpotlightRaycaster; // Damit du ColorChannel enum hier auch nutzen kannst

public class ColorReceiver : MonoBehaviour
{
    private float r, g, b;
    private bool colorChanged = false;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = Color.black;
    }

    public void SetChannelValue(ColorChannel channel, float value)
    {
        value = Mathf.Clamp01(value); // sicherstellen, dass Wert zwischen 0–1 ist

        switch (channel)
        {
            case ColorChannel.Red:
                if (!Mathf.Approximately(r, value))
                {
                    r = value;
                    colorChanged = true;
                }
                break;
            case ColorChannel.Green:
                if (!Mathf.Approximately(g, value))
                {
                    g = value;
                    colorChanged = true;
                }
                break;
            case ColorChannel.Blue:
                if (!Mathf.Approximately(b, value))
                {
                    b = value;
                    colorChanged = true;
                }
                break;
        }
    }

    void LateUpdate()
    {
        if (colorChanged)
        {
            Color newColor = new Color(r, g, b);
            rend.material.color = newColor;

            Debug.Log($"{gameObject.name} Farbe geändert zu: {ColorToString(newColor)}");

            colorChanged = false;
        }
    }

    private string ColorToString(Color color)
    {
        return $"R:{color.r:F1} G:{color.g:F1} B:{color.b:F1}";
    }

    public Color GetCurrentColor()
    {
        return new Color(r, g, b);
    }

}
