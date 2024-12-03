using UnityEngine;
using UnityEditor;

namespace Watermelon
{
    [PropertyValidator(typeof(GameObjectTagAttribute))]
    public class GameObjectPropertyValidator : PropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            GameObjectTagAttribute requiredAttribute = PropertyUtility.GetAttribute<GameObjectTagAttribute>(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox(property.name + " can't be null!", MessageType.Error);
                }
                else
                {
                    GameObject go = (GameObject)property.objectReferenceValue;
                    if (!go.CompareTag(requiredAttribute.Tag))
                    {
                        string errorMessage = requiredAttribute.DefaultMessage;
                        if (!string.IsNullOrEmpty(requiredAttribute.Message))
                        {
                            errorMessage = requiredAttribute.Message;
                        }

                        EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    }
                }
            }
            else
            {
                string warning = requiredAttribute.GetType().Name + " works only on reference types";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
            }
        }
    }
}