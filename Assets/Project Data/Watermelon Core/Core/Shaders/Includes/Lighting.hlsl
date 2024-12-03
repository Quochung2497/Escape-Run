#ifndef WMELON_LIGHTING
#define WMELON_LIGHTING

half ShadowAttenHalf(half3 WorldPos)
{
	half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	Light mainLight = GetMainLight(shadowCoord);
	return mainLight.shadowAttenuation;
}

half3 LightColorHalf(half3 WorldPos)
{
	half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	Light mainLight = GetMainLight(shadowCoord);
	return mainLight.color;

}

half3 LightDirectionHalf(half3 WorldPos) {
	half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	Light mainLight = GetMainLight(shadowCoord);
	return mainLight.direction;
}


float3 ShadowAttenFloat(float3 WorldPos)
{
	float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	Light mainLight = GetMainLight(shadowCoord);
	return mainLight.shadowAttenuation;
}

float3 LightColorFloat(float3 WorldPos)
{
	float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	Light mainLight = GetMainLight(shadowCoord);
	return mainLight.color;
}

float3 LightDirectionHalf(float3 WorldPos) {
	float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	Light mainLight = GetMainLight(shadowCoord);
	return mainLight.direction;
}

#endif
