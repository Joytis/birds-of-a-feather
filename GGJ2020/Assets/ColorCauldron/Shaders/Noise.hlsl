#ifndef LWG_POTION_NOISE_INCLUDED
#define LWG_POTION_NOISE_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

inline half lwg_potion_noise_interpolate(half a, half b, half t)
{
    return (1.0-t)*a + (t*b);
}

inline half lwg_potion_noise_randomValue(half2 uv)
{
    return frac(sin(dot(uv, half2(12.9898, 78.233)))*43758.5453);
}

inline half lwg_potion_valueNoise(half2 uv)
{
    half2 i = floor(uv);
    half2 f = frac(uv);
    f = f * f * (3.0 - 2.0 * f);

    uv = abs(frac(uv) - 0.5);
    half2 c0 = i + half2(0.0, 0.0);
    half2 c1 = i + half2(1.0, 0.0);
    half2 c2 = i + half2(0.0, 1.0);
    half2 c3 = i + half2(1.0, 1.0);
    half r0 = lwg_potion_noise_randomValue(c0);
    half r1 = lwg_potion_noise_randomValue(c1);
    half r2 = lwg_potion_noise_randomValue(c2);
    half r3 = lwg_potion_noise_randomValue(c3);

    half bottomOfGrid = lwg_potion_noise_interpolate(r0, r1, f.x);
    half topOfGrid = lwg_potion_noise_interpolate(r2, r3, f.x);
    half t = lwg_potion_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
    return t;
}

inline half lwg_potion_noise(half2 uv, half scale)
{
    half t = 0.0;

    half freq = pow(2.0, half(0));
    half amp = pow(0.5, half(3-0));
    t += lwg_potion_valueNoise(half2(uv.x*scale/freq, uv.y*scale/freq))*amp;

    freq = pow(2.0, half(1));
    amp = pow(0.5, half(3-1));
    t += lwg_potion_valueNoise(half2(uv.x*scale/freq, uv.y*scale/freq))*amp;

    freq = pow(2.0, half(2));
    amp = pow(0.5, half(3-2));
    t += lwg_potion_valueNoise(half2(uv.x*scale/freq, uv.y*scale/freq))*amp;

    return t;
}

#endif