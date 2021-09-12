Shader "Hidden/Custom/Glitch"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

        float _ScanlineJitterFrequency;
        float _ScanlineJitterDisplacement;

        // Pseudo random number generator with 2D coordinates
        float UVRandom(float u, float v)
        {
            return frac(sin(dot(float2(u, v), float2(12.9898,78.233))) * 43758.5453);
        }

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            // uses a combination of the analogue glitch's scanline jitter and the digital glitch from KinoGlitch (https://github.com/keijiro/KinoGlitch/tree/e102950361c260aba8967d55f14212b5a70f4cce)

            float u = i.texcoord.x;
            float v = i.texcoord.y;

            float jitter = UVRandom(v, _Time.x) * 2 - 1;
            jitter *= step(_ScanlineJitterFrequency, abs(jitter)) * _ScanlineJitterDisplacement;

            // TODO: digital glitch

            return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(frac(u + jitter), v));
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}
