using UnityEngine;
using UnityEditor;

namespace Watermelon
{
    [CustomPropertyDrawer(typeof(LineSpacerAttribute))]
    public class LineSpacerDrawer : DecoratorDrawer
    {
        private LineSpacerAttribute lineSpacer
        {
            get { return (LineSpacerAttribute)attribute; }
        }

        public override void OnGUI(Rect position)
        {
            LineSpacerAttribute lineSpacer = this.lineSpacer;

            Color oldGuiColor = GUI.color;
            if(!string.IsNullOrEmpty(lineSpacer.title))
            {
                EditorGUI.LabelField(new Rect(position.x, position.y + lineSpacer.height - 12, position.width, lineSpacer.height), lineSpacer.title, EditorStyles.boldLabel);
                EditorGUI.LabelField(new Rect(position.x, position.y + lineSpacer.height, position.width, lineSpacer.height), "", GUI.skin.horizontalSlider);
            }
            else
            {
                EditorGUI.LabelField(new Rect(position.x, position.y, position.width, lineSpacer.height), "", GUI.skin.horizontalSlider);
            }

            GUI.color = oldGuiColor;
        }

        public override float GetHeight()
        {
            LineSpacerAttribute lineSpacer = this.lineSpacer;

            float height = base.GetHeight();
            if (!string.IsNullOrEmpty(lineSpacer.title))
            {
                height += lineSpacer.height;
            }

            return height;
        }
    }
}