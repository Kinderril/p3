Shader "Custom/FogInside2"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		//_Color("Main Color", Color) = (0.32,0.32,0.32,1)
		// Control Texture ("Splat Map")
		[HideInInspector] _Control("Control (RGBA)", 2D) = "red" {}

		// Terrain textures - each weighted according to the corresponding colour
		// channel in the control texture
		[HideInInspector] _Splat3("Layer 3 (A)", 2D) = "white" {}
		[HideInInspector] _Splat2("Layer 2 (B)", 2D) = "white" {}
		[HideInInspector] _Splat1("Layer 1 (G)", 2D) = "white" {}
		[HideInInspector] _Splat0("Layer 0 (R)", 2D) = "white" {}
		// Let the user assign a lighting ramp to be used for toon lighting
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
	}
		SubShader
	{

CGPROGRAM
#pragma surface surf ToonRamp vertex:vert
#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION0;
		float4 position_in_world_space : NORMAL;
	};
	static const fixed4 _Color = fixed4(0.18, 0.18, 0.18,1);
	//uniform float4 _Color = (0.32, 0.32, 0.32, 1);
	uniform float diff;
	uniform float _C2;
	float b;
	sampler2D _MainTex;
	uniform sampler2D _Control;
	uniform sampler2D _Splat0, _Splat1, _Splat2, _Splat3;
	uniform sampler2D _Ramp;
	//uniform fixed4 _Color;
	

	/*
	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.uv;
		o.position_in_world_space = mul(_Object2World, v.vertex);
		return o;
	}*/



	// Surface shader input structure
	struct Input {
		float3 localPos;
		float2 uv_Control : TEXCOORD0;
		float2 uv_Splat0 : TEXCOORD1;
		float2 uv_Splat1 : TEXCOORD2;
		float2 uv_Splat2 : TEXCOORD3;
		float2 uv_Splat3 : TEXCOORD4;
	};

	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif
		// Wrapped lighting
		//float4 aa = s.Position;
		half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
		// Applied through ramp
		//half3 ramp = tex2D(_Ramp, float2(d, d)).rgb;
		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * (atten * 2);
		c.a = 0;
		return c;
	}

	void surf(Input IN, inout SurfaceOutput o) {
		float3 pp = IN.localPos;

		fixed4 splat_control = tex2D(_Control, IN.uv_Control);
		fixed3 col;
		col = splat_control.r * tex2D(_Splat0, IN.uv_Splat0).rgb;
		col += splat_control.g * tex2D(_Splat1, IN.uv_Splat1).rgb;
		col += splat_control.b * tex2D(_Splat2, IN.uv_Splat2).rgb;
		//col += splat_control.a * tex2D(_Splat3, IN.uv_Splat3).rgb;


		float b = (31 - pp.y + 2) / 5.7;
		b = clamp(b, 0.0, 1.0);
		col = lerp(col, _Color, b);

		o.Albedo = col;// (0.32, 0.32, 0.32, 1);
		o.Alpha = 0.5;
	} 
	
	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.localPos = v.vertex.xyz;
	}

	ENDCG
	}
}