#ifndef LWG_TOON_LIGHTING_INCLUDED
#define LWG_TOON_LIGHTING_INCLUDED

#include "Noise.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

half ConvertUserShinessToShininess(half shininess)
{
    return (1 - shininess);
}

half ToonCalculateAttenuation(Light light)
{
    half attenuation = light.distanceAttenuation * light.shadowAttenuation;
    return pow(abs(attenuation), 0.3);
}

half AverageColorIntensity(half3 color)
{
    return (color.r + color.g + color.b) / 3; 
}

half ToonDiffuseIntensity(half3 lightColor, half attenuation, half3 lightDir, half3 normal)
{
    half colorIntependantIntensity = clamp(saturate(dot(normal, lightDir)), 0.0, 1.0) * attenuation;
    return colorIntependantIntensity * AverageColorIntensity(lightColor);
}

half ToonSpecularIntensity(half3 lightColor, half attenuation, half3 lightDir, half3 normal, half3 viewDir, half shininess)
{
#ifndef _SPECULARHIGHLIGHTS_OFF
    half3 halfVec = SafeNormalize(lightDir + viewDir);
    half NdotH = saturate(dot(normal, halfVec));
    half intensity = pow(NdotH * attenuation, shininess * shininess);
    intensity = step(0.005, intensity);
    intensity = clamp(intensity, 0.0, 1.0);
    return intensity * AverageColorIntensity(lightColor);
#else
    return 0;
#endif
}

// These three functions return the calculated lighting intensity alongside the blending colors. 
half4 ToonDiffuseColorIntensity(half3 lightColor, half attenuation, half3 lightDir, half3 normal) 
{
    half intensity = ToonDiffuseIntensity(lightColor, attenuation, lightDir, normal);
    return half4(lightColor * intensity, intensity);
}

half4 ToonSpecularColorIntensity(half3 lightColor, half attenuation, half3 lightDir, 
                                 half3 normal, half3 viewDir, half shininess)
{
    half intensity = ToonSpecularIntensity(lightColor, attenuation, lightDir, normal, viewDir, shininess);
    return half4(lightColor * intensity, intensity);
}


half GetBurnNoise(half2 uv) 
{
    return lwg_potion_noise(uv, 20);
}

half BurnAlphaCutoffBlend(half noise, half cutoff, half dissolve) 
{
    half burnScalar = step(noise, dissolve);
    half originalScalar = 1 - burnScalar;
    return (originalScalar * cutoff) + (burnScalar * dissolve);
}

half BurnAlphaBlend(half noise, half alpha, half dissolve) 
{
    half burnScalar = step(noise, dissolve);
    half originalScalar = 1 - burnScalar;
    return (originalScalar * alpha) + (burnScalar * noise);
}

half3 BurnDiffuseBlend(half noise, half3 color, half dissolve, half edge, half3 edgeColor)
{
    half burnScalar = step(noise, dissolve + edge);
    half originalScalar = 1 - burnScalar;
    return (originalScalar * color) + (burnScalar * edgeColor);
}

half4 ToonLighting(in InputData inputData, 
    TEXTURE2D_PARAM(diffuseTex, diffuseSampler), half4 colorTint,
    half4 specColorTint, half shininess)
{
    Light mainLight = GetMainLight(inputData.shadowCoord);
    MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, half4(0, 0, 0, 0));

    half mainLightAttenuation = ToonCalculateAttenuation(mainLight);
    half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);

    half3 diffuseColor = inputData.bakedGI + mainLight.color;

    // For main light diffuse, ignore self-shadow. 
    half4 diffuseColorIntensity = half4(diffuseColor, ToonDiffuseIntensity(attenuatedLightColor,
                                                                           mainLightAttenuation,
                                                                           mainLight.direction, 
                                                                           inputData.normalWS));
    half4 specularColorIntensity = ToonSpecularColorIntensity(attenuatedLightColor, 
                                                              mainLightAttenuation,
                                                              mainLight.direction, 
                                                              inputData.normalWS, 
                                                              inputData.viewDirectionWS, 
                                                              shininess);

    half3 additionalColor = half3(0, 0, 0);

