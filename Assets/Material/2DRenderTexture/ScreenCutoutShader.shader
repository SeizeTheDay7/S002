Shader "URP/ScreenCutoutShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" "RenderType" = "Transparent" }

        Pass
        {
            Name "ScreenCutoutPass"
            Tags { "LightMode" = "UniversalForward" }

            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 screenUV : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float4 posHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionHCS = posHCS;

                // NDC 기준으로 스크린 UV 계산 (0~1)
                OUT.screenUV = ComputeScreenPos(posHCS);

                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 screenUV = IN.screenUV.xy / IN.screenUV.w;
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, screenUV);
                return col;
            }

            ENDHLSL
        }
    }
}
