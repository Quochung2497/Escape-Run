using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    public static class TextureExporter
    {
        [MenuItem("Assets/Save to PNG", priority = 80)]
        public static void InitAudioFiles()
        {
            if (Selection.activeObject != null)
            {
                Texture2D texture = Selection.activeObject as Texture2D;
                if (texture != null)
                {
                    byte[] bytes = texture.EncodeToPNG();

                    string path = EditorUtility.SaveFilePanel("Select PNG file path", "", "", "png");
                    if (!string.IsNullOrEmpty(path))
                    {
                        System.IO.File.WriteAllBytes(path, bytes);
                    }
                }
            }
        }

        [MenuItem("Assets/Save to PNG", true, 0)]
        public static bool ValidateInitAudioFiles()
        {
            return Selection.objects != null && Selection.activeObject is Texture2D;
        }
    }
}