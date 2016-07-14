Shader "Custom/TerrainWithFog"{
	Properties{

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

	// Used in fallback on old cards & also for distant base map
	[HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "white" {}
	[HideInInspector] _Color("Main Color", Color) = (1,1,1,1)


	}

		SubShader{
		Tags{
		"SplatCount" = "4"
		"Queue" = "Geometry-100"
		"RenderType" = "Opaque"
	}

		// TERRAIN PASS 
		CGPROGRAM
#pragma surface surf ToonRamp exclude_path:prepass 

	// Access the Shaderlab properties
	uniform sampler2D _Control;
	uniform sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
	uniform fixed4 _Color;
	uniform sampler2D _Ramp;

	// Custom lighting model that uses a texture ramp based
	// on angle between light direction and normal
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif
		// Wrapped lighting
		//float4 aa = s.Position;
		half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
		// Applied through ramp
		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;
		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
		c.a = 0;
		return c;
	}
	
	// Surface shader input structure
	struct Input {
		//float3 worldPos : SV_POSITION;
		float2 uv_Control : TEXCOORD0;
		float2 uv_Splat0 : TEXCOORD1;
		float2 uv_Splat1 : TEXCOORD2;
		float2 uv_Splat2 : TEXCOORD3;
		float2 uv_Splat3 : TEXCOORD4;
	};

	// Surface Shader function
	void surf(Input IN, inout SurfaceOutput o) {
		//float4 pp = IN.worldPos;

		fixed4 splat_control = tex2D(_Control, IN.uv_Control);
		fixed3 col;
		col = splat_control.r * tex2D(_Splat0, IN.uv_Splat0).rgb;
		col += splat_control.g * tex2D(_Splat1, IN.uv_Splat1).rgb;
		col += splat_control.b * tex2D(_Splat2, IN.uv_Splat2).rgb;
		col += splat_control.a * tex2D(_Splat3, IN.uv_Splat3).rgb;


		//float b = (31 - pp.y) / 1.5;
		//b = clamp(b, 0.0, 1.0);
		//col = lerp(col, _Color, b);

		o.Albedo = col *(0.32, 0.32, 0.32, 1);
		o.Alpha = 0.0;
	}
	ENDCG


	} // End SubShader

	  // Specify dependency shaders   
		Dependency "AddPassShader" = "Custom/ToonTerrainAddPass"
		Dependency "BaseMapShader" = "Toon/Lighted Outline"

		// Fallback to Diffuse
		Fallback "Diffuse"

} // Ehd Shader