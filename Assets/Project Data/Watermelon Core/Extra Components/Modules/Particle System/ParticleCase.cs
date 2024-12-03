using UnityEngine;

namespace Watermelon
{
    public class ParticleCase
    {
        private ParticleSystem particleSystem;
        public ParticleSystem ParticleSystem => particleSystem;

        private float disableTime = -1;

        private bool forceDisable;

        private Particle particle;
        private ParticleBehaviour particleBehaviour;

        public ParticleCase(Particle particle)
        {
            this.particle = particle;

            // Create object
            GameObject particleObject = particle.ParticlePool.GetPooledObject();
            particleObject.SetActive(true);

            // Get particle component
            particleSystem = particleObject.GetComponent<ParticleSystem>();
            particleSystem.Play();

            if (particle.SpecialBehaviour)
            {
                particleBehaviour = particleObject.GetComponent<ParticleBehaviour>();
                particleBehaviour.OnParticleActivated();
            }
        }

        public ParticleCase SetPosition(Vector3 position)
        {
            particleSystem.transform.position = position;

            return this;
        }

        public ParticleCase SetScale(Vector3 scale)
        {
            particleSystem.transform.localScale = scale;

            return this;
        }

        public ParticleCase SetRotation(Quaternion rotation)
        {
            particleSystem.transform.localRotation = rotation;

            return this;
        }

        public ParticleCase SetDuration(float duration)
        {
            disableTime = Time.time + duration;

            return this;
        }

        public ParticleCase SetTarget(Transform followTarget, Vector3 localPosition)
        {
            particleSystem.transform.SetParent(followTarget);
            particleSystem.transform.localPosition = localPosition;

            return this;
        }

        public void OnDisable()
        {
            particleSystem.transform.SetParent(null);
            particleSystem.Stop();

            particleSystem.gameObject.SetActive(false);

            if (particle.SpecialBehaviour)
                particleBehaviour.OnParticleDisabled();
        }

        public void ForceDisable()
        {
            forceDisable = true;
            particleSystem.transform.SetParent(null);

            particleSystem.Stop();
        }

        public bool IsForceDisabledRequired()
        {
            if (forceDisable)
                return true;

            if (disableTime != -1 && Time.time > disableTime)
                return true;

            return false;
        }
    }
}