using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    using Watermelon.UI.Particle;

    public class UIParticleTrail : MonoBehaviour
    {
        [SerializeField] UIParticleSettings settings;
        [SerializeField] RectTransform targetRect;

        public Vector2 AnchoredPos { get => targetRect.anchoredPosition; set => targetRect.anchoredPosition = value; }

        private PoolGeneric<UIParticle> particlePool;

        public bool IsPlaying { get; set; }

        public bool DisableWhenReady { get; set; }

        private float spawnRate;

        private float lastSpawnTime;

        public Vector3 NormalizedVelocity { get; set; }

        private List<UIParticle> particles = new List<UIParticle>();

        private void Awake()
        {
            spawnRate = 1f / settings.emissionPerSecond;

            lastSpawnTime = Time.time;
        }

        public void SetPool(PoolGeneric<UIParticle> particlesPool)
        {
            particlePool = particlesPool;
        }

        public void Init()
        {
            DisableWhenReady = false;
            lastSpawnTime = Time.time;
        }

        private void LateUpdate()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                var particle = particles[i];

                if (particle.Tick())
                {
                    particles.RemoveAt(i);
                    i--;

                    particle.IsActive = false;

                    continue;
                }
            }

            if (DisableWhenReady && particles.Count == 0) gameObject.SetActive(false);

            if (!IsPlaying)
            {
                lastSpawnTime = Time.time;

                return;
            }

            var timeSpend = Time.time - lastSpawnTime;

            if (timeSpend >= spawnRate)
            {
                do
                {
                    timeSpend -= spawnRate;

                    // SpawnParticle;

                    var particle = particlePool.GetPooledComponent();

                    particle.Init(settings, timeSpend, targetRect.anchoredPosition, NormalizedVelocity);

                    particles.Add(particle);

                } while (timeSpend >= spawnRate);

                lastSpawnTime = Time.time - timeSpend;
            }
        }

    }
}