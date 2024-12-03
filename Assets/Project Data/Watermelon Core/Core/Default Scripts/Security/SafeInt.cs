using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public struct SafeInt
    {
        private int value;
        private int offset;

        public int Value
        {
            get { return value + offset; }
        }

        public SafeInt(int value)
        {
            offset = Random.Range(-9999, 9999);

            this.value = value - offset;
        }

        public static SafeInt operator +(SafeInt f1, SafeInt f2)
        {
            return new SafeInt(f1.Value + f2.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}