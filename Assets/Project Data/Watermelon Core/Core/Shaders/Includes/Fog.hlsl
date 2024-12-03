#ifndef WMELON_FOG
#define WMELON_FOG

half3 FogDepthLinearHalf(half fogNear, half fogFar, half3 worldPos, sampler2D fogTexture, half3 color) {
	half fogStrength = saturate((length(worldPos - _WorldSpaceCameraPos) - fogNear) / (fogFar - fogNear));

	half4 screenPos = ComputeScreenPos(TransformWorldToHClip(half4(worldPos, 0)));
	half3 fogColor = tex2D(fogTexture, screenPos.rg / screenPos.w);

	return lerp(color, fogColor, fogStrength);
}

half3 FogDepthLinearHalf(half fogNear, half fogFar, half3 worldPos, half3 fogColor, half3 color) {
	half fogStrength = saturate((length(worldPos - _WorldSpaceCameraPos) - fogNear) / (fogFar - fogNear));

	return lerp(color, fogColor, fogStrength);
}

#endif
