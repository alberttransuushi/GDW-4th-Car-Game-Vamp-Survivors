Shader "Custom/StandardPBR"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MetallicTex ("Metallic (R)", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags{ 
            "Queue" = "Geometry" 
        }
        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MetallicTex;
        half _Metallic;
        fixed4 _Color;
        float4 _RimColor;
        float _RimPower;

        struct Input
        {
            float2 uv_MetallicTex;
            float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half4 c = tex2D(_MetallicTex, IN.uv_MetallicTex);
            o.Albedo = _Color.rgb;
            o.Smoothness = c.rgb;
            //o.Metallic = _Metallic;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
