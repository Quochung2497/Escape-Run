using UnityEngine;
using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ComponentAttribute : ValidatorAttribute
    {
        private Type type;
        private string message;

        public ComponentAttribute(Type type, string message = null)
        {
            this.type = type;
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
            get { return "GameObject must contains " + type.Name + " component!"; }
        }

        public Type RequiredType
        {
            get
            {
                return this.type;
            }
        }

        public override ValidateResult Validate(object value, object target)
        {
#if UNITY_EDITOR
            if (!type.IsSubclassOf(typeof(Component)))
            {
                return new ValidateResult(ValidateType.Warning, "Wrong component type!");
            }

            if (value != null && !value.Equals(null))
            {
                Type targetType = value.GetType();

                if (targetType == typeof(GameObject))
                {
                    GameObject valueObject = (GameObject)value;

                    if (valueObject.GetComponent(type) == null)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            return new ValidateResult(ValidateType.Error, message);
                        }
                        else
                        {
                            return new ValidateResult(ValidateType.Error, "GameObject must contains " + type.Name + " component!");
                        }
                    }
                }
                else
                {
                    return new ValidateResult(ValidateType.Error, "Wrong field type!");
                }

                return new ValidateResult(ValidateType.Success, DefaultMessage);
            }
#endif

            return new ValidateResult(ValidateType.Error, "Value can't be null!");
        }
    }
}
