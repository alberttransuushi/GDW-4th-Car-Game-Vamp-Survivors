



Shader "Custom/WarnaBaseShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        _RimColor("Rim Color", Color) = (0,0.5,0.5,0)
        _RimPower("Rim Power", Range(0.5,8.0)) = 3.0
        //_myBump("Bump Texture", 2D) = "bump" {}
        //_mySlider("Bump Amount", Range(0,10)) = 1
    }
        SubShader
        {
            
                CGPROGRAM


                #include "UnityCG.cginc"

                #pragma surface surf ToonRamp

                sampler2D _MainTex;
                sampler2D _RampTex;
                float4 _RimColor;
                float _RimPower;
                //sampler2D _myBump;
                //half _mySlider;


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
                    float2 uv_RampTex;
                    //float2 uv_myBump;
                    //float2 uv_MainTex_ST;
                    float3 viewDir;

                };



                void surf(Input IN, inout SurfaceOutput o)
                {
                    half4 c = tex2D(_MainTex, IN.uv_MainTex);
                    half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
                    o.Albedo = c.rgb - _RimColor.rgb * pow(rim, _RimPower) * 3;
                    //o.Alpha = c.a;
                    //o.Emission = _RimColor.rgb * pow(rim, _RimPower) * 10;
                    //o.Normal = UnpackNormal(tex2D(_myBump, IN.uv_myBump));
                    //o.Normal *= float3(_mySlider, _mySlider, 1);
                }
            
            
            ENDCG
        }
            FallBack "Diffuse"
}
