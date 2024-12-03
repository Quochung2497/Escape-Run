#pragma warning disable 0414
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    using UI.Particle;

    public class UIParticleSystem : MonoBehaviour
    {
        private RectTransform rectTransform;

        [SerializeField] UIParticleSettings settings;

        private RuntimeGenericPool<UIParticle> particlesPool;

        public bool IsPlaying { get; set; }

        private List<UIParticle> particles;
        public bool DisableWhenReady { get; set; }

        private float spawnRate;

        private float lastSpawnTime;
        private List<BurstData> burstsData = new List<BurstData>();

        private bool isInited = false;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            rectTransform = GetComponent<RectTransform>();

            int maxCount = 0;
            if (settings.emissionPerSecond > 0)
            {
                spawnRate = 1f / settings.emissionPerSecond;
                maxCount = Mathf.CeilToInt(settings.emissionPerSecond * settings.lifetime.Max * 1.2f);
            }
            else
            {
                spawnRate = float.PositiveInfinity;
            }

            if (!settings.bursts.IsNullOrEmpty())
            {
                for (int i = 0; i < settings.bursts.Length; i++)
                {
                    var burst = settings.bursts[i];

                    var burstCount = settings.lifetime.Max / burst.interval;
                    if (burst.loopsCount < 0)
                    {
                        maxCount += Mathf.CeilToInt(burstCount) * burst.count;
                    }
                    else
                    {
                        if (burstCount > burst.loopsCount)
                        {
                            maxCount += burst.loopsCount * burst.count;
                        }
                        else
                        {
                            maxCount += Mathf.CeilToInt(burstCount) * burst.count;
                        }
                    }

                    burstsData.Add(new BurstData
                    {
                        isActive = burst.loopsCount != 0,
                        burstSettings = burst,
                        counter = 0,
                        timeToSpawn = Time.time + burst.delay
                    });
                }
            }

            Debug.Log(maxCount);
            if (maxCount == 0) maxCount = 1;

            particlesPool = new RuntimeGenericPool<UIParticle>(settings.uiParticlePrefab, maxCount, rectTransform);

            lastSpawnTime = Time.time + settings.startDelay;

            particles = new List<UIParticle>();

            isInited = true;
        }

        private void OnEnable()
        {
            if (settings.playOnAwake)
            {
                IsPlaying = true;
                lastSpawnTime = Time.time + settings.startDelay;
                DisableWhenReady = false;
            }
        }

        private void LateUpdate()
        {
            // Tick through all active particles and remove finished ones
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

            //Particle System finished playing
            if (DisableWhenReady && particles.Count == 0) gameObject.SetActive(false);

            if (!IsPlaying)
            {
                lastSpawnTime = Time.time;

                return;
            }

            // Emitting new Particles
            var timeSpend = Time.time - lastSpawnTime;

            if (settings.emissionPerSecond > 0 && timeSpend >= spawnRate)
            {
                do
                {
                    timeSpend -= spawnRate;

                    // SpawnParticle;

                    SpawnParticle(timeSpend);

                } while (timeSpend >= spawnRate);

                lastSpawnTime = Time.time - timeSpend;
            }

            for (int i = 0; i < burstsData.Count; i++)
            {
                var burst = burstsData[i];
                if (!burst.isActive) return;

                if (Time.time >= burst.timeToSpawn)
                {
                    timeSpend = Time.time - burst.timeToSpawn;

                    for (int j = 0; j < burst.burstSettings.count; j++)
                    {
                        SpawnParticle(timeSpend);
                    }

                    burst.counter++;

                    if (burst.counter >= burst.burstSettings.loopsCount && burst.burstSettings.loopsCount >= 0)
                    {
                        burst.isActive = false;
                    }
                    else
                    {
                        burst.timeToSpawn += burst.burstSettings.interval;
                    }

                }
            }
        }

        private void SpawnParticle(float timeSpend)
        {
            var particle = particlesPool.GetComponent();

            var spawnPos = Vector2.zero;

            switch (settings.shape)
            {
                case UIParticleSettings.Shape.Circle:
                    spawnPos = Random.insideUnitCircle * settings.circleRadius;
                    break;
                case UIParticleSettings.Shape.Rect:
                    var halfSize = settings.rectSize / 2f;
                    spawnPos = new Vector2(Random.Range(-halfSize.x, halfSize.x), Random.Range(-halfSize.y, halfSize.y));
                    break;
            }

            particle.Init(settings, timeSpend, spawnPos, Vector2.up);

            particles.Add(particle);
        }
    }

    public class BurstData
    {
        public bool isActive;
        public UIParticleSettings.BurstSettings burstSettings;
        public int counter;
        public float timeToSpawn;
    }

    public class RuntimeGenericPool<T> where T : MonoBehaviour
    {
        private List<T> pooledComponents;

        private GameObject prefab;
        private Transform parent;

        public RuntimeGenericPool(GameObject prefab, int maxCount, Transform parent)
        {
            pooledComponents = new List<T>();

            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < maxCount; i++)
            {
                InstantiateComponent(prefab, parent);
            }
        }

        public T GetComponent()
        {
            for (int i = 0; i < pooledComponents.Count; i++)
            {
                var component = pooledComponents[i];
                if (!component.gameObject.activeSelf)
                {
                    component.gameObject.SetActive(true);
                    return component;
                }
            }

            return InstantiateComponent(prefab, parent, false);
        }

        private T InstantiateComponent(GameObject prefab, Transform parent, bool reset = true)
        {
            var component = Object.Instantiate(prefab, parent).GetComponent<T>();

            if (reset)
            {
                component.gameObject.SetActive(false);
            }

            pooledComponents.Add(component);

            return component;

        }

    }

}