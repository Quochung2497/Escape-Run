using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Watermelon.List
{
    public abstract class AbstractField
    {
        public abstract void Draw(SerializedProperty elementProperty, Rect rect, CustomListStyle style);

        public abstract float GetHeight(SerializedProperty elementProperty, CustomListStyle style);
    }

    public class PropertyField : AbstractField
    {
        protected string propertyName;
        private GUIContent customGUIContent;
        private bool useCustomGUIContent;
        public PropertyField(string propertyName)
        {
            this.propertyName = propertyName;
            this.useCustomGUIContent = false;
        }

        public PropertyField(string propertyName, GUIContent customGUIContent)
        {
            this.propertyName = propertyName;
            this.customGUIContent = customGUIContent;
            this.useCustomGUIContent = true;
        }

        public override void Draw(SerializedProperty elementProperty, Rect rect, CustomListStyle style)
        {
            if (useCustomGUIContent)
            {
                EditorGUI.PropertyField(rect, elementProperty.FindPropertyRelative(propertyName), customGUIContent);
            }
            else
            {
                EditorGUI.PropertyField(rect, elementProperty.FindPropertyRelative(propertyName));
            }

        }

        public override float GetHeight(SerializedProperty elementProperty, CustomListStyle style)
        {
            if (useCustomGUIContent)
            {
                return EditorGUI.GetPropertyHeight(elementProperty.FindPropertyRelative(propertyName), customGUIContent);
            }
            else
            {
                return EditorGUI.GetPropertyHeight(elementProperty.FindPropertyRelative(propertyName));
            }
        }
    }

    public class CustomField : AbstractField
    {
        public delegate void DrawCallbackDelegate(SerializedProperty elementProperty, Rect rect, CustomListStyle style);
        public delegate float GetHeightCallbackDelegate(SerializedProperty elementProperty, CustomListStyle style);
        public DrawCallbackDelegate drawCallbackDelegate;
        public GetHeightCallbackDelegate getHeightCallbackDelegate;

        public CustomField(DrawCallbackDelegate drawCallbackDelegate, GetHeightCallbackDelegate getHeightCallbackDelegate)
        {
            this.drawCallbackDelegate = drawCallbackDelegate;
            this.getHeightCallbackDelegate = getHeightCallbackDelegate;
        }

        public override void Draw(SerializedProperty elementProperty, Rect rect, CustomListStyle style)
        {
            drawCallbackDelegate?.Invoke(elementProperty, rect, style);
        }

        public override float GetHeight(SerializedProperty elementProperty, CustomListStyle style)
        {
            return getHeightCallbackDelegate.Invoke(elementProperty, style);
        }
    }

    public class Space : AbstractField
    {
        float height;

        public Space()
        {
            height = 8;
        }

        public Space(float height)
        {
            this.height = height;
        }

        public override void Draw(SerializedProperty elementProperty, Rect rect, CustomListStyle style)
        {
        }

        public override float GetHeight(SerializedProperty elementProperty, CustomListStyle style)
        {
            return height;
        }
    }

    public class Separator : AbstractField
    {
        private Color color;

        public Separator()
        {
            color = new Color(0.2f, 0.2f, 0.2f, 1);
        }

        public Separator(Color color)
        {
            this.color = color;
        }

        public override void Draw(SerializedProperty elementProperty, Rect rect, CustomListStyle style)
        {
            EditorGUI.DrawRect(rect, color);
        }

        public override float GetHeight(SerializedProperty elementProperty, CustomListStyle style)
        {
            return 2;
        }
    }
}
