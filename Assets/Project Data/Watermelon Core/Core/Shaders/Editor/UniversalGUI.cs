using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Watermelon.Shader
{

    public class UniversalGUI : ShaderGUI
    {

        private Dictionary<ShadowType, string> shadowKeys = new Dictionary<ShadowType, string>()
        {
            { ShadowType.None, "_SHADOWS_NONE" },
            { ShadowType.Vertex, "_SHADOWS_VERTEX" },
            { ShadowType.Pixel, "_SHADOWS_PIXEL" },
        };

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            // Custom code that controls the appearance of the Inspector goes here

            //base.OnGUI(materialEditor, properties);

            Material material = materialEditor.target as Material;

            var _Transparent = FindProperty("_Transparent", properties);

            var _Color = FindProperty("_Color", properties);
            var _MainTex = FindProperty("_MainTex", properties);

            var _Shadows = FindProperty("_Shadows", properties);
            var _SColor = FindProperty("_SColor", properties);

            var _EmissionOn = FindProperty("_EmissionOn", properties);
            var _Emission = FindProperty("_EmissionColor", properties);
            var _EmissionTex = FindProperty("_EmissionTex", properties);

            var _RimOn = FindProperty("_RimOn", properties);
            var _RimColor = FindProperty("_RimColor", properties);
            var _RimMin = FindProperty("_RimMin", properties);
            var _RimMax = FindProperty("_RimMax", properties);

            var _LightColorOn = FindProperty("_LightColorOn", properties);

            var _ToonOn = FindProperty("_ToonOn", properties);
            var _RampTex = FindProperty("_RampTex", properties);
            var _RampMin = FindProperty("_RampMin", properties);
            var _RampMax = FindProperty("_RampMax", properties);


            var headerStyle = new GUIStyle
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState
                {
                    textColor = new Color32(194, 194, 194, 255),
                }
            };

            var middleStyle = new GUIStyle
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState
                {
                    textColor = new Color32(194, 194, 194, 255),
                }
            };

            var minStyle = new GUIStyle
            {
                fontSize = 10,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState
                {
                    textColor = new Color32(194, 194, 194, 255),
                }
            };


            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent("Opacity:", 
                "Uses LOD to swap subshaders\n" +
                "'Opaque'        = 400\n" +
                "'Transparent' = 200"
                ), middleStyle);

            GUILayout.FlexibleSpace();

            var opacity = (OpacityType)_Transparent.floatValue;

            opacity = (OpacityType)EditorGUILayout.EnumPopup(opacity, GUILayout.Width(120));

            _Transparent.floatValue = (int)opacity;

            material.shader.maximumLOD = opacity == OpacityType.Transparent ? 200 : 400;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Color:", 
                "Color    = '_Color'\n" +
                "Texture = '_MainTex'"
                ), middleStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            _MainTex.textureValue = (Texture)EditorGUILayout.ObjectField(_MainTex.textureValue, typeof(Texture), false, GUILayout.Height(50), GUILayout.Width(50));
            _Color.colorValue = EditorGUILayout.ColorField(_Color.colorValue, GUILayout.Height(50), GUILayout.Width(65));
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(
                "Shadows:", "Uses keywords:\n" +
                "'None'   = '_SHADOWS_NONE' (Fastest)\n" +
                "'Vertex' = '_SHADOWS_VERTEX' (Average)\n" +
                "'Pixel'    = '_SHADOWS_PIXEL' (Slowest)\n\n" +
                "Shadow Color = '_SColor'"
                ), middleStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginVertical();

            var shadowType = (ShadowType)_Shadows.floatValue;

            shadowType = (ShadowType)EditorGUILayout.EnumPopup(shadowType, GUILayout.Width(120));

            _Shadows.floatValue = (int)shadowType;

            foreach(var key in shadowKeys.Keys)
            {
                if(key == shadowType)
                {
                    material.EnableKeyword(shadowKeys[key]);
                } else
                {
                    material.DisableKeyword(shadowKeys[key]);
                }
            }

            if(shadowType != ShadowType.None)
            {
                _SColor.colorValue = EditorGUILayout.ColorField(_SColor.colorValue, GUILayout.Width(120));
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Emission:",
                "Uses keyword 'EMISSION_ON'\n" +
                "Texture = '_EmissionTex'\n" +
                "Color     = '_EmissionColor'"
                ), middleStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginVertical();

            var isEmissionOn = (Switch)_EmissionOn.floatValue;
            isEmissionOn = (Switch)EditorGUILayout.EnumPopup(isEmissionOn, GUILayout.Width(120));
            if (isEmissionOn == Switch.On)
            {
                material.EnableKeyword("EMISSION_ON");
            }
            else
            {
                material.DisableKeyword("EMISSION_ON");
            }

            _EmissionOn.floatValue = (int)isEmissionOn;


            if (isEmissionOn == Switch.On)
            {
                EditorGUILayout.BeginHorizontal();

                _EmissionTex.textureValue = (Texture)EditorGUILayout.ObjectField(_EmissionTex.textureValue, typeof(Texture), false, GUILayout.Height(50), GUILayout.Width(50));

                _Emission.colorValue = EditorGUILayout.ColorField(GUIContent.none, _Emission.colorValue, true, false, true, GUILayout.Width(65), GUILayout.Height(50));

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();



            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Rim:",
                "Uses keyword 'RIM_ON'\n" +
                "Max   = '_RimMax'\n" +
                "Min    = '_RimMin'\n" +
                "Color = '_RimColor'"
                ), middleStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            var isRimOn = (Switch)_RimOn.floatValue;

            isRimOn = (Switch)EditorGUILayout.EnumPopup(isRimOn, GUILayout.Width(120));
            _RimOn.floatValue = (int)isRimOn;

            EditorGUILayout.EndHorizontal();

            if (isRimOn == Switch.On)
            {
                material.EnableKeyword("RIM_ON");
            }
            else
            {
                material.DisableKeyword("RIM_ON");
            }

            if (isRimOn == Switch.On)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Max:", minStyle);
                _RimMax.floatValue = EditorGUILayout.Slider(_RimMax.floatValue, 0, 1, GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();

                GUILayout.FlexibleSpace();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Min:", minStyle);
                _RimMin.floatValue = EditorGUILayout.Slider(_RimMin.floatValue, 0, 1, GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                _RimColor.colorValue = EditorGUILayout.ColorField(_RimColor.colorValue, GUILayout.Width(65), GUILayout.Height(50));

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            

            

            EditorGUILayout.EndVertical();

            

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Toon:",
                "Uses keyword 'TOON_ON'\n" +
                "Max       = '_RampMax'\n" +
                "Min        = '_RampMin'\n" +
                "Texture = '_RampTex'"
                ), middleStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            var isToonOn = (Switch)_ToonOn.floatValue;
            isToonOn = (Switch)EditorGUILayout.EnumPopup(isToonOn, GUILayout.Width(120));
            _ToonOn.floatValue = (int)isToonOn;

            EditorGUILayout.EndHorizontal();

            if (isToonOn == Switch.On)
            {
                material.EnableKeyword("TOON_ON");
            }
            else
            {
                material.DisableKeyword("TOON_ON");
            }

            if (isToonOn == Switch.On)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Max:", minStyle);
                _RampMax.colorValue = EditorGUILayout.ColorField(_RampMax.colorValue, GUILayout.Width(65));
                EditorGUILayout.EndHorizontal();

                GUILayout.FlexibleSpace();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Min:", minStyle);
                _RampMin.colorValue = EditorGUILayout.ColorField(_RampMin.colorValue, GUILayout.Width(65));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();

                _RampTex.textureValue = (Texture)EditorGUILayout.ObjectField(_RampTex.textureValue, typeof(Texture), false, GUILayout.Height(50), GUILayout.Width(50));

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Light Color:",
                "Uses keyword 'LIGHT_COLOR_ON'"
                ), middleStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();


            var isLightColor = (Switch)_LightColorOn.floatValue;
            isLightColor = (Switch)EditorGUILayout.EnumPopup(isLightColor, GUILayout.Width(120));
            _LightColorOn.floatValue = (int)isLightColor;


            if (isLightColor == Switch.On)
            {
                material.EnableKeyword("LIGHT_COLOR_ON");
            }
            else
            {
                material.DisableKeyword("LIGHT_COLOR_ON");
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


            EditorGUILayout.BeginHorizontal(GUILayout.Width(90));
            {
                material.renderQueue = EditorGUILayout.IntField(material.renderQueue);
                GUILayout.Label("Render Queue");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(150));
            {
                material.enableInstancing = EditorGUILayout.Toggle(material.enableInstancing, GUILayout.ExpandWidth(false));
                GUILayout.Label("Enable GPU Instancing", GUILayout.ExpandWidth(false));

                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();

            //_Emission.colorValue = EditorGUILayout.ColorField(GUIContent.none, _Emission.colorValue, true, false, true );

            //GUILayout.Label("Main", headerStyle);
            //var transparent = EditorGUILayout.Toggle(property.floatValue == 1);

            //property.floatValue = transparent ? 1 : 0;


        }
    }

    public enum Switch
    {
        Off = 0,
        On = 1
    }

    public enum OpacityType
    {
        Opaque = 0,
        Transparent = 1
    }

    public enum ShadowType
    {
        None = 0,
        Vertex = 1,
        Pixel = 2,
    }
}