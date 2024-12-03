using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public struct SafeFloat
    {
        private float value;
        private float offset;

        public float Value
        {
            get { return value + offset; }
        }

        public SafeFloat(float value)
        {
            offset = Random.Range(-9999, 9999);

            this.value = value - offset;
        }

        public static SafeFloat operator +(SafeFloat f1, SafeFloat f2)
        {
            return new SafeFloat(f1.Value + f2.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