#ifdef _ADDITIONAL_LIGHTS
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, inputData.positionWS);
        half lightAttenuation = ToonCalculateAttenuation(light);
        half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);

        // Make extra intensity exponential. 
        half4 diffuseIntensity = ToonDiffuseColorIntensity(attenuatedLightColor,
                                                           lightAttenuation, 
                                                           light.direction, 
                                                           inputData.normalWS);
        diffuseIntensity.a = pow(diffuseIntensity.a, 2);
        diffuseColorIntensity += diffuseIntensity;

        // Make extra intensity exponential. 
        half4 specularIntensity = ToonSpecularColorIntensity(attenuatedLightColor,
                                                              lightAttenuation,
                                                              light.direction, 
                                                              inputData.normalWS, 
                                                              inputData.viewDirectionWS, 
                                                              shininess);
        specularIntensity.a = pow(specularIntensity.a, 2);
        specularColorIntensity += specularIntensity;

        additionalColor += diffuseIntensity.rgb + specularIntensity.rgb;
    }
#endif

    // Clamp to 'almost 1' to ensure our generated texture does not accidentally wrap 
    //   (which is likely considering its size.) 
    half virtualUvX = clamp(diffuseColorIntensity.a + specularColorIntensity.a, 0.0, 0.99);
    half2 virtualUvs = half2(virtualUvX, 0.5);

    half4 potionAlbedoAlpha = SampleAlbedoAlpha(virtualUvs, TEXTURE2D_ARGS(diffuseTex, diffuseSampler));
    half3 potionDiffuse = potionAlbedoAlpha.rgb * colorTint.rgb;

#ifdef _FIRE_DISSOLVE_ON
    half noise = GetBurnNoise(inputData.positionWS);
    half cutoff = BurnAlphaCutoffBlend(noise, _Cutoff, _BurnDissolveAmount);
    half blendedAlpha = BurnAlphaBlend(noise, potionAlbedoAlpha.a, _BurnDissolveAmount);
    half3 diffuse = BurnDiffuseBlend(noise, potionDiffuse, _BurnDissolveAmount, _BurnEdgeThiccness, _BurnEdgeColor);
#else
    half cutoff = _Cutoff;
    half blendedAlpha = potionAlbedoAlpha.a;
    half3 diffuse = potionDiffuse;
#endif

    half alpha = Alpha(blendedAlpha, colorTint, cutoff);

    diffuse += additionalColor;
    // Sample texture with constructed UVs

#ifdef _ALPHAPREMULTIPLY_ON
    diffuse *= alpha;
#endif

    // Create final color
#ifdef _ADDITIONAL_LIGHTS_VERTEX
    diffuseColorIntensity += half4(inputData.vertexLighting.rgb, 0);
#endif

    half3 finalColor = diffuse * diffuseColorIntensity.rgb; 
    finalColor += (specularColorIntensity.rgb * specColorTint.rgb);

    return half4(finalColor, alpha);
}

struct TriplanarUV 
{
    half2 x;
    half2 y;
    half2 z;
};

TriplanarUV GetTriplanarUV (in InputData inputData)
{
    TriplanarUV triUV;
    half3 p = inputData.positionWS;

    triUV.x = p.zy;
    triUV.y = p.xz;
    triUV.z = p.xy;

    if (inputData.normalWS.x < 0) {
        triUV.x.x = -triUV.x.x;
    }
    if (inputData.normalWS.y < 0) {
        triUV.y.x = -triUV.y.x;
    }
    if (inputData.normalWS.z >= 0) {
        triUV.z.x = -triUV.z.x;
    }

    triUV.x.y += 0.5;
    triUV.z.x += 0.5;
    return triUV;
}


half3 GetTriplanarWeights(in InputData inputData, half blendOffset, half blendExponent) 
{
    half topWeight = clamp(inputData.normalWS.y, 0, 1);
    half sideWeight = length(inputData.normalWS.xz);
    half bottomWeight = clamp(-inputData.normalWS.y, 0, 1);

    half3 triW = half3(topWeight, sideWeight, bottomWeight);
    triW = saturate(triW - blendOffset);
    triW = pow(triW, blendExponent);
    return triW / (triW.x + triW.y + triW.z);
}

half4 ToonTriplanarBlend(in InputData inputData, half blendOffset, half blendExponent, 
                         half4 colorTop, half4 colorSides, half4 colorBottom)
{
    half3 triW = GetTriplanarWeights(inputData, blendOffset, blendExponent);
    return colorTop * triW.x + colorSides * triW.y + colorBottom * triW.z;
}

#endif
