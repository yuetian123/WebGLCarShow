//URP Car Paint

#ifndef UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
#define UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
  
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

// GLES2 has limited amount of interpolators
#if defined(_PARALLAXMAP) && !defined(SHADER_API_GLES)
#define REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR
#endif

#if (defined(_NORMALMAP) || (defined(_PARALLAXMAP) && !defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR))) || defined(_DETAIL)
#define REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR
#endif

// keep this file in sync with LitGBufferPass.hlsl

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float4 tangentOS    : TANGENT;
    float2 texcoord     : TEXCOORD0;
    float2 staticLightmapUV   : TEXCOORD1;
    float2 dynamicLightmapUV  : TEXCOORD2;
    float2 texcoord1    : TEXCOORD3;
    float2 texcoord2    : TEXCOORD4;
    float2 texcoord3    : TEXCOORD5;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv                       : TEXCOORD0;

#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
    float3 positionWS               : TEXCOORD1;
#endif

    half3 normalWS                 : TEXCOORD2;
#if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
    half4 tangentWS                : TEXCOORD3;    // xyz: tangent, w: sign
#endif
    float3 viewDirWS                : TEXCOORD4;

#ifdef _ADDITIONAL_LIGHTS_VERTEX
    half4 fogFactorAndVertexLight   : TEXCOORD5; // x: fogFactor, yzw: vertex light
#else
    half  fogFactor                 : TEXCOORD5;
#endif

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4 shadowCoord              : TEXCOORD6;
#endif

#if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
    half3 viewDirTS                : TEXCOORD7;
#endif

    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 8);
#ifdef DYNAMICLIGHTMAP_ON
    float2  dynamicLightmapUV : TEXCOORD9; // Dynamic lightmap UVs
#endif

    float3 objectPos                : TEXCOORD10;
    float3 objectPosWSRotated       : TEXCOORD11; //our rotated WS object position to sample cube

    //for other UV's
    float2 uv1                       : TEXCOORD12;
    float2 uv2                       : TEXCOORD13;
    float2 uv3                       : TEXCOORD14;

    float4 positionCS               : SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
{
    inputData = (InputData)0;

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
        inputData.positionWS = input.positionWS;
    #endif

    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
#if defined(_NORMALMAP) || defined(_DETAIL)
    float sgn = input.tangentWS.w;      // should be either +1 or -1
    float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
    half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);

    #if defined(_NORMALMAP)
    inputData.tangentToWorld = tangentToWorld;
    #endif
    inputData.normalWS = TransformTangentToWorld(normalTS, tangentToWorld);
#else
    inputData.normalWS = input.normalWS;
#endif

    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
    inputData.viewDirectionWS = viewDirWS;

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    inputData.shadowCoord = input.shadowCoord;
#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
    inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
#else
    inputData.shadowCoord = float4(0, 0, 0, 0);
#endif
#ifdef _ADDITIONAL_LIGHTS_VERTEX
    inputData.fogCoord = InitializeInputDataFog(float4(input.positionWS, 1.0), input.fogFactorAndVertexLight.x);
    inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
#else
    inputData.fogCoord = InitializeInputDataFog(float4(input.positionWS, 1.0), input.fogFactor);
#endif

#if defined(DYNAMICLIGHTMAP_ON)
    inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.dynamicLightmapUV, input.vertexSH, inputData.normalWS);
#else
    inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.vertexSH, inputData.normalWS);
#endif

    inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
    inputData.shadowMask = SAMPLE_SHADOWMASK(input.staticLightmapUV);

    #if defined(DEBUG_DISPLAY)
    #if defined(DYNAMICLIGHTMAP_ON)
    inputData.dynamicLightmapUV = input.dynamicLightmapUV;
    #endif
    #if defined(LIGHTMAP_ON)
    inputData.staticLightmapUV = input.staticLightmapUV;
    #else
    inputData.vertexSH = input.vertexSH;
    #endif
    #endif
}

