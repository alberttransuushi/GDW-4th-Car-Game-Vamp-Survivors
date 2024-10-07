Shader "Custom/Waves" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RampTex ("Toon Shading Tex", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline Width", Range(0.05, 1)) = 0.005
		_LitTex("Light Hatch", 2D) = "white" {}
		_MedTex("Medium Hatch", 2D) = "white" {}
		_HvyTex("Heavy Hatch", 2D) = "white" {}
		_Repeat("Repeat Tile", float) = 4
		_DisplacementStrength("Displancement Strength", Range(0,10)) = 0.5
		_xOffset("x Offset", Range(-10,10)) = 1
		_yOffset("y Offset", Range(-10,10)) = 1
		_zOffset("z Offset", Range(-10,10)) = 1
		_TimeShift("time shift affect", Range(0,1000)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf ToonRamp vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _Color;
		sampler2D _RampTex;
		sampler2D _LitTex;
		sampler2D _MedTex;
		sampler2D _HvyTex;
		fixed _Repeat;
		half _DisplacementStrength;
		half _xOffset;
		half _yOffset;
		half _zOffset;
		half _TimeShift;

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

		struct Input {
			float2 uv_MainTex;
			float2 uv_RampTex;
			float4 screenPos;
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

		

		void vert(inout appdata_full v) {
			float displacement = (sqrt(pow(v.vertex.x + _xOffset, 2) + pow(v.vertex.y + _yOffset, 2) + pow(v.vertex.z + _zOffset, 2)) + _TimeShift * (_Time/30)) % 2;
			if (displacement < 1) {
				displacement = 0;
			}
			else {
				displacement = 1;
			}
			v.vertex.y = displacement * _DisplacementStrength;
		}

		void surf (Input IN, inout MySurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			o.screenUV = IN.screenPos.xy * 4 / IN.screenPos.w;
			half v = length(tex2D(_MainTex, IN.uv_MainTex).rgb) * _Repeat;
			o.val = v;

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}