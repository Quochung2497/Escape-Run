using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxValueAttribute : ValidatorAttribute
    {
        private float maxValue;

        public MaxValueAttribute(float maxValue)
        {
            this.maxValue = maxValue;
        }

        public MaxValueAttribute(int maxValue)
        {
            this.maxValue = maxValue;
        }

        public override string DefaultMessage
        {
            get { return "Value must be less than " + maxValue + "!"; }
        }

        public float MaxValue
        {
            get
            {
                return this.maxValue;
            }
        }
    }
}
