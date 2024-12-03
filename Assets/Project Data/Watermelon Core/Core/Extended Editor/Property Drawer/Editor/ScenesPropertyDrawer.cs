using System.IO;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [CustomPropertyDrawer(typeof(ScenesAttribute))]
    public class ScenesPropertyDrawer : UnityEditor.PropertyDrawer
    {
        private bool isInited = false;

        private EditorBuildSettingsScene[] scenes;
        private string[] scenesNames;

        private void Init()
        {
            scenes = EditorBuildSettings.scenes;
            scenesNames = new string[scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                scenesNames[i] = System.IO.Path.GetFileNameWithoutExtension(scenes[i].path);
            }

            isInited = true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!isInited)
                Init();

            string selectedScene = property.stringValue;
            int selectedSceneIndex = 0;

            if (string.IsNullOrEmpty(selectedScene))
            {
                property.stringValue = null;
                selectedSceneIndex = -1;
            }
            else
            {
                int tempSceneIndex = System.Array.FindIndex(scenesNames, x => x == selectedScene);

                if (tempSceneIndex != -1)
                {
                    selectedSceneIndex = tempSceneIndex;
                }
                else
                {
                    property.stringValue = "Unknown";
                    selectedSceneIndex = -1;
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var amountRect = new Rect(position.x, position.y, position.width, position.height);

            selectedSceneIndex = EditorGUI.Popup(amountRect, selectedSceneIndex, scenesNames);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();

            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = scenesNames[selectedSceneIndex];
            }
        }
    }
}
