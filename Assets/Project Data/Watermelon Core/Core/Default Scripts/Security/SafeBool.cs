using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public struct SafeBool
    {
        private int value;
        private int trueValue;

        public bool Value
        {
            get
            {
                if (value == trueValue)
                    return true;

                return false;
            }
        }

        public SafeBool(bool value)
        {
            trueValue = Random.Range(-9999, 9999);

            if(value)
            {
                this.value = trueValue;
            }
            else
            {
                this.value = Random.Range(-9999, 9999);
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
