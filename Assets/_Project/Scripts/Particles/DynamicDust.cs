using UnityEngine;

public class DynamicDust : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Transform player;
    public float repelRadius = 2f;
    public float repelForce = 1f;

    private ParticleSystem.Particle[] particles;

    void LateUpdate()
    {
        if (particles == null || particles.Length < particleSystem.main.maxParticles)
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];

        int aliveParticles = particleSystem.GetParticles(particles);

        for (int i = 0; i < aliveParticles; i++)
        {
            Vector3 toPlayer = particles[i].position - player.position;
            float distance = toPlayer.magnitude;

            if (distance < repelRadius)
            {
                Vector3 repelDirection = toPlayer.normalized;
                particles[i].velocity += repelDirection * repelForce * Time.deltaTime;
            }
        }

        particleSystem.SetParticles(particles, aliveParticles);
    }
}