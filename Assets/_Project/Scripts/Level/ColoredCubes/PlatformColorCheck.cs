using UnityEngine;

public class PlatformColorCheck : MonoBehaviour
{
    [Header("Farbvergleich")]
    public Color targetColor = Color.red; // gewünschte Ziel-Farbe der Plattform
    [Range(0f, 0.1f)]
    public float tolerance = 0.01f; // erlaubt minimale Abweichung

    [Header("Status")]
    public bool isColorMatch = false;

    private bool lastMatch = false; // zum Vergleichen alter Zustand

    private Renderer rend;

    bool solved = false;


    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = targetColor; // Setze die Plattformfarbe visuell
        }
    }

    private void OnTriggerStay(Collider other)
    {
        ColorReceiver receiver = other.GetComponent<ColorReceiver>();
        if (receiver != null)
        {
            Color objectColor = receiver.GetCurrentColor();

            //============================= Debug only ===============================
            bool currentMatch = ColorsAreEqual(objectColor, targetColor, tolerance);

            // Nur loggen, wenn sich der Match-Status ändert
            if (currentMatch != lastMatch)
            {
                if (currentMatch)
                {
                    Debug.Log("🎉 Juhuu, richtige Farbe!");
                    solved = true;
                }
                else
                {
                    Debug.Log("💢 Oh no, falsche Farbe!");
                }
                lastMatch = currentMatch;
            }
            //============================= Debug only ===============================

            isColorMatch = ColorsAreEqual(objectColor, targetColor, tolerance);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Sobald das Objekt die Plattform verlässt, ist kein Match mehr aktiv
        if (other.GetComponent<ColorReceiver>())
        {
            isColorMatch = false;

            //============================= Debug only ===============================
            if (lastMatch) // Falls vorher korrekt war
            {
                Debug.Log("⬅️ Würfel hat Plattform verlassen.");
            }

            lastMatch = false;
            //============================= Debug only ===============================

        }
    }

    private bool ColorsAreEqual(Color a, Color b, float tolerance)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }

    public bool getSolved => solved; // Getter für den aktuellen Farbstatus
}