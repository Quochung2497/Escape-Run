using UnityEngine;
using UnityEditor;
using System;

namespace Watermelon
{
    public class DateProperty : UnityEditor.PropertyDrawer
    {
        private const float EXPANDED_HEIGHT = 18 * 4;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Float)
            {
                EditorGUILayout.LabelField("Wrong property type!", EditorStyles.boldLabel);

                return;
            }

            if (property.doubleValue == 0)
                property.doubleValue = TimeUtils.GetCurrentUnixTimestamp();

            GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            centeredStyle.fontSize = 14;
            centeredStyle.fontStyle = FontStyle.Bold;

            DateTime currentDateTime = TimeUtils.GetDateTimeFromUnixTime(property.doubleValue);

            EditorGUI.LabelField(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight), label);
            GUI.enabled = false;
            EditorGUI.TextField(new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth - 16, EditorGUIUtility.singleLineHeight), currentDateTime.ToString("dd.MM.yyyy H:mm"));
            GUI.enabled = true;

            if (GUI.Button(new Rect(position.x + position.width - 14, position.y, 14, 14), "Pick", EditorStyles.miniButton))
            {
                property.isExpanded = !property.isExpanded;
            }

            if (property.isExpanded)
            {
                GUI.Box(new Rect(position.x, position.y + 18, position.width, EXPANDED_HEIGHT), "");

                float defaultXPosition = 5;
                float defaultYPosition = position.y + 5;
                float defaultWidth = 30;

                float spacing = (position.width - 235) / 2;


                //Day
                if (GUI.Button(new Rect(position.x + spacing + defaultXPosition + 5, defaultYPosition + 20, defaultWidth, 18), "▲"))
                {
                    currentDateTime = currentDateTime.AddDays(1);
                }
                GUI.Label(new Rect(position.x + spacing + defaultXPosition, defaultYPosition + 40, defaultWidth + 10, 18), currentDateTime.Day.ToString("00"), centeredStyle);
                if (GUI.Button(new Rect(position.x + spacing + defaultXPosition + 5, defaultYPosition + 60, defaultWidth, 18), "▼"))
                {
                    currentDateTime = currentDateTime.AddDays(-1);
                }

                //Separator
                defaultXPosition = 40;
                GUI.Label(new Rect(position.x + spacing + defaultXPosition - 4, defaultYPosition + 40, 10, 18), ".");

                //Month
                if (GUI.Button(new Rect(position.x + spacing + defaultXPosition + 5, defaultYPosition + 20, defaultWidth, 18), "▲"))
                {
                    currentDateTime = currentDateTime.AddMonths(1);
                }
                GUI.Label(new Rect(position.x + spacing + defaultXPosition, defaultYPosition + 40, defaultWidth + 10, 18), currentDateTime.Month.ToString("00"), centeredStyle);
                if (GUI.Button(new Rect(position.x + spacing + defaultXPosition + 5, defaultYPosition + 60, defaultWidth, 18), "▼"))
                {
                    currentDateTime = currentDateTime.AddMonths(-1);
                }

                defaultXPosition = 75;
                GUI.Label(new Rect(position.x + spacing + defaultXPosition - 4, defaultYPosition + 40, 10, 18), ".");
                defaultWidth = 50;

                //Year
                if (GUI.Button(new Rect(position.x + spacing + defaultXPosition + 5, defaultYPosition + 20, defaultWidth, 18), "▲"))
                {
                    currentDateTime = currentDateTime.AddYears(1);
                }
                GUI.Label(new Rect(position.x + spacing + defaultXPosition, defaultYPosition + 40, defaultWidth + 10, 18), currentDateTime.Year.ToString(), centeredStyle);
                if (GUI.Button(new Rect(position.x + spacing + defaultXPosition + 5, defaultYPosition + 60, defaultWidth, 18), "▼"))
                {
                    currentDateTime = currentDateTime.AddYears(-1);
                }

                defaultXPosition = 155;
                defaultWidth = 30;

                //Hour
                if (GUI.Button(new Rect(position.x + spacing + defaultXPosition + 5, defaultYPosition + 20, defaultWidth, 18), "▲"))
                {
                    currentDateTime = currentDateTime.AddHours(1);
                }
                GUI.Label(new Rect(position.x + spacing + defaultXPosition, defaultYPosition + 40, defaultWidth + 10, 18), currentDateTime.Hour.ToString("00"), centeredStyle);
                if (GUI.Button(new Rect(position.x + spacing + defaultXPosition + 5, defaultYPosition + 60, defaultWidth, 18), "▼"))
                {
                    currentDateTime = currentDateTime.AddHours(-1);
                }

                defaultXPosition = 190;
                GUI.Label(new Rect(position.x + spacing + defaultXPosition - 3, defaultYPosition + 40, 10, 18), ":");

                //Minutes
                if (GUI.Button(new Rect(position.x + spacing + defaultXPosition + 5, defaultYPosition + 20, defaultWidth, 18), "▲"))
                {
                    currentDateTime = currentDateTime.AddMinutes(1);
                }
                GUI.Label(new Rect(position.x + spacing + defaultXPosition, defaultYPosition + 40, defaultWidth + 10, 18), currentDateTime.Minute.ToString("00"), centeredStyle);
                if (GUI.Button(new Rect(position.x + spacing + defaultXPosition + 5, defaultYPosition + 60, defaultWidth, 18), "▼"))
                {
                    currentDateTime = currentDateTime.AddMinutes(-1);
                }

            }

            if (GUI.changed)
            {
                property.doubleValue = TimeUtils.GetUnixTimestampFromDateTime(currentDateTime);

                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + (property.isExpanded ? EXPANDED_HEIGHT : 0) + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
