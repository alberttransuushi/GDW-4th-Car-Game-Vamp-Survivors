Shader "Custom/ToonRampOutline"
{

    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline Width", Range (0.05, 1)) = 0.005
    }
        SubShader
    {
        CGPROGRAM
        #pragma surface surf ToonRamp

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _RampTex;

        float4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            float diff = (dot(s.Normal, lightDir) * 0.5 + 0.5) * atten;
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
        };



        void surf(Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;

        }
        ENDCG

        Pass
        {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {

                float4 vertex : POSITION;
                float3 normal : NORMAL;

            };

            struct v2f {

                float4 pos : SV_POSITION;
                float4 color : COLOR;
                
            };

            float _Outline;
            float4 _OutlineColor;

            v2f vert(appdata v)
            {

                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float2 offset = TransformViewToProjection(norm.xy);

                o.pos.xy += offset * o.pos.z * _Outline;
                o.color = _OutlineColor;
                return o;

            }

            float4 frag(v2f i) : SV_Target
            {
                return i.color;

            }
            ENDCG
           

        }
    }
    FallBack "Diffuse"
}