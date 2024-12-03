using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RequiredAttribute : ValidatorAttribute
    {
        private string message;

        public RequiredAttribute(string message = null)
        {
            this.message = message;
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public override string DefaultMessage
        {
            get { return "Value can't be null!"; }
        }

        public override ValidateResult Validate(object value, object target)
        {
            if (value != null && !value.Equals(null))
            {
                Type targetType = value.GetType();

                if (targetType == typeof(string))
                {
                    if (string.IsNullOrEmpty((string)value))
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            return new ValidateResult(ValidateType.Error, message);
                        }
                        else
                        {
                            return new ValidateResult(ValidateType.Error, "Value can't be empty!");
                        }
                    }
                }

                return new ValidateResult(ValidateType.Success, DefaultMessage);
            }

            if (!string.IsNullOrEmpty(message))
            {
                return new ValidateResult(ValidateType.Error, message);
            }

            return new ValidateResult(ValidateType.Error, "Value can't be null!");
        }
    }
}
