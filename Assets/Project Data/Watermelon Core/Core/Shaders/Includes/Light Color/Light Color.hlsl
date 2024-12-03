
#ifndef WAATERMELON_LIGHT_COLOR
#define WAATERMELON_LIGHT_COLOR

void LightColor_float(float3 WorldPos, out float3 Color)
{
#ifdef SHADERGRAPH_PREVIEW
	Color = float3(1,1,1);
#else
    float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);
    Color = mainLight.color;
#endif

}

void LightColor_half(half3 WorldPos, out half3 Color)
{
#ifdef SHADERGRAPH_PREVIEW
	Color = half3(1,1,1);
#else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);
    Color = mainLight.color;
#endif

}

#endif