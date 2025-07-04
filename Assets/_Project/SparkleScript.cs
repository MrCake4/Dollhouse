using UnityEngine;

public class SparkleTrigger : MonoBehaviour
{
    public ParticleSystem sparkleEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sparkleEffect.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sparkleEffect.Stop();
        }
    }
}
