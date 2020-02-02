#ifndef TOON_LIT_INPUT_INCLUDED
#define TOON_LIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
half _BlendOffset;
half _BlendExponent;
float4 _BaseMap_ST;
half4 _BaseColor;
half4 _MidSpecColor;
half _MidShininess;
half4 _TopColor;
half4 _TopShininess;
half _TopSpecColor;
half4 _BottomColor;
half4 _BottomSpecColor;
half _BottomShininess;
half _BurnDissolveAmount;
half _BurnEdgeThiccness;
half4 _BurnEdgeColor;
half _Cutoff;
CBUFFER_END

TEXTURE2D(_TopTexture);            SAMPLER(sampler_TopTexture);
TEXTURE2D(_BottomTexture);         SAMPLER(sampler_BottomTexture);


#endif
