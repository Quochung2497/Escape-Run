using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Watermelon
{
    [CustomEditor(typeof(SettingsPanel))]
    public class SettingsPanelEditor : WatermelonEditor
    {
        private const string SETTINGS_ANIMATION_PROPERTY_NAME = "settingsAnimation";
        private const string X_PANEL_POSITION_PROPERTY_NAME = "xPanelPosition";
        private const string Y_PANEL_POSITION_PROPERTY_NAME = "yPanelPosition";
        private const string ELEMENT_SPACE_PROPERTY_NAME = "elementSpace";
        private const string SETTINGS_BUTTONS_PROPERTY_NAME = "settingsButtonsInfo";

        private ReorderableList reorderableList;

        private SettingsPanel settingsPanel;

        private SerializedProperty settingsAnimationProperty;
        private SerializedProperty xPanelPositionProperty;
        private SerializedProperty yPanelPositionProperty;
        private SerializedProperty elementSpaceProperty;
        private SerializedProperty settingsButtonsProperty;

        protected override void OnEnable()
        {
            settingsPanel = target as SettingsPanel;

            settingsAnimationProperty = serializedObject.FindProperty(SETTINGS_ANIMATION_PROPERTY_NAME);
            xPanelPositionProperty = serializedObject.FindProperty(X_PANEL_POSITION_PROPERTY_NAME);
            yPanelPositionProperty = serializedObject.FindProperty(Y_PANEL_POSITION_PROPERTY_NAME);
            elementSpaceProperty = serializedObject.FindProperty(ELEMENT_SPACE_PROPERTY_NAME);
            settingsButtonsProperty = serializedObject.FindProperty(SETTINGS_BUTTONS_PROPERTY_NAME);

            reorderableList = new ReorderableList(serializedObject, settingsButtonsProperty, true, true, true, true);

            reorderableList.drawHeaderCallback += DrawHeader;
            reorderableList.drawElementCallback += DrawElement;

            reorderableList.onAddCallback += AddItem;
            reorderableList.onRemoveCallback += RemoveItem;
            reorderableList.onReorderCallback += ReorderItems;
        }

        private void OnDisable()
        {
            reorderableList.drawHeaderCallback -= DrawHeader;
            reorderableList.drawElementCallback -= DrawElement;

            reorderableList.onAddCallback -= AddItem;
            reorderableList.onRemoveCallback -= RemoveItem;
            reorderableList.onReorderCallback -= ReorderItems;
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Buttons");
        }

        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            SettingsPanel.SettingsButtonInfo item = settingsPanel.SettingsButtonsInfo[index];

            EditorGUI.BeginChangeCheck();
            item.SettingsButton = (SettingsButtonBase)EditorGUI.ObjectField(new Rect(rect.x + 18, rect.y, rect.width - 18, 20), item.SettingsButton, typeof(SettingsButtonBase), true);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
                //settingsPanel.OnValidate();
            }
        }

        private void AddItem(ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;

            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("settingsButton").objectReferenceValue = null;
        }

        private void RemoveItem(ReorderableList list)
        {
            list.serializedProperty.serializedObject.Update();
            list.serializedProperty.DeleteArrayElementAtIndex(list.index);
            list.serializedProperty.serializedObject.ApplyModifiedProperties();
        }

        private void ReorderItems(ReorderableList list)
        {
            //settingsPanel.OnValidate();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Space(5);

            Object tempObject = settingsAnimationProperty.objectReferenceValue;
            EditorGUILayout.PropertyField(settingsAnimationProperty);
            if(tempObject != settingsAnimationProperty.objectReferenceValue)
            {
                if(settingsAnimationProperty.objectReferenceValue != null)
                {
                    if(EditorApplication.isPlaying)
                    {
                        SettingsAnimation settingsAnimation = (SettingsAnimation)settingsAnimationProperty.objectReferenceValue;
                        settingsAnimation.Init(settingsPanel);
                    }
                }
            }

            EditorGUILayout.PropertyField(xPanelPositionProperty);
            EditorGUILayout.PropertyField(yPanelPositionProperty);
            EditorGUILayout.PropertyField(elementSpaceProperty);

            GUILayout.Space(8);

            reorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