void Unity_RotateAboutAxis_Degrees_float(float3 In, float3 Axis, float Rotation, out float3 Out)
{
    Rotation = radians(Rotation);
    float s = sin(Rotation);
    float c = cos(Rotation);
    float one_minus_c = 1.0 - c;

    Axis = normalize(Axis);
    float3x3 rot_mat =
    { one_minus_c * Axis.x * Axis.x + c, one_minus_c * Axis.x * Axis.y - Axis.z * s, one_minus_c * Axis.z * Axis.x + Axis.y * s,
        one_minus_c * Axis.x * Axis.y + Axis.z * s, one_minus_c * Axis.y * Axis.y + c, one_minus_c * Axis.y * Axis.z - Axis.x * s,
        one_minus_c * Axis.z * Axis.x - Axis.y * s, one_minus_c * Axis.y * Axis.z + Axis.x * s, one_minus_c * Axis.z * Axis.z + c
    };
    Out = mul(rot_mat, In);
}

//rotates the point according to our rotation input and returns result
float3 CreateRotation(float3 toRotate) {
    float3 output = toRotate;

    if (_EnviRotation.x != 0)
        Unity_RotateAboutAxis_Degrees_float(output, float3 (1, 0, 0), -_EnviRotation.x, output);
    if (_EnviRotation.y != 0)
        Unity_RotateAboutAxis_Degrees_float(output, float3 (0, 1, 0), -_EnviRotation.y, output);
    if (_EnviRotation.z != 0)
        Unity_RotateAboutAxis_Degrees_float(output, float3 (0, 0, 1), -_EnviRotation.z, output);

    return output;
}

///////////////////////////////////////////////////////////////////////////////
//                  Vertex and Fragment functions                            //
///////////////////////////////////////////////////////////////////////////////

// Used in Standard (Physically Based) shader
Varyings LitPassVertex(Attributes input)
{
    Varyings output = (Varyings)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

    // normalWS and tangentWS already normalize.
    // this is required to avoid skewing the direction during interpolation
    // also required for per-vertex lighting and SH evaluation
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

    half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);

    half fogFactor = 0;
    #if !defined(_FOG_FRAGMENT)
        fogFactor = ComputeFogFactor(vertexInput.positionCS.z);
    #endif

    output.objectPosWSRotated = vertexInput.positionWS;

#ifdef _LCRS_PROBE_ROTATION
    float3 ObjectPosition = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0)).xyz;
    ObjectPosition += _EnviPosition;    
    output.objectPosWSRotated -= ObjectPosition;    
    output.objectPosWSRotated = CreateRotation(output.objectPosWSRotated);
    output.objectPosWSRotated += ObjectPosition;
#endif

    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.uv1 = TRANSFORM_TEX(
        _DecalUV == 0 ? input.texcoord * _DecalTileOffset.xy + _DecalTileOffset.zw :
        (_DecalUV == 1 ? input.texcoord1 * _DecalTileOffset.xy + _DecalTileOffset.zw :
            (_DecalUV == 2 ? input.texcoord2 * _DecalTileOffset.xy + _DecalTileOffset.zw :
                input.texcoord3 * _DecalTileOffset.xy + _DecalTileOffset.zw)), _DecalMap);
    output.uv2 = TRANSFORM_TEX(
        _LiveryUV == 0 ? input.texcoord * _LiveryTileOffset.xy + _LiveryTileOffset.zw :
        (_LiveryUV == 1 ? input.texcoord1 * _LiveryTileOffset.xy + _LiveryTileOffset.zw :
            (_LiveryUV == 2 ? input.texcoord2 * _LiveryTileOffset.xy + _LiveryTileOffset.zw :
                input.texcoord3 * _LiveryTileOffset.xy + _LiveryTileOffset.zw)), _LiveryMap);
    output.uv3 = TRANSFORM_TEX(
        _DirtUV == 0 ? input.texcoord * _DirtTileOffset.xy + _DirtTileOffset.zw :
        (_DirtUV == 1 ? input.texcoord1 * _DirtTileOffset.xy + _DirtTileOffset.zw :
            (_DirtUV == 2 ? input.texcoord2 * _DirtTileOffset.xy + _DirtTileOffset.zw :
                input.texcoord3 * _DirtTileOffset.xy + _DirtTileOffset.zw)), _DirtMap);

    // already normalized from normal transform to WS.
    output.normalWS = normalInput.normalWS;
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR) || defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
        real sign = input.tangentOS.w * GetOddNegativeScale();
        half4 tangentWS = half4(normalInput.tangentWS.xyz, sign);
    #endif
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        output.tangentWS = tangentWS;
    #endif

   #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(vertexInput.positionWS);
    half3 viewDirTS = GetViewDirectionTangentSpace(tangentWS, output.normalWS, viewDirWS);
    output.viewDirTS = viewDirTS;
