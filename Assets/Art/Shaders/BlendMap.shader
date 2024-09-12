Shader "Custom/BlendMap"

    {
        Properties{
            _TileTextureR("TileTexture R (RGB)", 2D) = "w$$anonymous$$te" {}
            _TileTextureG("TileTexture G (RGB)", 2D) = "w$$anonymous$$te" {}
            _TileTextureB("TileTexture B (RGB)", 2D) = "w$$anonymous$$te" {}
            _TileTextureA("TileTexture A (RGB)", 2D) = "w$$anonymous$$te" {}
            _RampTex("Ramp Texture", 2D) = "white" {}
            _BlendTex("Blend (RGB)", 2D) = "red" {}
            _RimColor("Rim Color", Color) = (0,0.5,0.5,0)
            _RimPower("Rim Power", Range(0.5,8.0)) = 3.0
        }
        
            SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

                CGPROGRAM
                #pragma surface surf ToonRamp

                // Use shader model 3.0 target, to get nicer looking lighting
                #pragma target 3.0

                sampler2D _TileTextureR;
                sampler2D _TileTextureG;
                sampler2D _TileTextureB;
                sampler2D _TileTextureA;
                sampler2D _BlendTex;
                sampler2D _RampTex;
                float4 _RimColor;
                float _RimPower;


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


                struct Input {
                    float2 uv_TileTextureR;
                    float2 uv_TileTextureA;
                    float2 uv_BlendTex;
                };



                void surf(Input IN, inout SurfaceOutput o)
                {
                    fixed4 blend = tex2D(_BlendTex, IN.uv_BlendTex);

                    //blendmaps with textures
                    fixed4 c =
                    tex2D(_TileTextureR, IN.uv_TileTextureR) * blend.r +
                    tex2D(_TileTextureG, IN.uv_TileTextureR) * blend.g +
                    tex2D(_TileTextureB, IN.uv_TileTextureR) * blend.b +
                    tex2D(_TileTextureA, IN.uv_TileTextureA) * abs(1 - blend.a);

                    //blendmaps with color
                    //fixed4 c = _RedColor * blend.r + _BlueColor * blend.b + _GreenColor * blend.g + _AlphaColor * abs(1 - blend.a);
                    o.Albedo = c.rgb;

                    half rim = 1 - saturate(dot(normalize(IN.uv_TileTextureR), o.Normal));
                    o.Emission = _RimColor.rgb * pow(rim, _RimPower) * 1.2;
                }
        ENDCG
    }
        FallBack "Diffuse"
}