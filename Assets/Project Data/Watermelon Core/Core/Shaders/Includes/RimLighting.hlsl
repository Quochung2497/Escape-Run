#ifndef WMELON_RIM_LIGHTING
#define WMELON_RIM_LIGHTING

half3 RimLightingHalf(half rimMin, half rimMax, half4 rimColor, half3 viewDirection, half3 worldNormal) {
	half3 viewDir = normalize(viewDirection);
	half rimTemp = smoothstep(rimMin, rimMax, 1 - saturate(dot(viewDir, worldNormal)));

	return clamp(rimColor.rgb * rimColor.a * rimTemp, half3(0, 0, 0), half3(100, 100, 100));
}

float3 RimLightingFloat(float rimMin, float rimMax, float4 rimColor, float3 viewDirection, float3 worldNormal) {
	float3 viewDir = normalize(viewDirection);
	float rimTemp = smoothstep(rimMin, rimMax, 1 - saturate(dot(viewDir, worldNormal)));

	return clamp(rimColor.rgb * rimColor.a * rimTemp, float3(0, 0, 0), float3(100, 100, 100));
}

#endif
