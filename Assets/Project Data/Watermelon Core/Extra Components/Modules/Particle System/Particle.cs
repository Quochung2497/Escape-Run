using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class Particle
    {
        [SerializeField] string particleName;
        public string ParticleName => particleName;

        [SerializeField] GameObject particlePrefab;
        public GameObject ParticlePrefab => particlePrefab;

        [SerializeField] bool specialBehaviour;
        public bool SpecialBehaviour => specialBehaviour;

        private Pool particlePool;
        public Pool ParticlePool => particlePool;

        private bool isInitialised;

        public Particle(string particleName, GameObject particlePrefab)
        {
            this.particleName = particleName;
            this.particlePrefab = particlePrefab;
        }

        public void Initialise()
        {
            if (isInitialised)
                return;

            // Mark as initialised
            isInitialised = true;

            // Create particle pool
            particlePool = new Pool(new PoolSettings(particlePrefab.name, particlePrefab, 0, true));
        }

        public ParticleCase Activate()
        {
            if (isInitialised)
                return ParticlesController.ActivateParticle(this);

            return null;
        }
    }
}