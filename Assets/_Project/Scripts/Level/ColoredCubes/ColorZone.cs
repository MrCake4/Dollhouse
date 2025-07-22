using UnityEngine;
using System.Collections.Generic;

public class ColorZone : MonoBehaviour
{
    [System.Serializable]
    public class LampenInfo
    {
        public Light spotlight;
        public SpotlightRaycaster.ColorChannel colorChannel;
    }

    [Header("Zugehörige Lampen")]
    public List<LampenInfo> lampen = new List<LampenInfo>();

    private float r, g, b;

    private void Update()
    {
        // Werte von allen Lampen sammeln
        r = g = b = 0f;

        foreach (var lampe in lampen)
        {
            if (lampe.spotlight == null) continue;

            float value = lampe.spotlight.enabled ? 1f : 0f;

            switch (lampe.colorChannel)
            {
                case SpotlightRaycaster.ColorChannel.Red:
                    r = value;
                    break;
                case SpotlightRaycaster.ColorChannel.Green:
                    g = value;
                    break;
                case SpotlightRaycaster.ColorChannel.Blue:
                    b = value;
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        ColorReceiver receiver = other.GetComponent<ColorReceiver>();
        if (receiver != null)
        {
            receiver.SetChannelValue(SpotlightRaycaster.ColorChannel.Red, r);
            receiver.SetChannelValue(SpotlightRaycaster.ColorChannel.Green, g);
            receiver.SetChannelValue(SpotlightRaycaster.ColorChannel.Blue, b);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerStay(other); // Sofort anwenden
    }
}
