using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [PropertyValidator(typeof(ValidateInputAttribute))]
    public class ValidateInputPropertyValidator : PropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            ValidateInputAttribute validateInputAttribute = PropertyUtility.GetAttribute<ValidateInputAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            MethodInfo validationCallback = target.GetType().GetMethod(validateInputAttribute.CallbackName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (validationCallback != null &&
                validationCallback.ReturnType == typeof(ValidatorAttribute.ValidateResult) &&
                validationCallback.GetParameters().Length == 1)
            {
                FieldInfo fieldInfo = target.GetType().GetField(property.name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                Type fieldType = fieldInfo.FieldType;
                Type parameterType = validationCallback.GetParameters()[0].ParameterType;

                if (fieldType == parameterType)
                {
                    ValidatorAttribute.ValidateResult result = (ValidatorAttribute.ValidateResult)validationCallback.Invoke(target, new object[] { fieldInfo.GetValue(target) });
                    if (result != null && result.ValidateType != ValidatorAttribute.ValidateType.Success)
                    {
                        MessageType messageType = MessageType.Error;

                        if (result.ValidateType == ValidatorAttribute.ValidateType.Warning)
                            messageType = MessageType.Warning;

                        EditorGUILayout.HelpBox(result.Message, messageType);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("The field type is not the same as the callback's parameter type", MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.HelpBox(validateInputAttribute.GetType().Name + " needs a callback with boolean return type and a single parameter of the same type as the field", MessageType.Warning);
            }
        }
    }
}
