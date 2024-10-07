Shader "SketchSurface" {
    Properties {
        _MainTex("Main Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,0,0,1)
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _Outline("Outline Width", Range(0, 1)) = 0.005
        _LitTex("Light Hatch", 2D) = "white" {}
        _MedTex("Medium Hatch", 2D) = "white" {}
        _HvyTex("Heavy Hatch", 2D) = "white" {}
        _Repeat("Repeat Tile", float) = 4
        _ContourColor ("Contour Color", Color) = (0, 0, 0, 0.5)
        _ContourWidth ("Contour Width", Float) = 0.01
        _Amplitude ("Amplitude", Float) = 0.01
        _Speed ("Speed", Float) = 6.0
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
    
            CGPROGRAM
            #pragma surface surf ToonRamp //vertex:vert

            #pragma target 3.0

            sampler2D _MainTex;
            sampler2D _RampTex;
            sampler2D _LitTex;
            sampler2D _MedTex;
            sampler2D _HvyTex;
            float4 _Color;
            float _GeoRes;
            fixed _Repeat;

            struct MySurfaceOutput
            {
                fixed3 Albedo;
                fixed3 Normal;
                fixed3 Emission;
                fixed Gloss;
                fixed Alpha;
                fixed val;
                float2 screenUV;
            };

            float4 LightingToonRamp(MySurfaceOutput s, fixed3 lightDir, fixed atten)
            {

                half NdotL = dot(s.Normal, lightDir);

                half4 cLit = tex2D(_LitTex, s.screenUV);
                half4 cMed = tex2D(_MedTex, s.screenUV);
                half4 cHvy = tex2D(_HvyTex, s.screenUV);

                float4 t;

                half v = saturate(length(_LightColor0.rgb) * (NdotL * atten * 2) * s.val);

                float diff = (dot(s.Normal, lightDir) * 0.5 + 0.5) * atten;
                float h = diff * 0.5 + 0.5;
                float2 rh = h;
                float3 ramp = tex2D(_RampTex, rh).rgb;

                t.rgb = lerp(cHvy, cMed, v);
                t.rgb = lerp(t.rgb, cLit, v);
                t.rgb = s.Albedo * _LightColor0.rgb * ((lerp(t.rgb, cLit, v)) * (lerp(cHvy, cMed, v)) * ramp);
                t.a = s.Alpha;
                return t;
            }

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_RampTex;
                float2 uv_MainTex_ST;
                float2 uv_Tex;
                float3 viewDir;
                float3 rez;
                float4 screenPos;
            };


            void surf(Input IN, inout MySurfaceOutput o)
            {
                half4 c = tex2D(_MainTex, IN.uv_MainTex);

                //o.screenUV = IN.screenPos.xy * 4 / IN.screenPos.w * _Repeat;
                o.screenUV = IN.uv_MainTex * _Repeat;
                half v = length(tex2D(_MainTex, IN.uv_MainTex).rgb);
                o.val = v;

                o.Albedo = c.rgb * _Color;
                o.Alpha = c.a;

            }
            ENDCG

    

        // Pass for the outline effect
        Pass {
            Cull Front
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vertOutline
            #pragma fragment fragOutline

            struct appdata_t {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float4 scrpos : TEXCOORD0;
            };

            float hash (float2 seed) {
                return frac(sin(dot(seed.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 _ContourColor;
            half _ContourWidth, _Amplitude, _Speed;

            v2f vertOutline (appdata_t v) {
                v2f o;
                float4 offset = float4(v.normal, 0) * (_ContourWidth + _Amplitude * (hash(v.uv + floor(_Time.y * _Speed)) - 0.5));
                o.pos = UnityObjectToClipPos(v.vertex + offset);
                o.scrpos = o.pos;
                return o;
            }

            fixed4 fragOutline (v2f i) : SV_Target {
                return _ContourColor;
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}