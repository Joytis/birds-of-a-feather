#ifndef LIGHTWEIGHT_SIMPLE_LIT_INPUT_INCLUDED
#define LIGHTWEIGHT_SIMPLE_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
half4 _BaseColor;
half4 _SpecColor;
half _Shininess;
half _BurnDissolveAmount;
half _BurnEdgeThiccness;
half4 _BurnEdgeColor;
half _Cutoff;
CBUFFER_END


#endif