#endif

    OUTPUT_LIGHTMAP_UV(input.staticLightmapUV, unity_LightmapST, output.staticLightmapUV);
#ifdef DYNAMICLIGHTMAP_ON
    output.dynamicLightmapUV = input.dynamicLightmapUV.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#endif
    OUTPUT_SH(output.normalWS.xyz, output.vertexSH);
#ifdef _ADDITIONAL_LIGHTS_VERTEX
    output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
#else
    output.fogFactor = fogFactor;
#endif

    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
        output.positionWS = vertexInput.positionWS;
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        output.shadowCoord = GetShadowCoord(vertexInput);
    #endif

        output.positionCS = vertexInput.positionCS;

        output.objectPos = input.positionOS; //object space

        return output;
}

float mod(float a, float b)
{
    return a - floor(a / b) * b;
}
float2 mod(float2 a, float2 b)
{
    return a - floor(a / b) * b;
}
float3 mod(float3 a, float3 b)
{
    return a - floor(a / b) * b;
}
float4 mod(float4 a, float4 b)
{
    return a - floor(a / b) * b;
} 

//#if defined(SHADER_API_OPENGL)  !defined(SHADER_TARGET_GLSL)
//#define UNITY_BUGGY_TEX2DPROJ4
//#define UNITY_PROJ_COORD(a) a.xyw
//#else
//#define UNITY_PROJ_COORD(a) a
//#endif

//  #define WorldNormalVector(data,normal) 
//     half3(dot(data.normalWS.x,normal), dot(data.normalWS.y,normal), dot(data.normalWS.z,normal))

float3 LocalCorrect(float3 origVec, float3 bboxMin, float3 bboxMax, float3 objectPosWSRotated, float3 cubemapPos)
{
    // Find the ray intersection with box plane
    float3 invOrigVec = float3(1.0, 1.0, 1.0) / origVec;
    
    float3 intersecAtMaxPlane = (bboxMax - objectPosWSRotated) * invOrigVec;

    float3 intersecAtMinPlane = (bboxMin - objectPosWSRotated) * invOrigVec;

    // Get the largest intersection values (we are not intersted in negative values)
    float3 largestIntersec = max(intersecAtMaxPlane, intersecAtMinPlane);

    // Get the closest of all solutions
    float Distance = min(min(largestIntersec.x, largestIntersec.y), largestIntersec.z);

    // Get the intersection position
    float3 IntersectPositionWS = objectPosWSRotated + origVec * Distance;

    // Get corrected vector
    float3 localCorrectedVec = IntersectPositionWS - cubemapPos;

    return localCorrectedVec;
}

