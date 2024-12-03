using System;
using System.Reflection;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ValidateInputAttribute : ValidatorAttribute
    {
        private string callbackName;

        public ValidateInputAttribute(string callbackName)
        {
            this.callbackName = callbackName;
        }

        public override string DefaultMessage
        {
            get { return "Custom validate function"; }
        }

        public string CallbackName
        {
            get
            {
                return this.callbackName;
            }
        }

        public override ValidateResult Validate(object value, object target)
        {
            MethodInfo validationCallback = target.GetType().GetMethod(CallbackName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (validationCallback != null && validationCallback.ReturnType == typeof(ValidateResult) && validationCallback.GetParameters().Length == 1)
            {
                ValidateResult message = (ValidateResult)validationCallback.Invoke(target, new object[] { value });
                if (message != null)
                {
                    return message;
                }
            }
            else
            {
                return new ValidateResult(ValidateType.Warning, GetType().Name + " needs a callback with boolean return type and a single parameter of the same type as the field");
            }

            return new ValidateResult(ValidateType.Success, DefaultMessage);
        }
    }
}