Shader "Custom/TopSideTexture"
{
    Properties
    {
        _TopTex ("Top Texture", 2D) = "white" {}
        _SideTex ("Side Texture", 2D) = "white" {}
        _Blend ("Blend Strength", Range(1,20)) = 8
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            TEXTURE2D(_TopTex);
            SAMPLER(sampler_TopTex);

            TEXTURE2D(_SideTex);
            SAMPLER(sampler_SideTex);

            float _Blend;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 normal = normalize(IN.normalWS);
                
                float topMask = pow(saturate(normal.y), _Blend);

                half4 topCol =
                    SAMPLE_TEXTURE2D(_TopTex, sampler_TopTex, IN.uv);

                half4 sideCol =
                    SAMPLE_TEXTURE2D(_SideTex, sampler_SideTex, IN.uv);

                return lerp(sideCol, topCol, topMask);
            }

            ENDHLSL
        }
    }
}