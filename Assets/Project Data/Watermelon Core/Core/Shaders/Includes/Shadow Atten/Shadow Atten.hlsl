#ifndef WAATERMELON_SHADOW_ATTEN
#define WAATERMELON_SHADOW_ATTEN


void ShadowAtten_float(float3 WorldPos, out float ShadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
	ShadowAtten = 1;
#else
    float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);
    ShadowAtten = mainLight.shadowAttenuation;
#endif

}

void ShadowAtten_half(half3 WorldPos, out half ShadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
	ShadowAtten = 1;
#else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);
    ShadowAtten = mainLight.shadowAttenuation;
#endif

}

#endif