half3 GlossyEnvironmentReflectionEx(half3 reflectVector, float3 positionWS, half perceptualRoughness, half occlusion,
    float3 localCorrReflDirWS, float mixMultiplier)
{
#if !defined(_ENVIRONMENTREFLECTIONS_OFF)
    half3 irradiance;

#ifdef _REFLECTION_PROBE_BLENDING
    irradiance = CalculateIrradianceFromReflectionProbes(reflectVector, positionWS, perceptualRoughness);
#else
#ifdef _REFLECTION_PROBE_BOX_PROJECTION
    reflectVector = BoxProjectedCubemapDirection(reflectVector, positionWS, unity_SpecCube0_ProbePosition, unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax);
#endif // _REFLECTION_PROBE_BOX_PROJECTION
    half mip = PerceptualRoughnessToMipmapLevel(perceptualRoughness);
    half4 encodedIrradiance = half4(SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, reflectVector, mip));
    if (_EnableRealTimeReflection > 0.5)
    {
        encodedIrradiance = SAMPLE_TEXTURECUBE_LOD(_EnviCubeMapMain, sampler_EnviCubeMapMain, localCorrReflDirWS, mip);
#ifdef _REALTIMEREFLECTION_MIX
        //change full black areas with other probes
        if (encodedIrradiance.r < 0.01 && encodedIrradiance.g < 0.01 && encodedIrradiance.b < 0.01) {
            encodedIrradiance = SAMPLE_TEXTURECUBE_LOD(_EnviCubeMapToMix1, sampler_EnviCubeMapToMix1, localCorrReflDirWS, mip) * mixMultiplier;
        }
#endif
    }
#if defined(UNITY_USE_NATIVE_HDR)
    irradiance = encodedIrradiance.rgb;
#else
    irradiance = DecodeHDREnvironment(encodedIrradiance, unity_SpecCube0_HDR);
#endif // UNITY_USE_NATIVE_HDR
    if (_EnableRealTimeReflection > 0.5)
    {
        irradiance = DecodeHDREnvironment(encodedIrradiance, _EnviCubeMapMain_HDR);
#ifdef _REALTIMEREFLECTION_MIX
        //change full black areas with other probes
        if (irradiance.r < 0.01 && irradiance.g < 0.01 && irradiance.b < 0.01) {
            irradiance = DecodeHDREnvironment(encodedIrradiance, _EnviCubeMapToMix1_HDR);
        }
#endif
    }
#endif // _REFLECTION_PROBE_BLENDING
    return irradiance * occlusion;
#else
    return _GlossyEnvironmentColor.rgb * occlusion;
#endif // _ENVIRONMENTREFLECTIONS_OFF
}

half3 GlobalIlluminationEx(BRDFData brdfData, BRDFData brdfDataClearCoat, float clearCoatMask,
    half3 bakedGI, half occlusion, float3 positionWS,
    half3 normalWS, half3 viewDirectionWS,
    float3 objectPosWSRotated, float3 bBoxMin, float3 bBoxMax, float3 enviCubeMapPos, float mixMultiplier)
{
    half3 reflectVector = reflect(-viewDirectionWS, normalWS);

    #ifdef _LCRS_PROBE_ROTATION
        reflectVector = CreateRotation(reflectVector);
    #endif

    float3 localCorrReflDirWS = reflectVector;
    //find local correction if real time!
    //#ifdef _REALTIMEREFLECTION
    if (_EnableRealTimeReflection > 0.5)
    {
        localCorrReflDirWS = LocalCorrect(reflectVector, bBoxMin, bBoxMax, objectPosWSRotated, enviCubeMapPos);
    }
    //#endif

    half NoV = saturate(dot(normalWS, viewDirectionWS));
    half fresnelTerm = Pow4(1.0 - NoV);

    half3 indirectDiffuse = bakedGI;
    half3 indirectSpecular = GlossyEnvironmentReflectionEx(reflectVector, positionWS, brdfData.perceptualRoughness, 1.0h,
        localCorrReflDirWS, mixMultiplier);

    half3 color = EnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);

    if (IsOnlyAOLightingFeatureEnabled())
    {
        color = half3(1, 1, 1); // "Base white" for AO debug lighting mode
    }

