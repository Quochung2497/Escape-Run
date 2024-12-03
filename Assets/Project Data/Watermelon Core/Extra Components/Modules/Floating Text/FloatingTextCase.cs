using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class FloatingTextCase
    {
        [SerializeField] string name;
        public string Name => name;

        [SerializeField] FloatingTextBaseBehaviour floatingTextBehaviour;
        public FloatingTextBaseBehaviour FloatingTextBehaviour => floatingTextBehaviour;

        private Pool floatingTextPool;
        public Pool FloatingTextPool => floatingTextPool;

        public void Initialise()
        {
            floatingTextPool = new Pool(new PoolSettings(floatingTextBehaviour.name, floatingTextBehaviour.gameObject, 1, true));
        }
    }
}