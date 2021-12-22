Shader "Hidden/Custom/Glitch"
{
    // based off the digital glitch from KinoGlitch (https://github.com/keijiro/KinoGlitch/tree/e102950361c260aba8967d55f14212b5a70f4cce)

    Properties
    {
        _MainTex  ("-", 2D) = "" {}
        _NoiseTex ("-", 2D) = "" {}
        _TrashTex ("-", 2D) = "" {}
    }

    CGINCLUDE
        #include "UnityCG.cginc"

        sampler2D _MainTex;
        sampler2D _NoiseTex;
        sampler2D _TrashTex;
        float _Intensity;

        float4 frag(v2f_img i) : SV_Target 
        {
            float4 glitch = tex2D(_NoiseTex, i.uv);

            float thresh = 1.001 - _Intensity * 1.001;
            float w_d = step(thresh, pow(glitch.z, 2.5)); // displacement glitch
            float w_f = step(thresh, pow(glitch.w, 2.5)); // frame glitch

            // Displacement.
            float2 uv = frac(i.uv + glitch.xy * w_d);
            float4 source = tex2D(_MainTex, uv);

            // Mix with trash frame.
            float3 color = lerp(source, tex2D(_TrashTex, uv), w_f).rgb;

            return float4(color, source.a);
        }

    ENDCG

    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            CGPROGRAM
                #pragma vertex vert_img
                #pragma fragment frag
                #pragma target 3.0
            ENDCG
        }
    }
}
