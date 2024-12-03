using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;

namespace Watermelon
{

    public partial class ParticlesController : MonoBehaviour
    {
        private static ParticlesController particlesController;

        [SerializeField] Particle[] particles;
        [SerializeField] RingEffectController ringEffectController;

        private static Dictionary<int, Particle> registerParticles = new Dictionary<int, Particle>();

        private static List<ParticleCase> activeParticles = new List<ParticleCase>();
        private static int activeParticlesCount = 0;

        public static RingEffectController RingEffectController => particlesController.ringEffectController;

        public void Initialise()
        {
            particlesController = this;

            // Register particles
            for (int i = 0; i < particles.Length; i++)
            {
                RegisterParticle(particles[i]);
            }

            ringEffectController.Initialise();

            StartCoroutine(CheckForActiveParticles());
        }

        public static void ClearParticles()
        {
            for (int i = activeParticlesCount - 1; i >= 0; i--)
            {
                activeParticles[i].OnDisable();

                activeParticles.RemoveAt(i);
                activeParticlesCount--;
            }
        }

        private IEnumerator CheckForActiveParticles()
        {
            while (true)
            {
                yield return null;
                yield return null;
                yield return null;
                yield return null;
                yield return null;

                for (int i = activeParticlesCount - 1; i >= 0; i--)
                {
                    if (activeParticles[i] != null)
                    {
                        if (activeParticles[i].IsForceDisabledRequired())
                            activeParticles[i].ParticleSystem.Stop();

                        if (!activeParticles[i].ParticleSystem.IsAlive())
                        {
                            activeParticles[i].OnDisable();

                            activeParticles.RemoveAt(i);
                            activeParticlesCount--;
                        }
                    }
                    else
                    {
                        activeParticles.RemoveAt(i);
                        activeParticlesCount--;
                    }
                }
            }
        }

        public static ParticleCase ActivateParticle(Particle particle)
        {
            ParticleCase particleCase = new ParticleCase(particle);

            activeParticles.Add(particleCase);
            activeParticlesCount++;

            return particleCase;
        }

        #region Register
        public static int RegisterParticle(Particle particle)
        {
            int particleHash = particle.ParticleName.GetHashCode();
            if (!registerParticles.ContainsKey(particleHash))
            {
                particle.Initialise();

                registerParticles.Add(particle.ParticleName.GetHashCode(), particle);

                //Debug.Log(string.Format("[Particle Controller]: Particle with name {0} registered!", particle.ParticleName));

                return particleHash;
            }
            else
            {
                Debug.LogError(string.Format("[Particle Controller]: Particle with name {0} already register!"));
            }

            return -1;
        }

        public static int RegisterParticle(string particleName, GameObject particlePrefab)
        {
            return RegisterParticle(new Particle(particleName, particlePrefab));
        }
        #endregion

        #region Play
        public static ParticleCase PlayParticle(string particleName)
        {
            int particleHash = particleName.GetHashCode();

            if (registerParticles.ContainsKey(particleHash))
            {
                return ActivateParticle(registerParticles[particleHash]);
            }

            Debug.LogError(string.Format("[Particles System]: Particle with type {0} is missing!", particleName));

            return null;
        }

        public static ParticleCase PlayParticle(int particleHash)
        {
            if (registerParticles.ContainsKey(particleHash))
            {
                return ActivateParticle(registerParticles[particleHash]);
            }

            Debug.LogError(string.Format("[Particles System]: Particle with hash {0} is missing!", particleHash));

            return null;
        }
        #endregion

        public static int GetHash(string particleName)
        {
            return particleName.GetHashCode();
        }
    }

    [System.Serializable]
    public class RingEffectController
    {
        [SerializeField] bool isActive;

        [SerializeField] GameObject ringEffectPrefab;
        [SerializeField] Gradient defaultGradient;

        private Pool ringEffectPool;

        public void Initialise()
        {
            if (!isActive)
                return;

            ringEffectPool = new Pool(new PoolSettings(ringEffectPrefab.name, ringEffectPrefab, 1, true));
        }

        public RingEffectCase SpawnEffect(Vector3 position, float targetSize, float time, Ease.Type easing)
        {
            return SpawnEffect(position, defaultGradient, targetSize, time, easing);
        }

        public RingEffectCase SpawnEffect(Vector3 position, Gradient gradient, float targetSize, float time, Ease.Type easing)
        {
            GameObject ringObject = ringEffectPool.GetPooledObject();
            ringObject.transform.position = position;
            ringObject.transform.localScale = Vector3.zero;
            ringObject.SetActive(true);

            RingEffectCase ringEffectCase = new RingEffectCase(ringObject, targetSize, gradient);

            ringEffectCase.SetTime(time);
            ringEffectCase.SetEasing(easing);
            ringEffectCase.StartTween();

            return ringEffectCase;
        }
    }

    public class RingEffectCase : TweenCase
    {
        private static readonly int SHADER_SCALE_PROPERTY = Shader.PropertyToID("_Scale");
        private static readonly int SHADER_COLOR_PROPERTY = Shader.PropertyToID("_Color");

        private GameObject ringGameObject;
        private MeshRenderer ringMeshRenderer;

        private MaterialPropertyBlock materialPropertyBlock;

        private float targetSize;
        private Gradient targetGradient;

        public RingEffectCase(GameObject gameObject, float targetSize, Gradient targetGradient)
        {
            ringGameObject = gameObject;
            ringMeshRenderer = ringGameObject.GetComponent<MeshRenderer>();

            this.targetGradient = targetGradient;
            this.targetSize = targetSize;

            materialPropertyBlock = new MaterialPropertyBlock();

            ringMeshRenderer.GetPropertyBlock(materialPropertyBlock);
            //materialPropertyBlock.SetFloat(SHADER_SCALE_PROPERTY, 0.1f);
            materialPropertyBlock.SetColor(SHADER_COLOR_PROPERTY, targetGradient.Evaluate(0.0f));
            ringMeshRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        public override void DefaultComplete()
        {
            ringGameObject.transform.localScale = targetSize.ToVector3();

            ringMeshRenderer.GetPropertyBlock(materialPropertyBlock);
            //materialPropertyBlock.SetFloat(SHADER_SCALE_PROPERTY, targetSize);
            materialPropertyBlock.SetColor(SHADER_COLOR_PROPERTY, targetGradient.Evaluate(1.0f));
            ringMeshRenderer.SetPropertyBlock(materialPropertyBlock);

            ringGameObject.SetActive(false);
        }

        public override void Invoke(float deltaTime)
        {
            float interpolatedState = Interpolate(state);

            ringMeshRenderer.GetPropertyBlock(materialPropertyBlock);
            //materialPropertyBlock.SetFloat(SHADER_SCALE_PROPERTY, Mathf.LerpUnclamped(0.1f, targetSize, interpolatedState));
            materialPropertyBlock.SetColor(SHADER_COLOR_PROPERTY, targetGradient.Evaluate(interpolatedState));
            ringMeshRenderer.SetPropertyBlock(materialPropertyBlock);

            ringGameObject.transform.localScale = Vector3.one * Mathf.LerpUnclamped(0.1f, targetSize, interpolatedState);
        }

        public override bool Validate()
        {
            return true;
        }
    }
}
