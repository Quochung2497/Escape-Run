using System;

namespace Watermelon
{
    public abstract class ValidatorAttribute : ExtendedEditorAttribute
    {
        public abstract string DefaultMessage
        {
            get;
        }

        public virtual ValidateResult Validate(object value, object target)
        {
            return new ValidateResult(ValidateType.Success, DefaultMessage);
        }

        public class ValidateResult
        {
            private ValidateType validateType;
            private string message;

            public ValidateType ValidateType
            {
                get { return validateType; }
            }

            public string Message
            {
                get { return message; }
            }

            public ValidateResult(ValidateType validateType, string message)
            {
                this.validateType = validateType;
                this.message = message;
            }
        }

        [System.Flags]
        public enum ValidateType
        {
            Success = 1 << 0,
            Warning = 1 << 1,
            Error = 1 << 2
        }
    }
}