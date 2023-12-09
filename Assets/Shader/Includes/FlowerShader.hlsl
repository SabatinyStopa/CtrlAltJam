#include <Packages/com.blendernodesgraph.core/Editor/Includes/Importers.hlsl>

void FlowerShader_float(float3 _POS, float3 _PVS, float3 _PWS, float3 _NOS, float3 _NVS, float3 _NWS, float3 _NTS, float3 _TWS, float3 _BTWS, float3 _UV, float3 _SP, float3 _VVS, float3 _VWS, float4 colorIn, Texture2D gradient_123442, out float4 colorOut)
{
	
	float4 _Mapping_123474 = float4(mapping_point(float4(_UV, 0), float3(0, 0, 0), float3(0, 0, -9E+17), float3(10, 10, 10)), 0);
	float _GradientTexture_123490_fac; float4 _GradientTexture_123490_col; node_tex_gradient(_Mapping_123474, 0, _GradientTexture_123490_fac, _GradientTexture_123490_col);
	float4 _ColorRamp_123442 = color_ramp(gradient_123442, _GradientTexture_123490_fac);

	colorOut = _ColorRamp_123442;
}