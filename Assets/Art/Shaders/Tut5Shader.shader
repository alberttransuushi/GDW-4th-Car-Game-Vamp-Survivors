Shader "Custom/Tut5Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Color2("Color2", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldPos;
            float3 worldNormal;
        };

        fixed4 _Color;
        fixed4 _Color2;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            half3 c = float3(2, IN.uv_MainTex);
            half3 v = float4(2, IN.viewDir);
            half3 w = float4(2, IN.worldPos);
            half3 nw = float4(2, IN.worldNormal);
            half4 dp = dot(normalize(IN.viewDir), IN.worldNormal) * _Color;

            //o.Albedo = c.rgb;
            //o.Albedo = v.rgb;
            //o.Albedo = w.rgb;
            //o.Albedo = nw.rgb;
            /*if (IN.worldPos.y > 0) {
                o.Albedo = _Color.rgb;
            }
            else {
                o.Albedo = _Color2.rgb;
            }*/
            o.Albedo = dp.rgb;
            o.Alpha = 1.0f;


            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