#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
    half3 coatIndirectSpecular = GlossyEnvironmentReflectionEx(reflectVector, positionWS, brdfDataClearCoat.perceptualRoughness, 1.0h,
        localCorrReflDirWS, mixMultiplier);
    // TODO: "grazing term" causes problems on full roughness
    half3 coatColor = EnvironmentBRDFClearCoat(brdfDataClearCoat, clearCoatMask, coatIndirectSpecular, fresnelTerm);

    // Blend with base layer using khronos glTF recommended way using NoV
    // Smooth surface & "ambiguous" lighting
    // NOTE: fresnelTerm (above) is pow4 instead of pow5, but should be ok as blend weight.
    half coatFresnel = kDielectricSpec.x + kDielectricSpec.a * fresnelTerm;
    return (color * (1.0 - coatFresnel * clearCoatMask) + coatColor) * occlusion;
#else
    return color * occlusion;
#endif
}

///////////////////////////////////////////////////////////////////////////////
//                      Fragment Functions                                   //
//       Used by ShaderGraph and others builtin renderers                    //
///////////////////////////////////////////////////////////////////////////////
half4 UniversalFragmentPBREx(InputData inputData, SurfaceData surfaceData,
    float3 objectPosWSRotated, float3 bBoxMin, float3 bBoxMax, float3 enviCubeMapPos, float mixMultiplier)
{
    #if defined(_SPECULARHIGHLIGHTS_OFF)
    bool specularHighlightsOff = true;
    #else
    bool specularHighlightsOff = false;
    #endif
    BRDFData brdfData;

    // NOTE: can modify "surfaceData"...
    InitializeBRDFData(surfaceData, brdfData);

    #if defined(DEBUG_DISPLAY)
    half4 debugColor;

    if (CanDebugOverrideOutputColor(inputData, surfaceData, brdfData, debugColor))
    {
        return debugColor;
    }
    #endif
    
    // Clear-coat calculation...
    BRDFData brdfDataClearCoat = CreateClearCoatBRDFData(surfaceData, brdfData);
    half4 shadowMask = CalculateShadowMask(inputData);
    AmbientOcclusionFactor aoFactor = CreateAmbientOcclusionFactor(inputData, surfaceData);
    uint meshRenderingLayers = GetMeshRenderingLayer();
    Light mainLight = GetMainLight(inputData, shadowMask, aoFactor);

    // NOTE: We don't apply AO to the GI here because it's done in the lighting calculation below...
    MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI);

    LightingData lightingData = CreateLightingData(inputData, surfaceData);

    lightingData.giColor = GlobalIlluminationEx(brdfData, brdfDataClearCoat, surfaceData.clearCoatMask,
                                              inputData.bakedGI, aoFactor.indirectAmbientOcclusion, inputData.positionWS,
                                              inputData.normalWS, inputData.viewDirectionWS,
                                              objectPosWSRotated, bBoxMin, bBoxMax, enviCubeMapPos, mixMultiplier);

    if (IsMatchingLightLayer(mainLight.layerMask, meshRenderingLayers))
    {
        lightingData.mainLightColor = LightingPhysicallyBased(brdfData, brdfDataClearCoat,
                                                              mainLight,
                                                              inputData.normalWS, inputData.viewDirectionWS,
                                                              surfaceData.clearCoatMask, specularHighlightsOff);
    }

    #if defined(_ADDITIONAL_LIGHTS)
    uint pixelLightCount = GetAdditionalLightsCount();

    #if USE_CLUSTERED_LIGHTING
    for (uint lightIndex = 0; lightIndex < min(_AdditionalLightsDirectionalCount, MAX_VISIBLE_LIGHTS); lightIndex++)
    {
        Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);

        if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
        {
            lightingData.additionalLightsColor += LightingPhysicallyBased(brdfData, brdfDataClearCoat, light,
                                                                          inputData.normalWS, inputData.viewDirectionWS,
                                                                          surfaceData.clearCoatMask, specularHighlightsOff);
        }
    }
    #endif

    LIGHT_LOOP_BEGIN(pixelLightCount)
        Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);

        if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
        {
            lightingData.additionalLightsColor += LightingPhysicallyBased(brdfData, brdfDataClearCoat, light,
                                                                          inputData.normalWS, inputData.viewDirectionWS,
                                                                          surfaceData.clearCoatMask, specularHighlightsOff);
        }
    LIGHT_LOOP_END
    #endif

    #if defined(_ADDITIONAL_LIGHTS_VERTEX)
    lightingData.vertexLightingColor += inputData.vertexLighting * brdfData.diffuse;
    #endif

    return CalculateFinalColor(lightingData, surfaceData.alpha);
}

