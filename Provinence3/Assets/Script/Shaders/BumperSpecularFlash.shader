Shader "Custom/BumperSpecularFlash" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Specular Color", Color) = (0.0, 0.0, 0.0, 1)
		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_FlashColor("Flash Color", Color) = (1,1,1,1)
		_Time2("Time", Range(0.0, 1)) = 0.0
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 400

		CGPROGRAM
#pragma surface surf BlinnPhong

		sampler2D _MainTex;
	float4 _Color;
	float _Shininess;
	float4 _FlashColor;
	float _Time2;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half4 tex = tex2D(_MainTex, IN.uv_MainTex);


		o.Albedo = tex.rgb * _Color.rgb *_Time2 * _FlashColor;
		o.Gloss = tex.a;
		o.Alpha = tex.a * _Color.a;
		o.Specular = _Shininess;
		
	}
	ENDCG
	}

		FallBack "Specular"
}
