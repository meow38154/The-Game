Shader "Custom/SpriteGlitchStrong"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Intensity("Intensity", Range(0,1)) = 0.5
        _Speed("Speed", Float) = 12
        _BlockSize("Block Size", Range(5,80)) = 25
        _RGBOffset("RGB Offset", Range(0,0.1)) = 0.025
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Intensity;
            float _Speed;
            float _BlockSize;
            float _RGBOffset;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            float Random(float2 value)
            {
                return frac(sin(dot(value, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert(appdata input)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                output.color = input.color;
                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                float2 uv = input.uv;
                float timeValue = floor(_Time.y * _Speed);

                float blockY = floor(uv.y * _BlockSize);
                float noise = Random(float2(blockY, timeValue));

                float glitchLine = step(0.55, noise);
                float xOffset = (noise - 0.5) * _Intensity * 0.25 * glitchLine;

                uv.x += xOffset;

                float rgbOffset = _RGBOffset * _Intensity;

                fixed4 baseColor = tex2D(_MainTex, uv);

                fixed r = tex2D(_MainTex, uv + float2(rgbOffset, 0)).r;
                fixed g = tex2D(_MainTex, uv).g;
                fixed b = tex2D(_MainTex, uv - float2(rgbOffset, 0)).b;

                fixed4 finalColor = fixed4(r, g, b, baseColor.a);

                clip(finalColor.a - 0.01);

                return finalColor * input.color;
            }

            ENDCG
        }
    }
}