// Used in Standard (Physically Based) shader
half4 LitPassFragment(Varyings input) : SV_Target
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

#if defined(_PARALLAXMAP)
#if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
    half3 viewDirTS = input.viewDirTS;
#else
    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
    half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, viewDirWS);
#endif
    ApplyPerPixelDisplacement(viewDirTS, input.uv);
#endif

    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(input.uv, surfaceData);

//half3 viewDir;
//#ifdef _NORMALMAP
//    viewDir = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
//#else 
//    viewDir = input.viewDirWS;
//#endif

    half3 outputNormal = surfaceData.normalTS;
//#ifdef _FLAKENORMAL
if(_FlakesUsage > 0.5)
{
    // Apply scaled flake normal map
    half3 flakeNormal = SampleNormal(input.uv * _FlakesBumpMapScale, 
        TEXTURE2D_ARGS(_FlakesBumpMap, sampler_FlakesBumpMap), _BumpScale);

    // Apply flake map strength
    half3 scaledFlakeNormal = flakeNormal;
    scaledFlakeNormal.xy *= _FlakesBumpStrength;
    scaledFlakeNormal.z = 0; // Z set to 0 for better blending with other normal map.

    // Blend regular normal map with flakes normal map
    outputNormal = normalize(outputNormal + scaledFlakeNormal);
}
//#endif
//#ifdef _DIRTMAP
if(_DirtUsage > 0.5)
{
    float4 dirty = SAMPLE_TEXTURE2D(_DirtMap, sampler_DirtMap, input.uv3);
    half dirtyAlpha = saturate(dirty.a * _DirtMapCutoff);
    half3 dirtyNormal = SampleNormal(input.uv3,
        TEXTURE2D_ARGS(_DirtBumpMap, sampler_DirtBumpMap), _BumpScale);
    //new normal will be directly dirt's normal, because it is another layer on top of everything
    outputNormal = normalize(lerp(outputNormal, dirtyNormal, dirtyAlpha));
}
//#endif
	surfaceData.normalTS = outputNormal; //it may changed above

    InputData inputData;
    InitializeInputData(input, surfaceData.normalTS, inputData);
    SETUP_DEBUG_TEXTURE_DATA(inputData, input.uv, _BaseMap);

#ifdef _DBUFFER
    ApplyDecalToSurfaceData(input.positionCS, surfaceData, inputData);
#endif

    // Apply Fresnel
    //float fresnel = 1.0 - max(dot(normalize(inputData.viewDirectionWS), input.normalWS), 0.0);

    float fresnel = pow((1.0 - saturate(dot(normalize(input.normalWS), normalize(inputData.viewDirectionWS)))), _FresnelPower);
    
    //fresnel = pow(fresnel, _FresnelPower);
    /*surfaceData.albedo = lerp(surfaceData.albedo, _FresnelColor.xyz, saturate(fresnel)); */
    //return float4(lerp(surfaceData.albedo, _FresnelColor2.xyz, saturate(fresnel)), 1);
    //return float4(inputData.viewDirectionWS, 1);
    //_FresnelColor.xyz = lerp(_FresnelColor2.xyz, _FresnelColor.xyz, saturate(fresnel));
    
    surfaceData.albedo = lerp(lerp(surfaceData.albedo, _FresnelColor2.xyz, saturate(fresnel)),
        lerp(_FresnelColor2.xyz, _FresnelColor.xyz, saturate(fresnel * fresnel)), saturate(fresnel));
    
    /*float fresnelSq = fresnel * fresnel;
    surfaceData.albedo = lerp(surfaceData.albedo, fresnel * _FresnelColor.xyz +
        fresnelSq * _FresnelColor2.xyz, saturate(fresnel));*/


    half4 color = UniversalFragmentPBREx(inputData, surfaceData,
        input.objectPosWSRotated, _BBoxMin.xyz, _BBoxMax.xyz, _EnviCubeMapPos.xyz, _MixMultiplier);

