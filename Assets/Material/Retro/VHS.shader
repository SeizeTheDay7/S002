Shader "Hidden/RetroVHS_Fullscreen"
{
    Properties
    {
        // ※ _BlitTexture 는 엔진에서 자동으로 공급되므로 굳이 선언하지 않는다
        _MaskTex   ("Shadow Mask (Opt.)", 2D) = "white" {}
        _ScanPower ("Scanline Strength", Range(0, 2)) = 1
        _Bleed     ("Color Bleed (px)",  Float) = 1.2
        _NoiseAmp  ("Noise Amplitude",   Range(0, 1)) = 0.15
        _JitterAmp ("Horizontal Jitter", Range(0, 3)) = 0.3
        _TimeScale ("Jitter Speed",      Range(0, 8)) = 2.1
    }

    SubShader
    {
        // 풀스크린 패스용 필수 태그
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Overlay" }

        Pass
        {
            Name "RetroVHS_Fullscreen"
            // 후처리이므로 깊이/컬링/마스크 불필요
            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            // 공통 유틸
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // 풀스크린 정점/인터폴레이터 (Vert, Attributes, Varyings 정의)
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fragment _      // 런타임 키워드 최소화

            // ───────── 텍스처 & 샘플러 ──────────────────────────────────────────────
            // TEXTURE2D_X(_BlitTexture);    SAMPLER(sampler_BlitTexture);
            // SAMPLER(sampler_BlitTexture);
            TEXTURE2D    (_MaskTex);      SAMPLER(sampler_MaskTex);

            // ───────── 머티리얼 파라미터 ───────────────────────────────────────────
            float   _ScanPower;
            float   _Bleed;
            float   _NoiseAmp;
            float   _JitterAmp;
            float   _TimeScale;
            // _BlitTexture_TexelSize: (1/width,1/height,width,height) – 엔진 자동

            // 1-D 해시 : 값 0-1
            float Hash(float2 p) { return frac(sin(dot(p, 12.9898)) * 43758.5453); }

            half4 Frag (Varyings IN) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);     // XR

                float2 uv  = IN.texcoord;

                // ── Horizontal VHS jitter ────────────────────────────────────────
                float jitter = (Hash(float2(_Time.y * _TimeScale, 1)) - 0.5)
                             * _JitterAmp * _BlitTexture_TexelSize.y;
                uv.x += jitter;

                // ── Chromatic aberration / color-bleed ────────────────────────────
                float2 bleed = float2(_Bleed * _BlitTexture_TexelSize.x, 0);
                float  r = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv + bleed).r;
                float  g = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv).g;
                float  b = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv - bleed).b;
                float3 col = float3(r, g, b);

                // ── Scanlines ────────────────────────────────────────────────────
                // float scan = sin(uv.y * _ScreenParams.y * 3.14159) * 0.5 + 0.5;
                // float scanLineDensity = 300.0; // 화면 높이에 무관하게 고정 밀도
                // float scan = sin(uv.y * scanLineDensity * 3.14159) * 0.5 + 0.5;
                // col *= lerp(1.0, scan, _ScanPower);

                // // ── Shadow-mask (CRT triad) – 선택 ───────────────────────────────
                // float3 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex,
                //                                uv * float2(_ScreenParams.x / 3.0, _ScreenParams.y)).rgb;
                // col *= mask;

                // // ── Noise ────────────────────────────────────────────────────────
                // float n = (Hash(uv * _ScreenParams.xy + _Time.y) - 0.5) * _NoiseAmp;
                // col += n;

                return float4(col, 1);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
