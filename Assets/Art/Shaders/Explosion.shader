Shader "Custom/Explosion"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _DisplacementMap("DisMapInYouMouth", 2D) = "black" {}
        _DisplacmentStrength("DisStronkness", Range(0,1)) = 0.3
    }
        SubShader
        {
            Pass
            {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"


            fixed4 _Color;
            sampler2D _MainTex;
            sampler2D _DisplacementMap;
            half _DisplacmentStrength;
            float4 _MainTex_ST;
            float4 _DisplacmentMap_ST;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f {
                float2 uv :TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {

                v2f o;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float displacement = tex2Dlod(_DisplacementMap, float4(o.uv, 0, 0)).r;
                //float displacement = 0;
                float4 temp = float4(v.vertex.x, v.vertex.y, v.vertex.z, 1.0);
                temp.xyz += displacement * v.normal * _DisplacmentStrength * _Time;

                //temp.xyz += displacement * 1.0 * 1.0;

                o.vertex = UnityObjectToClipPos(temp);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target{

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG

            }
        }
}