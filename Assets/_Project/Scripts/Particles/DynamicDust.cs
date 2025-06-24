using UnityEngine;

public class DynamicDust : MonoBehaviour
{
    /*
    *   Inspired by the Dust Particles used in little nightmares 2
    *   This script modifies the velocity of particles in a ParticleSystem
    */

    [Header("Dynamic Dust Settings")]
    public ParticleSystem particleSystem;
    public Transform player;
    [Tooltip("The radius within which particles are repelled from the player"), Range(0f, 2f)]
    public float repelRadius = 2f;  // radius within which particles are repelled
    [Tooltip("The force applied to repel particles from the player"), Range(0f, 2f)]
    public float repelForce = 1f;   // force applied to repel particles

    private ParticleSystem.Particle[] particles;        // stores particles in an array

    // LateUpdate is called after all Update functions have been called
    // This is where we will modify the particles' velocities

    void Awake()
    {
        player = FindFirstObjectByType<PlayerStateManager>().transform;
    }

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