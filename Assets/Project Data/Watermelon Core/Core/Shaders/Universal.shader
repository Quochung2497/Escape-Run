Shader "WMelon/Universal"
{
    Properties
    {
        [HideInInspector]_Transparent("Transparent", float) = 0

        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        
        //[Toggle(RECEIVE_SHADOWS_ON)]_ReceiveShadowsOn("Receive Shadows", float) = 1
        [KeywordEnum(None, Vertex, Pixel)] _Shadows("Receive Shadows", Float) = 2
        _SColor("Shadow Color", Color) = (0.5,0.5,0.5,1)

        [Toggle(EMISSION_ON)]_EmissionOn("Emission", float) = 0
        [HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
        _EmissionTex("Emission Texture", 2D) = "white" {}

        [Toggle(RIM_ON)]_RimOn("Rim", float) = 0
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimMin("Rim Min", range(0, 1)) = 0.5
        _RimMax("Rim Max", range(0, 1)) = 1
        
        [Toggle(LIGHT_COLOR_ON)]_LightColorOn("Uses Light Color", float) = 0

        [Toggle(TOON_ON)]_ToonOn("Toon", float) = 0

        _RampTex("Texture", 2D) = "white" {}
        _RampMin("Ramp Min", Color) = (0,0,0,1)
        _RampMax("Ramp Max", Color) = (1,1,1,1)
    }

    SubShader
    {
        Cull back
        Tags { 
            "RenderType" = "Opaque" 
    
            "RenderPipeline" = "UniversalPipeline"
            "LightMode" = "UniversalForward"

            "Queue" = "AlphaTest"
            "RenderType" = "TransparentCutout"
            "IgnoreProjector" = "True"
        }
        
        LOD 400

        Pass
        {
            Name "MainPass"
            Cull Back


            HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

#if !defined(_SHADOWS_NONE) || defined(LIGHT_COLOR_ON) || defined(TOON_ON)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Includes/Lighting.hlsl"
#endif


#ifndef _SHADOWS_NONE
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _SHADOWS_SOFT
#endif
            
#ifdef RIM_ON
            #include "Includes/RimLighting.hlsl"
#endif
            //#include "Includes/Fog.hlsl"

            #pragma shader_feature_local EMISSION_ON
            #pragma shader_feature_local RIM_ON
            #pragma shader_feature_local LIGHT_COLOR_ON
            #pragma shader_feature_local TOON_ON
            #pragma shader_feature_local _SHADOWS_NONE _SHADOWS_VERTEX _SHADOWS_PIXEL

            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
#if defined(RIM_ON) || defined(TOON_ON) 
                float3 norm :NORMAL;
                float4 tan :TANGENT;
#endif
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;

#if !defined(_SHADOWS_NONE) || defined(LIGHT_COLOR_ON) || defined(TOON_ON) || defined(RIM_ON)
                float4 worldPos: TEXCOORD1;
#endif

#if defined(TOON_ON) || defined(RIM_ON)
                float3 normWorld : TEXCOORD2;
#endif

#ifdef RIM_ON
                float3 viewDir : TEXCOORD3;
#endif
            };

            half4 _Color;
            sampler2D _MainTex;

#ifdef EMISSION_ON
            half4 _EmissionColor;
            sampler2D _EmissionTex;
#endif

#ifndef _SHADOWS_NONE
            half4 _SColor;
#endif

#ifdef RIM_ON
            half4 _RimColor;
            half _RimMin;
            half _RimMax;
#endif

#ifdef TOON_ON
            sampler2D _RampTex;
            half4 _RampMin;
            half4 _RampMax;
#endif

            v2f vert(appdata input) 
            {
                v2f output;

#if !defined(_SHADOWS_NONE) || defined(LIGHT_COLOR_ON) || defined(TOON_ON) || defined(RIM_ON)
                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.pos.xyz);
                output.worldPos = float4(positionInputs.positionWS, 1);

    #ifdef _SHADOWS_VERTEX
                output.worldPos.w = ShadowAttenHalf(positionInputs.positionWS);
    #endif
#endif 

#if defined(RIM_ON) || defined(TOON_ON) 
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.norm, input.tan);
                output.normWorld = normalInputs.normalWS;

    #ifdef RIM_ON
                output.viewDir = GetWorldSpaceViewDir(output.worldPos.xyz);
    #endif
#endif

                output.pos = TransformObjectToHClip(input.pos.xyz);
                output.uv = input.uv;

                return output;
            }


            half Remap(half In, half2 InMinMax, half2 OutMinMax)
            {
                return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }


            float4 frag(v2f input) : COLOR
            {
                half4 color = tex2D(_MainTex, input.uv) * _Color;
#ifdef LIGHT_COLOR_ON
                color *= half4(LightColorHalf(input.worldPos.xyz), 1);
#endif

#ifdef TOON_ON
                half3 lightDir = LightDirectionHalf(input.worldPos.xyz);
                half tempToon = Remap(dot(input.normWorld, lightDir), half2(-1, 1), half2(0, 1));

                half3 rampTemp =  lerp(_RampMin, _RampMax, tempToon).rgb;
                half3 rampTextTemp = tex2D(_RampTex, half2(tempToon, 0.5)).rgb;

                half3 toon = rampTemp * rampTextTemp;

                color *= half4(toon, 1);
#endif

#ifdef RIM_ON
                color += half4(RimLightingHalf(_RimMin, _RimMax, _RimColor, input.viewDir, input.normWorld), 1);
#endif

#ifndef _SHADOWS_NONE

#ifdef _SHADOWS_PIXEL
                half shadowAtten = ShadowAttenHalf(input.worldPos.rgb);
#elif _SHADOWS_VERTEX
                half shadowAtten = input.worldPos.w;
#endif
                half4 shadowTemp = half4(lerp(_Color.rgb, _SColor.rgb, _SColor.a), 1);
                shadowTemp = lerp(shadowTemp, _Color, shadowAtten);

                color *= shadowTemp;
#endif

#ifdef EMISSION_ON
                color += tex2D(_EmissionTex, input.uv) * _EmissionColor;
#endif

                return color;
            }

            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            Cull[_Cull]

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature _ALPHATEST_ON

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"

            ENDHLSL
        }
    }

        SubShader
        {
            Cull back
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

            LOD 200

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                Name "MainPass"
                Cull Back

                HLSLPROGRAM


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

#if !defined(_SHADOWS_NONE) || defined(LIGHT_COLOR_ON) || defined(TOON_ON)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Includes/Lighting.hlsl"
#endif


#ifndef _SHADOWS_NONE
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _SHADOWS_SOFT
#endif

#ifdef RIM_ON
            #include "Includes/RimLighting.hlsl"
#endif
            //#include "Includes/Fog.hlsl"

            #pragma shader_feature_local EMISSION_ON
            #pragma shader_feature_local RIM_ON
            #pragma shader_feature_local LIGHT_COLOR_ON
            #pragma shader_feature_local TOON_ON
            #pragma multi_compile_local _SHADOWS_NONE _SHADOWS_VERTEX _SHADOWS_PIXEL

            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
#if defined(RIM_ON) || defined(TOON_ON) 
                float3 norm :NORMAL;
                float4 tan :TANGENT;
#endif
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;

#if !defined(_SHADOWS_NONE) || defined(LIGHT_COLOR_ON) || defined(TOON_ON) || defined(RIM_ON)
                float4 worldPos: TEXCOORD1;
#endif

#if defined(TOON_ON) || defined(RIM_ON)
                float3 normWorld : TEXCOORD4;
#endif

#ifdef RIM_ON
                float3 viewDir : TEXCOORD2;
#endif
            };

            half4 _Color;
            sampler2D _MainTex;

