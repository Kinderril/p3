Shader "Custom/HeroShader" {

	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_RimColor("Rim Color", Color) = (0.97,0.88,1,0.75)
		_RimPower("Rim Power", Float) = 2.5
		_Fresnel("Fresnel Value", Float) = 0.28
		_MainTex("Base (RGB)", 2D) = "white" {}
	_BumpMap("Bump (RGB)", 2D) = "bump" {}
	_SpecularTex("Specular Level (R) Gloss (G)", 2D) = "gray" {}
	_RimTex("Rim ramp (RGB) Fresnel ramp (A)", 2D) = " grey" {}
	_WrapTex("Wrap ramp (RGBA)", 2D) = "black" {}

	_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }

		AlphaTest Greater[_Cutoff]
		Blend Off
		Cull Off

		CGPROGRAM
#pragma surface surf BumpSpecSkin
#include "UnityCG.cginc"

		float4 _Color;
	float _Shininess;
	sampler2D _MainTex;
	sampler2D _WrapTex;
	sampler2D _RimTex;
	sampler2D _BumpMap;
	sampler2D _SpecularTex;
	float4 _RimColor;
	float _RimPower;
	float _Fresnel;

	inline float CalcFresnel(float3 viewDir, float3 h, float fresnelValue)
	{
		float fresnel = pow(1.0 - dot(viewDir, h), 5.0);
		fresnel += fresnelValue * (1.0 - fresnel);
		return fresnel;
	}

	half4 LightingBumpSpecSkin(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
	{
		float rimf = dot(s.Normal, viewDir);
		half4 rim = tex2D(_RimTex, rimf.xx);
		half3 h = normalize(lightDir + viewDir);
		float fresnel = CalcFresnel(viewDir, h, lerp(0.2, _Fresnel, s.Specular));
		half diffusePos = dot(s.Normal, lightDir) * 0.5 + 0.5;
		half4 diffuse = tex2D(_WrapTex, diffusePos.xx);
		diffuse.rgb *= rim.rgb;
		float nh = saturate(dot(h, s.Normal));
		float spec = pow(nh, 128 * s.Gloss) * s.Specular * fresnel;
		half4 c;
		c.rgb = ((s.Albedo + spec * rim.a) * (diffuse + diffusePos) + spec)* (atten * 2) * _LightColor0.rgb;
		c.a = s.Alpha;

		return c;
	}

	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float3 viewDir;
	};


	void surf(Input IN, inout SurfaceOutput o) {
		half4 texcol = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = texcol.rgb * _Color.rgb;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		float3 specMap = tex2D(_SpecularTex, IN.uv_MainTex).rgb;
		o.Specular = specMap.r;
		o.Gloss = specMap.g;
		half3 rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
		o.Emission = _RimColor.rgb * pow(rim, _RimPower);
		o.Alpha = texcol.a * _Color.a;
	}


	ENDCG



	}

		Fallback "VertexLit"
}