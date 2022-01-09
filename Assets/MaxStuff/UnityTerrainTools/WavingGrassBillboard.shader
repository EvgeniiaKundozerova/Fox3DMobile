// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Hidden/TerrainEngine/Details/BillboardWavingDoublePass" {
    Properties {
        _WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
        _WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
        _Cutoff ("Cutoff", float) = 0.5
    }

CGINCLUDE
#include "UnityCG.cginc"
#include "TerrainEngine.cginc"

#define BENDERS 16
half4 _GrassBendersPositions [BENDERS];

half3 _GrassRandomOffset;
half2 _GrassRotate;

half3 _GrassFaceDirRight;
half3 _GrassFaceDirUp;
half3 _GrassFaceDirForward;

half3 _GrassWindDir;
half4 _GrassWindSettings;

sampler2D _GrassControl;
float4 _GrassControl_ST;
float4 _GrassLayerColors[4];

float4 _GrassGradientSettings;

struct Input {
    half2 uv_MainTex;
    half4 grassColor;
    half4 terrainColor;
};

float rand(float3 co)
{
    return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
}

void TerrainBillboardGrass_( inout float4 pos, float2 offset )
{
    // dist to camera
    float3 grasspos = pos.xyz - _CameraPosition.xyz;
    if (dot(grasspos, grasspos) > _WaveAndDistance.w)
        offset = 0.0;

    // random xz offset
    float2 randomOffset = _GrassRandomOffset.xz * float2(rand(pos.xyz) - 0.5, rand(pos.zyx) - 0.5);
    pos.xyz += float3(randomOffset.x, _GrassRandomOffset.y, randomOffset.y);

    // bending
    float bending = 1.0;
    for (int i = 0; i < BENDERS; i++)
    {
        float4 grassBenderPosition = _GrassBendersPositions[i];
        float3 dirToBender = grassBenderPosition.xyz - pos.xyz;
        bending = min(bending, saturate(dot(dirToBender, dirToBender) - grassBenderPosition.w) * 0.7 + 0.3);
    }
    offset *= bending;

    // wind power
    half windDot = dot(_GrassWindDir.xyz, pos.xyz);
    half constWind = _GrassWindSettings.x;
    half wind = sin(_Time.y * _GrassWindSettings.w + windDot * 0.1) * _GrassWindSettings.z;
    half randomWind = cos(_Time.z * 3.0 + pos.x * pos.y) * _GrassWindSettings.y * 0.5;
    //pos.xyz += _GrassWindDir.xyz * (constWind + wind + randomWind) * offset.y;

    // rotate basis
    float rotX = (rand(pos.xyz) - 0.5) * _GrassRotate.x + wind * offset.y;
    float3x3 rotMatrixX = float3x3(
        1, 0, 0,
        0, cos(rotX), -sin(rotX),
        0, sin(rotX), cos(rotX));
    float rotY = (rand(pos.yxz) - 0.5) * _GrassRotate.y + randomWind * offset.y;
    float3x3 rotMatrixY = float3x3(
        cos(rotY), 0, sin(rotY),
        0, 1, 0,
        -sin(rotY), 0, cos(rotY));
    _GrassFaceDirRight.xyz = mul(rotMatrixY, mul(rotMatrixX, _GrassFaceDirRight.xyz));
    _GrassFaceDirUp.xyz = mul(rotMatrixY, mul(rotMatrixX, _GrassFaceDirUp.xyz));
    pos.xyz += offset.x * _GrassFaceDirRight.xyz;
    pos.xyz += offset.y * _GrassFaceDirUp.xyz;
}

void WavingGrassBillboardVert_ (inout appdata_full v, out Input o)
{
    TerrainBillboardGrass_ (v.vertex, v.tangent.xy);

    o.uv_MainTex = v.texcoord.xy;

    o.grassColor = v.color;
    half gradientControlX = v.tangent.y;
    //half gradientControlX = v.texcoord.y;
    o.grassColor.a = gradientControlX;

    float2 pos_ls = v.vertex.xz;
    float2 controlUv = TRANSFORM_TEX(pos_ls, _GrassControl);
    fixed4 control = tex2Dlod(_GrassControl, float4(controlUv.x, controlUv.y, 0, 0));
    fixed4 terrainColor = _GrassLayerColors[0] * control.x
        + _GrassLayerColors[1] * control.y
        + _GrassLayerColors[2] * control.z
        + _GrassLayerColors[3] * control.w;
    o.terrainColor = terrainColor;
    o.terrainColor.a = _GrassGradientSettings.x + gradientControlX * _GrassGradientSettings.y;
}

ENDCG

    SubShader {
        Tags {
            "Queue" = "Geometry+200"
            "IgnoreProjector"="True"
            "RenderType"="GrassBillboard"
            "DisableBatching"="True"
        }
        Cull Off
        LOD 200
        ColorMask RGB

CGPROGRAM
#pragma target 3.5
#pragma surface surf Lambert vertex:WavingGrassBillboardVert_ addshadow fullforwardshadows exclude_path:deferred

sampler2D _MainTex;
fixed _Cutoff;

void surf (Input IN, inout SurfaceOutput o) {

    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = lerp(IN.terrainColor.rgb, c.rgb * IN.grassColor.rgb, saturate(IN.terrainColor.a));
    //o.Albedo = IN.terrainColor.rgb;
    //o.Albedo = IN.grassColor.rgb;
    //o.Albedo = fixed3(saturate(IN.grassColor.a), 0, 0);
    //o.Albedo = fixed3(saturate(IN.terrainColor.a), 0, 0);

    o.Alpha = c.a;
    clip (o.Alpha - _Cutoff);
}

ENDCG
    }

    Fallback Off
}
