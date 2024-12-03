using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    public class RenameEditorWindow : EditorWindow
    {
        private string folerName;
        private string format = "000";

        private string assetPath;

        private static void Open(string assetPath)
        {
            Vector2 windowSize = new Vector2(200, 100);
            RenameEditorWindow window = (RenameEditorWindow)GetWindow(typeof(RenameEditorWindow), true, "Renaming settings", true);
            window.minSize = windowSize;
            window.maxSize = windowSize;

            window.assetPath = assetPath;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Name:");
            folerName = EditorGUILayout.TextField(GUIContent.none, folerName);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Format:");
            format = EditorGUILayout.TextField(GUIContent.none, format);
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Rename"))
            {
                Rename();
            }

            EditorGUILayout.EndVertical();
        }

        private void Rename()
        {
            EditorApplication.delayCall += delegate
            {
                string projectFolder = Application.dataPath.Replace("Assets", "");

                if (Directory.Exists(projectFolder + assetPath))
                {
                    IEnumerable<FileInfo> files = new DirectoryInfo(projectFolder + assetPath).GetFiles().Where(f => (f.Attributes & FileAttributes.Hidden) == 0 && f.Extension != ".meta");

                    int count = files.Count();
                    int index = 0;
                    foreach (FileInfo file in files)
                    {
                        EditorUtility.DisplayProgressBar("File renaming", "Renaming file.. (" + index + "/" + count + ")", (float)index / count);

                        AssetDatabase.RenameAsset(assetPath + file.Name, folerName + index.ToString(format));

                        index++;
                    }
                }

                EditorUtility.ClearProgressBar();
                AssetDatabase.Refresh();
            };

            Close();
        }

        [MenuItem("Assets/Rename Folder Objects", priority = 80)]
        public static void RenameFolder()
        {
            Open(AssetDatabase.GetAssetPath(Selection.activeObject) + "/");
        }

        [MenuItem("Assets/Rename Folder Objects", true, 0)]
        public static bool ValidateRenameFolder()
        {
            return Selection.activeObject != null && Selection.activeObject is DefaultAsset;
        }
    }
}