#ifdef EMISSION_ON
            half4 _EmissionColor;
            sampler2D _EmissionTex;
#endif

#ifndef _SHADOWS_NONE
            half4 _SColor;
#endif

#ifdef RIM_ON
            half4 _RimColor;
            half _RimMin;
            half _RimMax;
#endif

#ifdef TOON_ON
            sampler2D _RampTex;
            half4 _RampMin;
            half4 _RampMax;
#endif

            v2f vert(appdata input)
            {
                v2f output;

#if !defined(_SHADOWS_NONE) || defined(LIGHT_COLOR_ON) || defined(TOON_ON) || defined(RIM_ON)
                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.pos.xyz);
                output.worldPos = float4(positionInputs.positionWS, 1);

    #ifdef _SHADOWS_VERTEX
                output.worldPos.w = ShadowAttenHalf(positionInputs.positionWS);
    #endif
#endif 

#if defined(RIM_ON) || defined(TOON_ON) 
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.norm, input.tan);
                output.normWorld = normalInputs.normalWS;

    #ifdef RIM_ON
                output.viewDir = GetWorldSpaceViewDir(output.worldPos);
    #endif
#endif

                output.pos = TransformObjectToHClip(input.pos.xyz);
                output.uv = input.uv;

                return output;
            }


            half Remap(half In, half2 InMinMax, half2 OutMinMax)
            {
                return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }


            float4 frag(v2f input) : COLOR
            {
                half4 color = tex2D(_MainTex, input.uv) * _Color;
                half a = color.w;

#ifdef LIGHT_COLOR_ON
                color *= half4(LightColorHalf(input.worldPos), 1);
#endif

#ifdef TOON_ON
                half3 lightDir = LightDirectionHalf(input.worldPos);
                half tempToon = Remap(dot(input.normWorld, lightDir), half2(-1, 1), half2(0, 1));

                half3 rampTemp = lerp(_RampMin, _RampMax, tempToon).rgb;
                half3 rampTextTemp = tex2D(_RampTex, half2(tempToon, 0.5));

                half3 toon = rampTemp * rampTextTemp;

                color *= half4(toon, 1);
#endif

#ifdef RIM_ON
                color += half4(RimLightingHalf(_RimMin, _RimMax, _RimColor, input.viewDir, input.normWorld), 1);
#endif

#ifndef _SHADOWS_NONE

#ifdef _SHADOWS_PIXEL
                half shadowAtten = ShadowAttenHalf(input.worldPos.rgb);
#elif _SHADOWS_VERTEX
                half shadowAtten = input.worldPos.w;
#endif
                half4 shadowTemp = half4(lerp(_Color.rgb, _SColor.rgb, _SColor.a), 1);
                shadowTemp = lerp(shadowTemp, _Color, shadowAtten);

                color *= shadowTemp;
#endif

#ifdef EMISSION_ON
                color += tex2D(_EmissionTex, input.uv) * _EmissionColor;
#endif

                return half4(color.rgb, a);
            }

            ENDHLSL
        }
    }

    CustomEditor "Watermelon.Shader.UniversalGUI"
}
