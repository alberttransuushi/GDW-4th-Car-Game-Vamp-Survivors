

Shader "Custom/WibblyWobbly"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        _Affine("Affine", Vector) = (1, 1, 0, 0)
        _Distortion("Distortion", Range(0, 1)) = 0.1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200 

        CGPROGRAM

        #include "UnityCG.cginc"

        #pragma surface surf ToonRamp 


        #pragma target 5.0

        sampler2D _MainTex;
        sampler2D _RampTex;
        fixed4 _Color;
        float4 _Affine;
        float _Distortion;


        float4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            float diff = dot(s.Normal, lightDir);
            float h = diff * 0.5 + 0.5;
            float2 rh = h;
            float3 ramp = tex2D(_RampTex, rh).rgb;

            float4 t;
            t.rgb = s.Albedo * _LightColor0.rgb * (ramp);
            t.a = s.Alpha;
            return t;
        }





        struct Input
        {
            float2 uv_MainTex;
        };




        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
