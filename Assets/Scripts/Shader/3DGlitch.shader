Shader "Custom/URP/Glitch3D"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)

        _GlitchStrength ("Vertex Glitch Strength", Range(0, 0.3)) = 0.04
        _RGBSplit ("RGB Split", Range(0, 0.08)) = 0.015

        _BandCount ("Band Count", Range(1, 80)) = 25
        _BandWidth ("Band Width", Range(0.01, 1)) = 0.18
        _BandSpeed ("Band Speed", Range(0, 30)) = 8

        _FlickerStrength ("Flicker Strength", Range(0, 1)) = 0.25
        _ScanlineStrength ("Scanline Strength", Range(0, 1)) = 0.15
        _ScanlineDensity ("Scanline Density", Range(1, 400)) = 120
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "GlitchUnlit"
            Tags { "LightMode" = "UniversalForward" }

            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;

                float _GlitchStrength;
                float _RGBSplit;

                float _BandCount;
                float _BandWidth;
                float _BandSpeed;

                float _FlickerStrength;
                float _ScanlineStrength;
                float _ScanlineDensity;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionOS : TEXCOORD1;
            };

            float Hash(float value)
            {
                return frac(sin(value * 12.9898) * 43758.5453);
            }

            Varyings Vert(Attributes input)
            {
                Varyings output;

                float3 posOS = input.positionOS.xyz;

                float time = _Time.y;
                float bandValue = posOS.y * _BandCount + time * _BandSpeed;
                float bandID = floor(bandValue);
                float bandPosition = frac(bandValue);

                float bandMask = step(1.0 - _BandWidth, bandPosition);
                float randomValue = Hash(bandID);

                float xOffset = (randomValue - 0.5) * _GlitchStrength * bandMask;
                float zOffset = (Hash(bandID + 14.7) - 0.5) * _GlitchStrength * 0.35 * bandMask;

                posOS.x += xOffset;
                posOS.z += zOffset;

                output.positionOS = posOS;
                output.positionCS = TransformObjectToHClip(posOS);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float time = _Time.y;

                float bandValue = input.positionOS.y * _BandCount + time * _BandSpeed;
                float bandID = floor(bandValue);
                float bandPosition = frac(bandValue);

                float bandMask = step(1.0 - _BandWidth, bandPosition);
                float randomValue = Hash(bandID);

                float splitAmount = _RGBSplit * bandMask * (randomValue * 2.0 - 1.0);

                float2 uvR = input.uv + float2(splitAmount, 0);
                float2 uvG = input.uv;
                float2 uvB = input.uv - float2(splitAmount, 0);

                half r = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uvR).r;
                half g = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uvG).g;
                half b = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uvB).b;
                half a = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv).a;

                half4 color = half4(r, g, b, a) * _BaseColor;

                float flicker = Hash(floor(time * 30.0) + bandID);
                color.rgb += (flicker - 0.5) * _FlickerStrength * bandMask;

                float scanline = sin(input.positionCS.y * _ScanlineDensity * 0.01 + time * 20.0);
                scanline = scanline * 0.5 + 0.5;
                color.rgb *= lerp(1.0, scanline, _ScanlineStrength);

                return color;
            }

            ENDHLSL
        }
    }
}