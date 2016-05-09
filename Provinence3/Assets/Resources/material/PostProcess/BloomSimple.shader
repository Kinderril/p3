Shader "PostProcess/BloomSimple"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_SubTex(" Base2 (RGB)", 2D) = "white" {}
		_Strength("Bloom Strength", Float) = 0.5
		_Color("Tint", Color) = (1,1,1,1)
	}
		SubShader{
			Pass{
			ZTest Always Cull Off ZWrite Off Lighting Off Fog{ Mode off }
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _SubTex;
			float _Strength;
			fixed4 _Color;

			half3 frag(v2f_img i) : COLOR{
				float4 col = tex2D(_SubTex, i.uv);
				float4 bloom = col;
				col.rgb = pow(bloom.rgb, _Strength);
				col.rgb *= bloom;
				col.rgb += bloom;
				float4 result = col * _Color;
				//return result;
				half4 precompose = result;
				half3 sceneTexture = tex2D(_MainTex, i.uv);
				half3 resultEnd = sceneTexture.rgb /* *precompose.a*/ + precompose.rgb;

				return resultEnd;
				//half4 precompose = tex2D(PreComposeSampler, i.uv);
				//half3 sceneTexture = tex2D(SceneSampler, i.uv);
				//half3 result = sceneTexture.rgb*precompose.a + precompose.rgb;
			}
			ENDCG
		}
	}
	Fallback off
}