using UnityEngine;

public class SpotlightRaycaster : MonoBehaviour
{
    public Light spotlight;
    public float rayLength = 100f;

    public enum ColorChannel { Red, Green, Blue }
    public ColorChannel colorChannel = ColorChannel.Red; // Auswahl im Inspector

    void Start()
    {
        if (spotlight == null)
            spotlight = GetComponent<Light>();
    }

    void Update()
    {
        LightRayCast(); // Immer aktiv, unabhängig vom Spotlight.enabled
    }

    void LightRayCast()
    {
        //if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength))
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength, ~0, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(transform.position, hit.point, Color.yellow);

            ColorReceiver receiver = hit.collider.GetComponent<ColorReceiver>();
            if (receiver != null)
            {
                float value = spotlight.enabled ? 1f : 0f;
                receiver.SetChannelValue(colorChannel, value);
            }
        }
    }
}
