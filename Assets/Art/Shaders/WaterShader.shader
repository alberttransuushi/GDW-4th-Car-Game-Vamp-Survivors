Shader "Custom/WaterShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        _RotationSpeed("Rotation Speed", Float) = 2.0
        _RotationDegrees("Rotation Degrees", Float) = 0.0
        _RimPower("Rim Power", range(0.1, 10.0)) = 100.0
        _RimColor("Rim Color", Color) = (0,0.5,0.5,0.0)
        _Color("Color", Color) = (0,0.5,0.5,0.0)
    }
        SubShader
        {
            Tags {"RenderType" = "Geometry"}
            LOD 200

            Pass {
                Zwrite ON
                ColorMask 0
            }
            CGPROGRAM
            #pragma surface surf ToonRamp Lambert alpha:fade vertex:vert 

            sampler2D _MainTex;
            sampler2D _RampTex;
            float4 _RimColor;
            float4 _Color;

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
                float3 viewDir;
            };

            float _RotationSpeed;
            void vert(inout appdata_full v) {
                v.texcoord.xy -= 0.5;
                float s = sin(_RotationSpeed * _Time);
                float c = cos(_RotationSpeed * _Time);
                float2x2 rotationMatrix = float2x2(c, -s, s, c);
                rotationMatrix *= 0.5;
                rotationMatrix += 0.5;
                rotationMatrix = rotationMatrix * 2 - 1;
                v.texcoord.xy = mul(v.vertex.xy, rotationMatrix);
                v.texcoord.xy += 0.5;
            }

            float _RimPower;
            void surf(Input IN, inout SurfaceOutput o)
            {
                half rim = 1 - (dot(IN.viewDir, o.Normal));
                half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                //o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
                o.Emission = c.rgb * pow(rim, _RimPower) * _RimColor;
                o.Albedo = c.rgb;
                o.Alpha = pow(rim, _RimPower);

            }
            ENDCG
        }
            FallBack "Diffuse"
}
