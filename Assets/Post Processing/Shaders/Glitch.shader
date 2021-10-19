Shader "Hidden/Custom/Glitch"
{
    // based off the digital glitch from KinoGlitch (https://github.com/keijiro/KinoGlitch/tree/e102950361c260aba8967d55f14212b5a70f4cce)

    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        TEXTURE2D_SAMPLER2D(_NoiseTex, sampler_NoiseTex);
        TEXTURE2D_SAMPLER2D(_TrashTex, sampler_TrashTex);

        float _Intensity;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float2 xy = i.texcoord;

            float4 glitch = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, xy);

            float thresh = 1.001 - _Intensity * 1.001;
            float w_d = step(thresh, pow(glitch.z, 2.5)); // displacement glitch
            float w_f = step(thresh, pow(glitch.w, 2.5)); // frame glitch

            // Displacement.
            float2 uv = frac(xy + glitch.xy * w_d);
            float4 source = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

            // Mix with trash frame.
            float3 color = lerp(source, SAMPLE_TEXTURE2D(_TrashTex, sampler_TrashTex, uv), w_f).rgb;

            return float4(color, source.a);
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
