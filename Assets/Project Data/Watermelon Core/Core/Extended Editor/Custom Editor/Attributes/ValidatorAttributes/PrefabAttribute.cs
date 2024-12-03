using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class PrefabAttribute : ValidatorAttribute
    {
        private string message;

        public PrefabAttribute(string message = null)
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
            get { return "GameObject must be a prefab!"; }
        }

        public override ValidateResult Validate(object value, object target)
        {
#if UNITY_EDITOR
            if (value != null && !value.Equals(null))
            {
                Type targetType = value.GetType();

                if (targetType.IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    UnityEngine.Object valueObject = (UnityEngine.Object)value;

                    if (UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(valueObject) == null && UnityEditor.PrefabUtility.GetPrefabInstanceHandle(valueObject) == null)
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            return new ValidateResult(ValidateType.Error, message);
                        }
                        else
                        {
                            return new ValidateResult(ValidateType.Error, DefaultMessage);
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
