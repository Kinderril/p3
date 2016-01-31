 
 
 
Shader "Hidden/MyGlobalFogShader" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "black" {}
}
 
CGINCLUDE
 
    #include "UnityCG.cginc"
 
    uniform sampler2D _MainTex;
    uniform sampler2D _CameraDepthTexture;
   
    uniform float _GlobalDensity;
    uniform float4 _FogColor;
    uniform float4 _StartDistance;
    uniform float4 _Y;
    uniform float4 _MainTex_TexelSize;
   
    // for fast world space reconstruction
   
    uniform float4x4 _FrustumCornersWS;
    uniform float4 _CameraWS;
    struct v2f {
        float4 pos : POSITION;
        float2 uv : TEXCOORD0;
        float4 interpolatedRay : TEXCOORD1;
        float2 uv2: TEXCOORD2;
    };
    bool eq(float a,float b)
    {
        return abs(a-b) < 0.01;
    }
    v2f vert( appdata_img v )
    {
        v2f o;
        float xx = v.texcoord.x;
        float yy = v.texcoord.y;
 
        float2 uv = v.texcoord.xy;
        o.uv = uv;
        #if UNITY_UV_STARTS_AT_TOP
        if (_MainTex_TexelSize.y < 0)
        {
            uv.y = 1-uv.y;
        }
        else
        {
            yy = 1 - yy;
        }
        #endif
       
        int index = (int)floor(xx+yy*2 + 0.5);
        o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
       
        o.uv2 = uv;
       
        o.interpolatedRay = _FrustumCornersWS[(int)index];
        o.interpolatedRay.w = index;
       
        return o;
    }
   
    float ComputeFogForYAndDistance (in float3 camDir, in float3 wsPos)
    {
        float fogInt = saturate(length(camDir) * _StartDistance.x-1.0) * _StartDistance.y; 
        float fogVert = max(0.0, (wsPos.y-_Y.x) * _Y.y);
        fogVert *= fogVert;
        return  (1-exp(-_GlobalDensity*fogInt)) * exp (-fogVert);
    }
   
    half4 fragAbsoluteYAndDistance (v2f i) : COLOR
    {
        float dpth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,i.uv2)));
        float4 wsDir = dpth * i.interpolatedRay;
        float4 wsPos = _CameraWS + wsDir;
        return lerp(tex2D(_MainTex, i.uv), _FogColor, ComputeFogForYAndDistance(wsDir.xyz,wsPos.xyz));
    }
 
    half4 fragRelativeYAndDistance (v2f i) : COLOR
    {
        float dpth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,i.uv2)));
        float4 wsDir = dpth * i.interpolatedRay;
        return lerp(tex2D(_MainTex, i.uv), _FogColor, ComputeFogForYAndDistance(wsDir.xyz, wsDir.xyz));
    }
 
    half4 fragAbsoluteY (v2f i) : COLOR
    {
        float dpth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,i.uv2)));
        float4 wsPos = (_CameraWS + dpth * i.interpolatedRay);
        float fogVert = max(0.0, (wsPos.y-_Y.x) * _Y.y);
        fogVert *= fogVert;
        fogVert = (exp (-fogVert));
        return lerp(tex2D( _MainTex, i.uv ), _FogColor, fogVert);              
    }
 
    half4 fragDistance (v2f i) : COLOR
    {
        float dpth = Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture,i.uv2)));      
        float4 camDir = ( dpth * i.interpolatedRay);
        float fogInt = saturate(length( camDir ) * _StartDistance.x - 1.0) * _StartDistance.y; 
        return lerp(_FogColor, tex2D(_MainTex, i.uv), exp(-_GlobalDensity*fogInt));            
    }
 
ENDCG
 
SubShader {
    Pass {
        ZTest Always Cull Off ZWrite Off
        Fog { Mode off }
 
        CGPROGRAM
 
        #pragma vertex vert
        #pragma fragment fragAbsoluteYAndDistance
        #pragma fragmentoption ARB_precision_hint_fastest
       
        ENDCG
    }
 
    Pass {
        ZTest Always Cull Off ZWrite Off
        Fog { Mode off }
 
        CGPROGRAM
 
        #pragma vertex vert
        #pragma fragment fragAbsoluteY
        #pragma fragmentoption ARB_precision_hint_fastest
       
        ENDCG
    }
 
    Pass {
        ZTest Always Cull Off ZWrite Off
        Fog { Mode off }
 
        CGPROGRAM
 
        #pragma vertex vert
        #pragma fragment fragDistance
        #pragma fragmentoption ARB_precision_hint_fastest
       
        ENDCG
    }
 
    Pass {
        ZTest Always Cull Off ZWrite Off
        Fog { Mode off }
 
        CGPROGRAM
 
        #pragma vertex vert
        #pragma fragment fragRelativeYAndDistance
        #pragma fragmentoption ARB_precision_hint_fastest
       
        ENDCG
    }
}
 
Fallback off
 
}