Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _EdgeThreshold("Edge Threshold", Range(0, 1)) = 0.1
        _Color("Color", Color) = (0,0,0,1)
        _NoiseTex("Noise Texture", 2D) = "white" {} // Noise texture for edge deformation
        _NoiseScale("Noise Scale", Range(0.1, 10)) = 1.0
        _NoiseIntensity("Noise Intensity", Range(0, 10)) = 1.0 // Control the strength of the deformation
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                sampler2D _NoiseTex;
                float _EdgeThreshold;
                float4 _MainTex_TexelSize; // Automatically passed texel size
                float _NoiseScale;
                float _NoiseIntensity;
                float4 _Color;

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 pos : SV_POSITION;
                };

                v2f vert(appdata_full v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord;
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    float2 texelSize = _MainTex_TexelSize.xy;

                    // Sample surrounding pixels for Sobel edge detection
                    float3 top = tex2D(_MainTex, i.uv + float2(0, texelSize.y)).rgb;
                    float3 bottom = tex2D(_MainTex, i.uv + float2(0, -texelSize.y)).rgb;
                    float3 left = tex2D(_MainTex, i.uv + float2(-texelSize.x, 0)).rgb;
                    float3 right = tex2D(_MainTex, i.uv + float2(texelSize.x, 0)).rgb;

                    // Calculate differences (Sobel operator)
                    float3 dx = right - left;
                    float3 dy = top - bottom;
                    float edge = length(dx) + length(dy);

                    // Check if edge is strong enough
                    if (edge > _EdgeThreshold)
                    {
                        // Apply screen-space noise for edge deformation
                        float2 noiseUV = i.uv * _NoiseScale;
                        float2 noiseOffset = (tex2D(_NoiseTex, noiseUV).rg - 0.5) * (_NoiseIntensity * 2);
                        float2 distortedUV = i.uv + noiseOffset;

                        // Return the distorted edge (black outline)
                        return _Color;
                    }
                    else
                    {
                        // Return the original color where there's no edge
                        return tex2D(_MainTex, i.uv);
                    }
                }
                ENDCG
            }
        }
}
