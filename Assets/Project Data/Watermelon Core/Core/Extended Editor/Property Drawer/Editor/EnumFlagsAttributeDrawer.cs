using UnityEngine;
using UnityEditor;

namespace Watermelon
{
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

            UnityEditor.EditorGUI.BeginChangeCheck();
            int flagsValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            if (UnityEditor.EditorGUI.EndChangeCheck())
                property.intValue = flagsValue;
            EditorGUI.EndProperty();
        }
    }
}