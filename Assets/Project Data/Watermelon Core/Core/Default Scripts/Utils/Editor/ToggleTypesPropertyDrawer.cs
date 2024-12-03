using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [CustomPropertyDrawer(typeof(ToggleType<>), true)]
    public class ToggleTypesPropertyDrawer : UnityEditor.PropertyDrawer
    {
        private static int CHECKMARK_OFFSET = 24;
        private static string ENABLED_PROPERTY_NAME = "enabled";
        private static string NEW_VALUE_PROPERTY_NAME = "newValue";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect workRect = new Rect(position);
            workRect.width = EditorGUIUtility.labelWidth;

            if (property.FindPropertyRelative(ENABLED_PROPERTY_NAME).boolValue)
            {
                EditorGUI.PropertyField(workRect, property.FindPropertyRelative(ENABLED_PROPERTY_NAME), label);

                workRect.xMax = position.xMax;
                workRect.xMin = EditorGUIUtility.labelWidth + position.xMin + CHECKMARK_OFFSET;

                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 0;
                EditorGUI.PropertyField(workRect, property.FindPropertyRelative(NEW_VALUE_PROPERTY_NAME), GUIContent.none);
                EditorGUIUtility.labelWidth = labelWidth;
            }
            else
            {
                EditorGUI.PropertyField(workRect, property.FindPropertyRelative(ENABLED_PROPERTY_NAME), label);
            }
        }
    }
}