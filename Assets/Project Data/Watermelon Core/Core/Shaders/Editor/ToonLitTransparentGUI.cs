using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Watermelon.Shader
{
    public class ToonLitTransparentGUI : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material targetMat = materialEditor.target as Material;

            var _Color = FindProperty("_Color", properties);
            var _MainTex = FindProperty("_MainTex", properties);

            var _AlbedoTex = FindProperty("_AlbedoTex", properties);
            var _Albedo = FindProperty("_Albedo", properties);
            var _AlbedoColor = FindProperty("_Albedo_Color", properties);

            var _ReceiveShadows = FindProperty("_Receive_Shadows", properties);
            var _ShadowColor = FindProperty("_SColor", properties);

            var _Use_Ramp_Texture = FindProperty("_Use_Ramp_Texture", properties);
            var _Ramp = FindProperty("_Ramp", properties);
            var _RampMin = FindProperty("_RampMin", properties);
            var _RampMax = FindProperty("_RampMax", properties);

            var _Metallic = FindProperty("_Metallic", properties);
            var _Smoothness = FindProperty("_Smoothness", properties);
            var _AmbientOcclusion = FindProperty("_Occlusion", properties);

            var _CustomAlpha = FindProperty("_Custom_Alpha", properties);
            var _Alpha = FindProperty("_Alpha", properties);
            var _Alpha_Clip = FindProperty("_Alpha_Clip", properties);

            var headerStyle = new GUIStyle
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState
                {
                    textColor = new Color32(194, 194, 194, 255),
                }
            };

            bool receiveShadows = _ReceiveShadows.floatValue == 1;
            bool useAlbedo = _Albedo.floatValue == 1;
            bool useRampTexture = _Use_Ramp_Texture.floatValue == 1;

            GUILayout.Label("Main", headerStyle);

            EditorGUILayout.BeginHorizontal();
            {
                var texture = (Texture)EditorGUILayout.ObjectField(_MainTex.textureValue, typeof(Texture), false, GUILayout.Width(70), GUILayout.Height(70));

                _MainTex.textureValue = texture;

                GUILayout.Space(20);

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        _Color.colorValue = EditorGUILayout.ColorField(_Color.colorValue, GUILayout.Width(70));
                        GUILayout.Label("Color");
                    }
                    EditorGUILayout.EndHorizontal();

                    if (receiveShadows)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            _ShadowColor.colorValue = EditorGUILayout.ColorField(_ShadowColor.colorValue, GUILayout.Width(70));
                            GUILayout.Label("Shadow Color");
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(120));
            {
                _ReceiveShadows.floatValue = EditorGUILayout.Toggle(_ReceiveShadows.floatValue == 1, GUILayout.ExpandWidth(false)) ? 1 : 0;
                GUILayout.Label("Receive Shadows", GUILayout.ExpandWidth(false));

                GUILayout.FlexibleSpace();

            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.Label("Albedo", headerStyle);

            if (useAlbedo)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    var texture = (Texture)EditorGUILayout.ObjectField(_AlbedoTex.textureValue, typeof(Texture), false, GUILayout.Width(70), GUILayout.Height(70));

                    _AlbedoTex.textureValue = texture;

                    GUILayout.Space(20);

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            _AlbedoColor.colorValue = EditorGUILayout.ColorField(_AlbedoColor.colorValue, GUILayout.Width(70));
                            GUILayout.Label("Albedo Color");
                        }
                        EditorGUILayout.EndHorizontal();

                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Width(90));
            {
                _Albedo.floatValue = EditorGUILayout.Toggle(_Albedo.floatValue == 1, GUILayout.ExpandWidth(false)) ? 1 : 0;
                GUILayout.Label("Use Albedo", GUILayout.ExpandWidth(false));

                GUILayout.FlexibleSpace();

            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.Label("Transparency", headerStyle);

            EditorGUILayout.BeginHorizontal();
            {
                _Alpha_Clip.floatValue = EditorGUILayout.Slider(_Alpha_Clip.floatValue, 0, 1, GUILayout.Width(170));
                GUILayout.Label("Alpha Clipping");
            }
            EditorGUILayout.EndHorizontal();

            if (_CustomAlpha.floatValue == 1)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    _Alpha.floatValue = EditorGUILayout.Slider(_Alpha.floatValue, 0, 1, GUILayout.Width(170));
                    GUILayout.Label("Alpha");
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
            {
                _CustomAlpha.floatValue = EditorGUILayout.Toggle(_CustomAlpha.floatValue == 1, GUILayout.ExpandWidth(false)) ? 1 : 0;
                GUILayout.Label("Custom Alpha", GUILayout.ExpandWidth(false));

                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.Label("Ramp", headerStyle);

            if (useRampTexture)
            {
                var texture = (Texture)EditorGUILayout.ObjectField(_Ramp.textureValue, typeof(Texture), false, GUILayout.Width(70), GUILayout.Height(70));

                _Ramp.textureValue = texture;
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                {
                    _RampMin.colorValue = EditorGUILayout.ColorField(_RampMin.colorValue, GUILayout.Width(70));
                    GUILayout.Label("Ramp Min");
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    _RampMax.colorValue = EditorGUILayout.ColorField(_RampMax.colorValue, GUILayout.Width(70));
                    GUILayout.Label("Ramp Max");
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Width(120));
            {
                _Use_Ramp_Texture.floatValue = EditorGUILayout.Toggle(_Use_Ramp_Texture.floatValue == 1, GUILayout.ExpandWidth(false)) ? 1 : 0;
                GUILayout.Label("Use Ramp Texture", GUILayout.ExpandWidth(false));

                GUILayout.FlexibleSpace();

            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.Label("Additional", headerStyle);

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    _Metallic.floatValue = EditorGUILayout.Slider(_Metallic.floatValue, 0, 1, GUILayout.Width(170));
                    GUILayout.Label("Metallic");
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    _Smoothness.floatValue = EditorGUILayout.Slider(_Smoothness.floatValue, 0, 1, GUILayout.Width(170));
                    GUILayout.Label("Smoothness");
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    _AmbientOcclusion.floatValue = EditorGUILayout.Slider(_AmbientOcclusion.floatValue, 0, 1, GUILayout.Width(170));
                    GUILayout.Label("Ambient Occlusion");
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(90));
                {
                    targetMat.renderQueue = EditorGUILayout.IntField(targetMat.renderQueue);
                    GUILayout.Label("Render Queue");
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
                {
                    targetMat.enableInstancing = EditorGUILayout.Toggle(targetMat.enableInstancing, GUILayout.ExpandWidth(false));
                    GUILayout.Label("Enable GPU Instancing", GUILayout.ExpandWidth(false));

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();

            }
            EditorGUILayout.EndVertical();
        }
    }
}


