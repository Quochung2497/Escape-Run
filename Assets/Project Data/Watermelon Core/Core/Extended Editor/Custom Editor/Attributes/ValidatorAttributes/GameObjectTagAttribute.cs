using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Watermelon
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GameObjectTagAttribute : ValidatorAttribute
    {
        private string tag;
        private string message;

        public GameObjectTagAttribute(string tag, string message = null)
        {
            this.tag = tag;
            this.message = message;
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public string Tag
        {
            get { return tag; }
        }

        public override string DefaultMessage
        {
            get { return "GameObject's tag must be " + tag + "!"; }
        }

        public override ValidateResult Validate(object value, object target)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(tag))
            {
                return new ValidateResult(ValidateType.Warning, "Tag can't be empty!");
            }

            if (value != null && !value.Equals(null))
            {
                Type targetType = value.GetType();

                if (targetType == typeof(GameObject))
                {
                    GameObject valueObject = (GameObject)value;

                    if (!valueObject.CompareTag(tag))
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
