using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Watermelon
{
    [CustomPropertyDrawer(typeof(PopupAttribute))]
    public class PopupDrawer : UnityEditor.PropertyDrawer
    {
        private bool m_IsInited = false;

        private string[] m_PopupElements;

        private void Init(SerializedProperty property)
        {
            PopupAttribute popupAttribute = (PopupAttribute)attribute;

            m_PopupElements = popupAttribute.arrayParams.Select(x => x.ToString()).ToArray();

            m_IsInited = true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!m_IsInited)
                Init(property);

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    DrawEnum(position, property, label, SerializedPropertyType.Integer);
                    break;
                case SerializedPropertyType.Float:
                    DrawEnum(position, property, label, SerializedPropertyType.Float);
                    break;
                case SerializedPropertyType.String:
                    DrawEnum(position, property, label, SerializedPropertyType.String);
                    break;
            }
        }

        public void SetValue(SerializedProperty property, SerializedPropertyType serializedType, string value)
        {
            switch (serializedType)
            {
                case SerializedPropertyType.Integer:
                    if (value == null)
                        property.intValue = -1;
                    else
                        property.intValue = (int)System.Convert.ChangeType(value, typeof(int));
                    break;
                case SerializedPropertyType.Float:
                    if (value == null)
                        property.floatValue = -1;
                    else
                        property.floatValue = (float)System.Convert.ChangeType(value, typeof(float));
                    break;
                case SerializedPropertyType.String:
                    if (value == null)
                        property.stringValue = "";
                    else
                        property.stringValue = value;
                    break;
            }
        }

        public string GetValue(SerializedProperty property, SerializedPropertyType serializedType)
        {
            switch (serializedType)
            {
                case SerializedPropertyType.Integer:
                    return property.intValue.ToString();
                case SerializedPropertyType.Float:
                    return property.floatValue.ToString();
                case SerializedPropertyType.String:
                    return property.stringValue;
            }

            return "";
        }

        private void DrawEnum(Rect position, SerializedProperty property, GUIContent label, SerializedPropertyType serializedType)
        {
            string propertyValue = GetValue(property, serializedType);

            int m_SelectedFileId = 0;

            if (string.IsNullOrEmpty(propertyValue))
            {
                SetValue(property, serializedType, null);

                m_SelectedFileId = -1;
            }
            else
            {
                int foundedKey = System.Array.FindIndex(m_PopupElements, x => x == propertyValue);

                if (foundedKey != -1)
                {
                    m_SelectedFileId = foundedKey;
                }
                else
                {
                    SetValue(property, serializedType, null);

                    m_SelectedFileId = -1;
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var amountRect = new Rect(position.x, position.y, position.width, position.height);

            m_SelectedFileId = EditorGUI.Popup(amountRect, m_SelectedFileId, m_PopupElements);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();

            if (GUI.changed)
            {
                SetValue(property, serializedType, m_PopupElements[m_SelectedFileId]);
            }
        }
    }
}
