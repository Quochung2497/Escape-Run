using UnityEditor;
using UnityEngine;

namespace Watermelon
{

    // Doubled Int Drawer
    [CustomPropertyDrawer(typeof(DuoInt)), CustomPropertyDrawer(typeof(DuoFloat)), CustomPropertyDrawer(typeof(DuoDouble))]
    public class DuoTypeDrawer : UnityEditor.PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float valuesSpace = 6f;
            float fieldSize = (EditorGUIUtility.currentViewWidth - position.x - valuesSpace - 5f) * 0.5f;

            // Calculate rects
            var amountRect = new Rect(position.x, position.y, fieldSize, position.height);
            var unitRect = new Rect(position.x + fieldSize + valuesSpace, position.y, fieldSize, position.height);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("firstValue"), GUIContent.none);
            EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("secondValue"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}