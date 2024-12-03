using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Watermelon.Shader
{
    public class RampLitTransparentGUI : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material targetMat = materialEditor.target as Material;

            var _Color = FindProperty("_Color", properties);
            var _HighlightColor = FindProperty("_HColor", properties);
            var _ShadowColor = FindProperty("_SColor", properties);

            var _RimColor = FindProperty("_RimColor", properties);
            var _RimMin = FindProperty("_RimMin", properties);
            var _RimMax = FindProperty("_RimMax", properties);

            var _Metallic = FindProperty("_Metallic", properties);
            var _Smoothness = FindProperty("_Smoothness", properties);
            var _AmbientOcclusion = FindProperty("_AmbientOcclusion", properties);

            var _Alpha = FindProperty("_Alpha", properties);

            var _MainTex = FindProperty("_MainTex", properties);
            var _Ramp = FindProperty("_Ramp", properties);

            var _ReceiveShadows = FindProperty("_Receive_Shadows", properties);
            var _CustomAlpha = FindProperty("_Custom_Alpha", properties);

            List<MaterialProperty> usedProperties = new List<MaterialProperty>();

            usedProperties.Add(_Color);
            usedProperties.Add(_HighlightColor);
            usedProperties.Add(_ShadowColor);

            usedProperties.Add(_RimColor);
            usedProperties.Add(_RimMin);
            usedProperties.Add(_RimMax);

            usedProperties.Add(_Metallic);
            usedProperties.Add(_Smoothness);
            usedProperties.Add(_AmbientOcclusion);

            usedProperties.Add(_Alpha);

            usedProperties.Add(_MainTex);
            usedProperties.Add(_Ramp);

            usedProperties.Add(_ReceiveShadows);
            usedProperties.Add(_CustomAlpha);

            usedProperties.Add(FindProperty("unity_Lightmaps", properties));
            usedProperties.Add(FindProperty("unity_LightmapsInd", properties));
            usedProperties.Add(FindProperty("unity_ShadowMasks", properties));


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

                    EditorGUILayout.BeginHorizontal();
                    {
                        _HighlightColor.colorValue = EditorGUILayout.ColorField(_HighlightColor.colorValue, GUILayout.Width(70));
                        GUILayout.Label("Highlight Color");
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

            GUILayout.Label("Transparency", headerStyle);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
            {
                _CustomAlpha.floatValue = EditorGUILayout.Toggle(_CustomAlpha.floatValue == 1, GUILayout.ExpandWidth(false)) ? 1 : 0;
                GUILayout.Label("Custom Alpha", GUILayout.ExpandWidth(false));

                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            if(_CustomAlpha.floatValue == 1)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    _Alpha.floatValue = EditorGUILayout.Slider(_Alpha.floatValue, 0, 1, GUILayout.Width(170));
                    GUILayout.Label("Alpha");
                }
                EditorGUILayout.EndHorizontal();
            }
            

            GUILayout.Space(20);

            GUILayout.Label("Rim", headerStyle);

            EditorGUILayout.BeginHorizontal();
            {
                var texture = (Texture)EditorGUILayout.ObjectField(_Ramp.textureValue, typeof(Texture), false, GUILayout.Width(70), GUILayout.Height(70));

                _Ramp.textureValue = texture;

                GUILayout.Space(20);

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        _RimColor.colorValue = EditorGUILayout.ColorField(_Color.colorValue, GUILayout.Width(70));
                        GUILayout.Label("Rim Color");
                    }
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    EditorGUILayout.BeginHorizontal();
                    {
                        _RimMin.floatValue = EditorGUILayout.Slider(_RimMin.floatValue, 0, 1, GUILayout.Width(170));
                        GUILayout.Label("Min");
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        _RimMax.floatValue = EditorGUILayout.Slider(_RimMax.floatValue, 0, 1, GUILayout.Width(170));
                        GUILayout.Label("Max");
                    }
                    EditorGUILayout.EndHorizontal();

                }
                EditorGUILayout.EndVertical();
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

            for(int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];

                if (!usedProperties.Contains(property))
                {
                    materialEditor.DefaultShaderProperty(property, property.displayName);
                    //materialEditor.FloatProperty(properties[i], properties[i].displayName);
                }
            }

            

            
        }
    }
}

