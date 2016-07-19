Shader "Custom/FogInsideFlash"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Time2("Time", Range(0.0, 1)) = 0.0
	}
		SubShader
	{

		Pass
	{
		CGPROGRAM
		// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct appdata members worldPos)
		//#pragma exclude_renderers d3d11 xbox360
#pragma vertex vert
#pragma fragment frag

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

	static const fixed4 _Color = fixed4(0.38, 0.38, 0.38, 1);
	static const fixed4 _FlashColor = fixed4(1, 1, 1, 1);
	float b;
	sampler2D _MainTex;
	float _Time2;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.uv;
		o.position_in_world_space = mul(_Object2World, v.vertex);
		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
		//float dist = distance(i.position_in_world_space,  _WorldSpaceCameraPos);
		
		fixed4 col = tex2D(_MainTex, i.uv);
	//return col;

	b = (31 - i.position_in_world_space.y) / 1.6;	
		b = clamp(b, 0.0,1.0);

		//fixed4 col = tex2D(_MainTex, i.uv);
		//b = (_C2 - i.position_in_world_space.y) / diff;
		//b = clamp(b, 0.0, 1.0);


	return  lerp(col,_Color,b) *_Time2 * _FlashColor;

	}
		ENDCG
	}
	}
}