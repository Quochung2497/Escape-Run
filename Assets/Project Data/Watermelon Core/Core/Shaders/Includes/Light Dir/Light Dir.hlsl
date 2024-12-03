
#ifndef WAATERMELON_LIGHT_DIR
#define WAATERMELON_LIGHT_DIR

void LightDirection_float(float3 WorldPos, out float3 LightDir)
{
#ifdef SHADERGRAPH_PREVIEW
	LightDir = float3(-0.5, -0.5, 0.5);
#else
	float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	Light mainLight = GetMainLight(shadowCoord);
	LightDir = mainLight.direction;
#endif
}

void LightDirection_half(half3 WorldPos, out half3 LightDir)
{
#ifdef SHADERGRAPH_PREVIEW
	LightDir = half3(-0.5, -0.5, 0.5);
#else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);
    LightDir = mainLight.direction;
#endif
}

#endif