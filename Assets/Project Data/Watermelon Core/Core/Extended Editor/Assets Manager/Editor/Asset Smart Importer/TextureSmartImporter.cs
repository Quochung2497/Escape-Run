using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    public partial class TextureSmartImporter : EditorWindow
    {
        private static TextureSmartImporter window;

        private List<TextureCase> importedTextures = new List<TextureCase>();

        private int selectedPresetID = -1;
        private int selectedTextureID = 0;

        private Texture2D selectedTexture;

        private IEnumerator enumerator;
        private bool isBusy = false;

        //Styles
        private GUIStyle centeredImageStyle;

        //Consts
        private readonly Vector2 WINDOW_SIZE = new Vector2(380, 205);
        private const string WINDOW_TITLE = "Imported textures: {0}/{1}";

        private const string CUTTING_PROGRESS_TITLE = "Texture Resaving";
        private const string CUTTING_PROGRESS_CONTENT_01 = "Prepairing texture..";
        private const string CUTTING_PROGRESS_CONTENT_02 = "Force reimport..";
        private const string CUTTING_PROGRESS_CONTENT_03 = "Cutting pixel..";
        private const string CUTTING_PROGRESS_CONTENT_04 = "Saving texture..";

        private const string INITTING_PROGRESS_TITLE = "Texture Initializing";
        private const string INITTING_PROGRESS_CONTENT_01 = "Prepairing texture..";
        private const string INITTING_PROGRESS_CONTENT_02 = "Size optimizing..";
        private const string INITTING_PROGRESS_CONTENT_03 = "Saving texture..";
        private const string INITTING_PROGRESS_CONTENT_04 = "Importing preset settings..";
        private const string INITTING_PROGRESS_CONTENT_05 = "Reimporting texture..";

        private const string PNG_EXTENSION = ".png";

        private static void Open(params TextureCase[] objects)
        {
            if (window == null)
            {
                window = (TextureSmartImporter)GetWindow(typeof(TextureSmartImporter), true, "Texture Import", true);
                window.importedTextures = new List<TextureCase>();
                window.InitWindowSize();

                for (int i = 0; i < objects.Length; i++)
                {
                    window.importedTextures.Add(objects[i]);
                }

                //Select first texture
                window.SelectTexture(0);
                //Select default preset
                window.SelectPreset(DEFAULT_PRESET_INDEX);
            }
            else
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    window.importedTextures.Add(objects[i]);
                }
            }

            window.titleContent = new GUIContent(string.Format(WINDOW_TITLE, (window.selectedTextureID + 1), window.importedTextures.Count));
        }

        private void InitWindowSize()
        {
            Vector2 windowSize = new Vector2(window.WINDOW_SIZE.x, window.WINDOW_SIZE.y + window.GetExtraHeight());
            window.minSize = windowSize;
            window.maxSize = windowSize;
        }

        private void InitStyles()
        {
            centeredImageStyle = GUI.skin.GetStyle("Label");
            centeredImageStyle.alignment = TextAnchor.MiddleCenter;
        }

        private void OnGUI()
        {
            InitStyles();

            if (selectedTexture == null)
            {
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Loading..", centeredImageStyle);

                //Try to reinit selected texture
                selectedTexture = importedTextures[selectedTextureID].Texture;
            }

            int texturesCount = importedTextures.Count;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.Width(130));
            GUILayout.Space(4);

            Rect imageBlockRect = EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.Width(128), GUILayout.Height(128));
            EditorGUILayout.LabelField(new GUIContent(selectedTexture), centeredImageStyle, GUILayout.Width(128), GUILayout.Height(128));

            if (GUI.Button(new Rect(imageBlockRect.x + (imageBlockRect.width / 2) - 40, imageBlockRect.y + imageBlockRect.height - 25, 80, 18), "Cut Pixel", EditorStyles.miniButton))
            {
                CutPixel();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("Skip", EditorStyles.miniButton))
                NextTexture();

            if (GUILayout.Button("Init", EditorStyles.miniButton))
                InitTextures(false, importedTextures[selectedTextureID]);

            if (GUILayout.Button("Init All", EditorStyles.miniButton))
                InitTextures(true, importedTextures.GetRange(selectedTextureID, importedTextures.Count - selectedTextureID).ToArray());

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            for (int i = 0; i < presets.Length; i++)
            {
                bool isSelected = selectedPresetID == i;
                if (isSelected)
                    GUI.color = Color.green;

                Rect clickRect = EditorGUILayout.BeginVertical(GUI.skin.textField);
                EditorGUILayout.LabelField(new GUIContent(presets[i].name, presets[i].description), EditorStyles.boldLabel);
                if (isSelected && !string.IsNullOrEmpty(presets[i].description))
                    EditorGUILayout.LabelField(new GUIContent(presets[i].description), GUI.skin.textField);
                EditorGUILayout.EndVertical();

                if (GUI.Button(clickRect, GUIContent.none, GUIStyle.none))
                    SelectPreset(i);

                if (isSelected)
                    GUI.color = Color.white;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void SelectTexture(int index)
        {
            selectedTextureID = index;
            selectedTexture = importedTextures[index].Texture;

            window.titleContent = new GUIContent(string.Format(WINDOW_TITLE, (selectedTextureID + 1), importedTextures.Count));

            Repaint();
        }

        private float GetExtraHeight()
        {
            float height = 0;

            for (int i = 0; i < presets.Length; i++)
            {
                if (!string.IsNullOrEmpty(presets[i].name)) height += 24;
            }

            if (selectedPresetID != -1 && !string.IsNullOrEmpty(presets[selectedPresetID].description))
                height += 18;

            if (height + 10 > WINDOW_SIZE.y)
                return height + 10 - WINDOW_SIZE.y;

            return 0;
        }

        private void SelectPreset(int index)
        {
            selectedPresetID = index;

            InitWindowSize();
        }

        private void NextTexture()
        {
            if (selectedTextureID + 1 < importedTextures.Count)
            {
                SelectTexture(selectedTextureID + 1);
            }
            else
            {
                Close();
            }
        }

        private void CutPixel()
        {
            if (isBusy)
                return;

            Texture2D selectedTexture = importedTextures[selectedTextureID].Texture;
            if (selectedTexture != null)
            {
                EditorUtility.DisplayProgressBar(CUTTING_PROGRESS_TITLE, CUTTING_PROGRESS_CONTENT_01, 0.25f);

                importedTextures[selectedTextureID].textureImporter.isReadable = true;

                bool useCrunch = importedTextures[selectedTextureID].textureImporter.crunchedCompression;
                importedTextures[selectedTextureID].textureImporter.crunchedCompression = false;

                int tempSize = importedTextures[selectedTextureID].textureImporter.maxTextureSize;
                importedTextures[selectedTextureID].textureImporter.maxTextureSize = 8192;

                EditorUtility.DisplayProgressBar(CUTTING_PROGRESS_TITLE, CUTTING_PROGRESS_CONTENT_02, 0.5f);

                AssetDatabase.ImportAsset(importedTextures[selectedTextureID].assetPath, ImportAssetOptions.ForceUpdate);

                EditorUtility.DisplayProgressBar(CUTTING_PROGRESS_TITLE, CUTTING_PROGRESS_CONTENT_03, 0.75f);

                Texture2D tempTexture = new Texture2D(selectedTexture.width, selectedTexture.height);
                tempTexture.SetPixels(selectedTexture.GetPixels());
                tempTexture.SetPixel(0, 0, new Color(0, 0, 0, 0));
                tempTexture.Apply();

                EditorUtility.DisplayProgressBar(CUTTING_PROGRESS_TITLE, CUTTING_PROGRESS_CONTENT_04, 0.95f);

                SetEnumerator(SaveTexture(importedTextures[selectedTextureID], tempTexture.EncodeToPNG(), () => EditorUtility.ClearProgressBar()));

                importedTextures[selectedTextureID].textureImporter.maxTextureSize = tempSize;
                importedTextures[selectedTextureID].textureImporter.crunchedCompression = useCrunch;

                importedTextures[selectedTextureID].textureImporter.SaveAndReimport();
            }
        }

        private void InitTextures(bool initAll, params TextureCase[] texturesCase)
        {
            SetEnumerator(InitEnumerator(initAll, texturesCase));
        }

        private void EnumerateUpdate()
        {
            if (!enumerator.MoveNext())
            {
                EditorApplication.update -= EnumerateUpdate;

                isBusy = false;
            }
        }

        private void SetEnumerator(IEnumerator enumerator)
        {
            if (isBusy)
                return;

            this.enumerator = enumerator;

            EditorApplication.update += EnumerateUpdate;

            isBusy = true;
        }

        private Texture2D OptimizeTexture(TextureImporter textureImporter, Texture2D baseTexture, out bool requiredSave)
        {
            int resultOffsetX = 0;
            int resultOffsetY = 0;

            int width = baseTexture.width;
            int height = baseTexture.height;
            int stepX = 0;
            int stepY = 0;

            if (width % 4 != 0)
                stepX = 4 - width % 4;

            if (height % 4 != 0)
                stepY = 4 - height % 4;

            TextureImporterSettings textureSettings = new TextureImporterSettings();
            textureImporter.ReadTextureSettings(textureSettings);

            if (textureImporter.spriteImportMode == SpriteImportMode.Single)
            {
                switch ((SpriteAlignment)textureSettings.spriteAlignment)
                {
                    case SpriteAlignment.TopLeft:
                        resultOffsetX = 0;
                        resultOffsetY = stepY;

                        break;
                    case SpriteAlignment.TopCenter:
                        resultOffsetX = stepX / 2;
                        resultOffsetY = stepY;

                        break;
                    case SpriteAlignment.TopRight:
                        resultOffsetX = stepX;
                        resultOffsetY = stepY;

                        break;
                    case SpriteAlignment.LeftCenter:
                        resultOffsetX = 0;
                        resultOffsetY = stepY / 2;

                        break;
                    case SpriteAlignment.Center:
                        resultOffsetX = stepX / 2;
                        resultOffsetY = stepY / 2;

                        break;
                    case SpriteAlignment.RightCenter:
                        resultOffsetX = stepX;
                        resultOffsetY = stepY / 2;

                        break;
                    case SpriteAlignment.BottomLeft:
                        resultOffsetX = 0;
                        resultOffsetY = 0;

                        break;
                    case SpriteAlignment.BottomCenter:
                        resultOffsetX = stepX / 2;
                        resultOffsetY = 0;

                        break;
                    case SpriteAlignment.BottomRight:
                        resultOffsetX = stepX;
                        resultOffsetY = 0;

                        break;
                    case SpriteAlignment.Custom:
                        resultOffsetX = stepX - (int)Mathf.Clamp(stepX * (1 - textureSettings.spritePivot.x), 0, stepX);
                        resultOffsetY = stepY - (int)Mathf.Clamp(stepY * (1 - textureSettings.spritePivot.y), 0, stepY);

                        break;
                }
            }
            else if (textureImporter.spriteImportMode == SpriteImportMode.Multiple)
            {
                resultOffsetX = 0;
                resultOffsetY = stepY;
            }
            else
            {
                resultOffsetX = stepX / 2;
                resultOffsetY = stepY / 2;
            }

            if (stepY == 0 && stepX == 0)
            {
                requiredSave = false;

                return baseTexture;
            }

            Texture2D tempTexture = new Texture2D(width + stepX, height + stepY);

            for (int i = 0; i < tempTexture.width; i++)
            {
                for (int j = 0; j < tempTexture.height; j++)
                {
                    tempTexture.SetPixel(i, j, Color.clear);
                }
            }

            tempTexture.SetPixels(resultOffsetX, resultOffsetY, width, height, baseTexture.GetPixels());
            tempTexture.Apply();

            requiredSave = true;

            return tempTexture;
        }

        private IEnumerator SaveTexture(TextureCase textureCase, byte[] textureBytes, System.Action onComplete = null)
        {
            string filePath = Application.dataPath + textureCase.assetPath.Replace("Assets/", "/");
            string extension = Path.GetExtension(filePath);
            string metaFilePath = AssetDatabase.GetTextMetaFilePathFromAssetPath(filePath);

            if (File.Exists(filePath))
            {
                try
                {
                    File.WriteAllBytes(filePath, textureBytes);

                    if (extension != PNG_EXTENSION)
                    {
                        //Get file path with new extension
                        string newFilePath = filePath.Replace(extension, PNG_EXTENSION);

                        //Rename file and metafile 
                        File.Move(filePath, newFilePath);
                        File.Move(metaFilePath, metaFilePath.Replace(extension, PNG_EXTENSION));

                        AssetDatabase.Refresh();

                        //Get project path
                        newFilePath = newFilePath.Replace(Application.dataPath, "Assets");

                        TextureImporter newTextureImporter = (TextureImporter)TextureImporter.GetAtPath(newFilePath);
                        if (newTextureImporter != null)
                        {
                            newTextureImporter.isReadable = false;
                            newTextureImporter.SaveAndReimport();
                        }

                        textureCase.assetPath = newFilePath;
                    }
                    else
                    {
                        textureCase.textureImporter.isReadable = false;
                        textureCase.textureImporter.SaveAndReimport();
                    }
                }
                catch (System.Exception)
                {
                    Debug.LogError("Access denied: " + filePath);
                }

                yield return null;

                if (onComplete != null)
                    onComplete.Invoke();
            }
        }

        private IEnumerator InitEnumerator(bool initAll, params TextureCase[] texturesCase)
        {
            if (selectedPresetID != -1)
            {
                for (int textureIndex = 0; textureIndex < texturesCase.Length; textureIndex++)
                {
                    EditorUtility.DisplayProgressBar(INITTING_PROGRESS_TITLE, INITTING_PROGRESS_CONTENT_01, 0.25f);

                    TextureImporter textureImporter = texturesCase[textureIndex].textureImporter;

                    textureImporter.isReadable = true;
                    AssetDatabase.ImportAsset(texturesCase[textureIndex].assetPath, ImportAssetOptions.ForceUpdate);

                    if (presets[selectedPresetID].optimizeSize)
                    {
                        EditorUtility.DisplayProgressBar(INITTING_PROGRESS_TITLE, INITTING_PROGRESS_CONTENT_02, 0.35f);
                        bool tempRequiredSave = false;
                        Texture2D tempTexture = OptimizeTexture(texturesCase[textureIndex].textureImporter, texturesCase[textureIndex].Texture, out tempRequiredSave);

                        if (tempRequiredSave)
                        {
                            textureImporter.maxTextureSize = 8192;
                            AssetDatabase.ImportAsset(texturesCase[textureIndex].assetPath, ImportAssetOptions.ForceUpdate);

                            EditorUtility.DisplayProgressBar(INITTING_PROGRESS_TITLE, INITTING_PROGRESS_CONTENT_03, 0.45f);
                            IEnumerator saveTextureEnumerator = SaveTexture(texturesCase[textureIndex], tempTexture.EncodeToPNG(), () => Debug.Log("Test"));
                            while (!saveTextureEnumerator.MoveNext())
                            {
                                yield return null;
                            }
                        }
                    }

                    EditorUtility.DisplayProgressBar(INITTING_PROGRESS_TITLE, INITTING_PROGRESS_CONTENT_04, 0.75f);

                    AssetDatabase.StartAssetEditing();
                    textureImporter.spritePivot = presets[selectedPresetID].customPivot;
                    textureImporter.spritePixelsPerUnit = presets[selectedPresetID].pixelsPerUnit;

                    if (presets[selectedPresetID].spriteAlignment != null)
                    {
                        TextureImporterSettings textureSettings = new TextureImporterSettings();
                        textureImporter.ReadTextureSettings(textureSettings);

                        textureSettings.spriteAlignment = (int)presets[selectedPresetID].spriteAlignment.Value;
                        textureSettings.spritePivot = presets[selectedPresetID].customPivot;

                        textureImporter.SetTextureSettings(textureSettings);
                    }

                    for (int i = 0; i < presets[selectedPresetID].presets.Length; i++)
                    {
                        if (presets[selectedPresetID].presets[i].name == "Default")
                        {
                            textureImporter.compressionQuality = presets[selectedPresetID].presets[i].compresionQuality;
                            textureImporter.textureCompression = presets[selectedPresetID].presets[i].textureCompression;
                            textureImporter.maxTextureSize = presets[selectedPresetID].presets[i].maxTextureSize;
                            textureImporter.crunchedCompression = presets[selectedPresetID].presets[i].crunchedCompression;
                        }
                        else
                        {
                            if (presets[selectedPresetID].presets[i].clearSettings)
                            {
                                textureImporter.ClearPlatformTextureSettings(presets[selectedPresetID].presets[i].name);
                            }
                            else
                            {
                                TextureImporterPlatformSettings importSettings = textureImporter.GetDefaultPlatformTextureSettings();

                                importSettings.name = presets[selectedPresetID].presets[i].name;
                                importSettings.format = presets[selectedPresetID].presets[i].format;
                                importSettings.maxTextureSize = presets[selectedPresetID].presets[i].maxTextureSize;
                                importSettings.overridden = true;

                                importSettings.resizeAlgorithm = TextureResizeAlgorithm.Bilinear;
                                importSettings.textureCompression = TextureImporterCompression.Compressed;
                                importSettings.compressionQuality = 100;
                                importSettings.allowsAlphaSplitting = false;

                                textureImporter.SetPlatformTextureSettings(importSettings);
                            }
                        }
                    }

                    AssetDatabase.StopAssetEditing();

                    EditorUtility.DisplayProgressBar(INITTING_PROGRESS_TITLE, INITTING_PROGRESS_CONTENT_05, 0.95f);

                    yield return null;

                    textureImporter.isReadable = false;
                    textureImporter.SaveAndReimport();

                    NextTexture();
                }
            }
            else
            {
                if (initAll)
                    Close();
                else
                    NextTexture();
            }

            EditorUtility.ClearProgressBar();
        }

        [MenuItem("Assets/Initialize Texture", priority = 80)]
        public static void InitAudioFiles()
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                Texture2D texture2D = Selection.objects[i] as Texture2D;
                if (texture2D != null)
                {
                    Open(new TextureCase(texture2D, (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(texture2D))));
                }
            }
        }

        [MenuItem("Assets/Initialize Texture", true, 0)]
        public static bool ValidateInitAudioFiles()
        {
            return Selection.objects != null && Selection.activeObject is Texture2D;
        }

        private class Preset
        {
            /// <summary>
            /// Preset name displayed in list
            /// </summary>
            public string name;

            /// <summary>
            /// Is shown when preset is selected
            /// </summary>
            public string description;

            /// <summary>
            /// Makes texture size divisible by 4 without the rest (required if you want to use crunch compression)
            /// </summary>
            public bool optimizeSize;

            /// <summary>
            /// Tag for sprite packer
            /// </summary>
            public string packingTag;

            /// <summary>
            /// 100 pixels per unit would mean a sprite that's 100 pixels would equal 1 unit in the scene.
            /// </summary>
            public float pixelsPerUnit = 100;

            /// <summary>
            /// Texture pivot. Leave it null if you don't want to change pivot setting.
            /// </summary>
            public SpriteAlignment? spriteAlignment = null;

            /// <summary>
            /// Default texture pivot (be sure to set spriteAlignment to SpriteAlignment.Custom if you want to use this value as pivot)
            /// </summary>
            public Vector2 customPivot = new Vector2(0.5f, 0.5f);

            /// <summary>
            /// Must be at least one preset (truly - not, but why you use util if you don't add presets?)
            /// </summary>
            public Settings[] presets;

            public class Settings
            {
                /// <summary>
                /// Build target name (Android, iOS, Standalone, WebGL, tvOS, Switch and etc..)
                /// </summary>
                public string name;

                /// <summary>
                /// Max texture size (32, 64, 128, 256, 512, 1024, 2048, 4096, 8192)
                /// </summary>
                public int maxTextureSize = 1024;

                /// <summary>
                /// Here is all required information - https://docs.unity3d.com/ScriptReference/TextureImporterFormat.html
                /// </summary>
                public TextureImporterFormat format = TextureImporterFormat.ARGB32;

                /// <summary>
                /// More information about resize algorithm - https://docs.unity3d.com/ScriptReference/TextureResizeAlgorithm.html
                /// </summary>
                public TextureResizeAlgorithm resizeAlgorithm = TextureResizeAlgorithm.Bilinear;

                /// <summary>
                /// More information about texture compression - https://docs.unity3d.com/ScriptReference/TextureImporterCompression.html
                /// </summary>
                public TextureImporterCompression textureCompression = TextureImporterCompression.Compressed;

                /// <summary>
                /// When using Crunch Texture compression, use the slider to adjust the quality. A higher compression quality means larger Textures and longer compression times.
                /// </summary>
                public int compresionQuality = 100;

                /// <summary>
                /// Use crunch compression, if applicable. Crunch is a lossy compression format on top of DXT or ETC Texture compression. Textures are decompressed to DXT or ETC on the CPU and then uploaded on the GPU at runtime. Crunch compression helps the Texture use the lowest possible amount of space on disk and for downloads. Crunch Textures can take a long time to compress, but decompression at runtime is very fast.
                /// </summary>
                public bool crunchedCompression = false;

                /// <summary>
                /// Set true if you want to remove special settings for this platform
                /// </summary>
                public bool clearSettings = false;
            }
        }

        private struct TextureCase
        {
            private Texture2D texture;
            public TextureImporter textureImporter;

            public string assetPath;

            public Texture2D Texture
            {
                get
                {
                    if (texture == null)
                        texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

                    return texture;
                }
            }

            public TextureCase(Texture2D texture, TextureImporter textureImporter)
            {
                this.texture = texture;
                this.textureImporter = textureImporter;
                this.assetPath = textureImporter.assetPath;
            }
        }
    }
}