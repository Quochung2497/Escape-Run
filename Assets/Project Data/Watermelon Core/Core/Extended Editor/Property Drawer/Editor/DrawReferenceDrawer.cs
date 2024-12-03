using UnityEngine;
using UnityEditor;

namespace Watermelon
{
    [CustomPropertyDrawer(typeof(DrawReferenceAttribute), true)]
    public class DrawReferenceDrawer : UnityEditor.PropertyDrawer
    {
        private float m_Space = EditorGUIUtility.standardVerticalSpacing * 2;

        private bool m_Inited = false;
        private SerializedObject m_SerializedObject;

        private void Init(SerializedProperty property)
        {
            if (property.objectReferenceValue == null)
                return;

            m_SerializedObject = new SerializedObject(property.objectReferenceValue);

            m_Inited = true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!m_Inited)
                Init(property);

            int indentLevel = EditorGUI.indentLevel + 1;

            GUI.Box(new Rect(0, position.y, Screen.width, position.height), GUIContent.none);

            position.y += EditorGUIUtility.standardVerticalSpacing;
            position.height = 16;

            if (m_SerializedObject != null)
            {
                property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height), property.isExpanded, label, true);
                EditorGUI.PropertyField(new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, position.height), property, GUIContent.none);

                position.y += 20;

                EditorGUI.indentLevel = indentLevel;

                if (property.isExpanded)
                {
                    m_SerializedObject.Update();

                    var prop = m_SerializedObject.GetIterator();
                    prop.NextVisible(true);

                    int subIndentLevel = EditorGUI.indentLevel;

                    while (prop.NextVisible(false))
                    {
                        EditorGUI.indentLevel = indentLevel + prop.depth;

                        position.height = EditorGUI.GetPropertyHeight(prop);
                        EditorGUI.PropertyField(position, prop, prop.isExpanded);
                        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
                    }

                    if (GUI.changed)
                        m_SerializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property);
            }

            EditorGUI.indentLevel = indentLevel - 1;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                return base.GetPropertyHeight(property, label) + m_Space;
            }

            if (!m_Inited)
                Init(property);

            float height = base.GetPropertyHeight(property, label) + m_Space;
            if (m_SerializedObject != null)
            {
                if (property.isExpanded)
                {
                    var prop = m_SerializedObject.GetIterator();
                    prop.NextVisible(true);

                    while (prop.NextVisible(false))
                    {
                        height += EditorGUI.GetPropertyHeight(prop) + EditorGUIUtility.standardVerticalSpacing;
                    }

                    height += EditorGUIUtility.standardVerticalSpacing;
                }
            }
            return height;
        }
    }
}
