using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinValueAttribute : ValidatorAttribute
    {
        public override string DefaultMessage
        {
            get { return "Value must be greater than " + minValue + "!"; }
        }

        private float minValue;

        public MinValueAttribute(float minValue)
        {
            this.minValue = minValue;
        }

        public MinValueAttribute(int minValue)
        {
            this.minValue = minValue;
        }

        public float MinValue
        {
            get
            {
                return this.minValue;
            }
        }
    }
}
