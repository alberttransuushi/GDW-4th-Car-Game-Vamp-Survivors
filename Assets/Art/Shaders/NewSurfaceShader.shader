Shader "HelloShader"{

	Properties{
		_myColor("Sample color", Color) = (1,1,1,1)
		_myEmission("Sample Emission", Color) = (1,1,1,1)
		_myNormal("Sample Normal", Color) = (1,1,1,1)
	}
		SubShader{
			CGPROGRAM
			#pragma surface surf Lambert

			struct Input {
			float2 uvMainTex;
		};

			fixed4 _myColor;
			fixed4 _myEmission;
			fixed4 _myNormal;

			void surf(Input IN, inout SurfaceOutput o) {
				o.Albedo = _myColor.rgb;
				o.Emission = _myEmission.rgb;
				o.Normal = _myNormal;
			}
			ENDCG
	}
	FallBack "Diffuse"
}