//mix below code above
//half4 color = UniversalFragmentPBR(inputData, surfaceData.albedo, surfaceData.metallic, 
//        surfaceData.specular, surfaceData.smoothness, surfaceData.occlusion, surfaceData.emission, surfaceData.alpha);

//!!!!! We are changing surface data with livery here be carefull if you will use it later!!!
//#ifdef _LIVERYMAP
if(_LiveryUsage > 0.5)
{ 
    float4 liveryTex = SAMPLE_TEXTURE2D(_LiveryMap, sampler_LiveryMap, input.uv2);
    surfaceData.albedo = liveryTex.xyz * _LiveryColor;
    surfaceData.metallic = _LiveryMetalic;
    surfaceData.smoothness = _LiverySmoothness;

    half4 livery = UniversalFragmentPBREx(inputData, surfaceData,
        input.objectPosWSRotated, _BBoxMin.xyz, _BBoxMax.xyz, _EnviCubeMapPos.xyz, _MixMultiplier);

    color = lerp(color, livery, liveryTex.a);
}
//#endif

//!!!!! We are changing surface data with decal here be carefull if you will use it later!!!
//#ifdef _DECALMAP
if(_DecalUsage > 0.5)
{
    float4 decalTex = SAMPLE_TEXTURE2D(_DecalMap, sampler_DecalMap, input.uv1);
    surfaceData.albedo = decalTex.xyz * _DecalColor;
    surfaceData.metallic = _DecalMetalic;
    surfaceData.smoothness = _DecalSmoothness;

   
    half4 decal = UniversalFragmentPBREx(inputData, surfaceData,
        input.objectPosWSRotated, _BBoxMin.xyz, _BBoxMax.xyz, _EnviCubeMapPos.xyz, _MixMultiplier);
   
    color = lerp(color, decal, decalTex.a);
}
//#endif

//#ifdef _REALTIMEREFLECTION
//    half4 colorRef = UniversalFragmentPBRExtended(inputData, surfaceData.alpha,
//        input.objectPosWS, _BBoxMin.xyz, _BBoxMax.xyz, _EnviCubeMapPos.xyz, _MixMultiplier);

//    #ifdef _DECALMAP
//        color = lerp(color, colorRef, _EnviCubeIntensity*(1-decalAlbedo.a));
//    #else
//        color = lerp(color, colorRef, _EnviCubeIntensity);
//    #endif
//#endif


//because dirt is not attached to color it self (it is like another layer on object)
//we will change this final color (after metalic color etc.)
//#ifdef _DIRTMAP
if(_DirtUsage > 0.5)
{
    float4 dirt = SAMPLE_TEXTURE2D(_DirtMap, sampler_DirtMap, input.uv3);
    //normal will be changed before initialization, look into the befıre "InitializeInputData" function !!!!
    /*half3 dirtNormal = SampleNormal(input.uv3,
        TEXTURE2D_ARGS(_DirtBumpMap, sampler_DirtBumpMap), _BumpScale);
    */
    half dirtAlpha = saturate(dirt.a * _DirtMapCutoff);
    
    surfaceData.albedo = dirt.xyz * _DirtColor;
    surfaceData.metallic = _DirtMetalic;
    surfaceData.smoothness = _DirtSmoothness;
    
    half4 dirtColor = UniversalFragmentPBREx(inputData, surfaceData,
        input.objectPosWSRotated, _BBoxMin.xyz, _BBoxMax.xyz, _EnviCubeMapPos.xyz, _MixMultiplier);

    color.rgb = lerp(color.rgb, dirtColor.xyz, dirtAlpha);
}

    color.rgb = MixFog(color.rgb, inputData.fogCoord);
    color.a = OutputAlpha(color.a, _Surface);
    color.rgb += _AddColor.rgb;
    return color;
}

#endif
