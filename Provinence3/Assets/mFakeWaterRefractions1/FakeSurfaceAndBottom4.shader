// fake river shader test v4.0 - 10.4.2012 - mgear - http://unitycoder.com/blog/

Shader "mShaders/FakeSurfaceAndBottom4" {
    Properties {
	_WaterSpeed ("WaterSpeed", Float) = 0.2
	_FakeDepth ("FakeDepth", Float) = 0.05
	
      _MainTex ("SurfaceWaveTexture", 2D) = "white" {}
      _BottomTex ("BottomTexture", 2D) = "white" {}
	  
	  //_StonePos ("StonePos", Vector) = (0,0,0,1)
	   
	  _Cube ("Cubemap", CUBE) = "" {}
	  
//	  _FoamTex ("FoamTexture (a)", 2D) = "white" {}
//	  _SpeedMask ("SpeedMask (a)", 2D) = "white" {}
	  //http://www.imageafter.com
	  

    }
    SubShader {
   // ZWrite Off //n ZTest Equal  
//	Cull Off
	
//    Offset 1, 1
	
      Tags { "Queue"="Geometry"  "IgnoreProjector"="True" "RenderType" = "Opaque" }
      CGPROGRAM
   //   #pragma surface surf WrapLambert alpha novertexlights
		#pragma target 3.0 
		#pragma surface surf Lambert alpha 
		//vertex:vert
//		#pragma surface surf BlinnPhong 
//		#pragma surface surf SimpleSpecular
		
		
		#include "UnityCG.cginc"
/*
      half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half atten) {
          half NdotL = dot (s.Normal, lightDir);
          half diff = NdotL * 0.5 + 0.5;
          half4 c;
          c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 3);
          c.a = s.Alpha;
          return c;
      }
*/
	  




      struct Input {
        float2 uv_MainTex;
        float2 uv_BottomTex;
//        float2 uv_FoamTex;
//        float2 uv_SpeedMask;
		float3 worldPos;
		float3 worldRefl;
		float3 worldNormal;
		//float4 StonePos;
		INTERNAL_DATA
      };
	  
	  /*
       void vert (inout appdata_full v) {
        float sinOff=v.vertex.x+v.vertex.y+v.vertex.z;
        float t=-_Time*2;
        float fx=v.texcoord.x;
        float fy=v.texcoord.x*v.texcoord.y;
//        v.vertex.x+=sin(t*1.45+sinOff)*fx*0.5;
        v.vertex.y=sin(t*3.12+sinOff)*fx*0.01-fy*0.01;
//        v.vertex.z-=sin(t*2.2+sinOff)*fx*0.2; 
      }	  
	  */
	  
//      sampler2D _FoamTex;
      sampler2D _MainTex;
      sampler2D _BottomTex;
//      sampler2D _SpeedMask;
	  samplerCUBE _Cube;

	  uniform float _WaterSpeed;
	  uniform float _FakeDepth;
	  //uniform float4 _StonePos;
	 // float _MinDist;
	//	float _MaxDist;


     // void vert (inout appdata_full v, out Input o) {
         // o.customColor = abs(v.normal);
		 //o.StonePos = mul (glstate.matrix.modelview[0], _StonePos);
		 
    //  }
	  
	  

	  
      void surf (Input IN, inout SurfaceOutput o) 
	  {
	  
				// top
//				half speedMask = 1-tex2D (_SpeedMask, IN.uv_SpeedMask).rgb;
				
//				float waveslide = (_Time.x*_WaterSpeed)*speedMask;
//				float waveslide = _WaterSpeed*speedMask*_Time.x;
				float waveslide = _WaterSpeed*_Time.x;
				half3 col_orig = tex2D(_MainTex, float2(IN.uv_MainTex.x,IN.uv_MainTex.y+waveslide)).rgb*_FakeDepth;
				
				half3 col1 = tex2D(_BottomTex, float2(IN.uv_MainTex.x+col_orig.b,IN.uv_MainTex.y+col_orig.g)).rgb*1;
//				half3 norm1 = UnpackNormal (tex2D (_MainTex, IN.uv_MainTex));
				half3 norm1 = UnpackNormal (tex2D (_BottomTex, IN.uv_BottomTex));
				half3 emis1 = texCUBE (_Cube, WorldReflectionVector (IN, o.Normal)).rgb;
				//half3 emis1 = texCUBE (_Cube, IN.worldRefl).rgb;
				
				
				// add foam, doesnt work if variable speed, 
//				half4 foam1 = tex2D (_FoamTex, float2(IN.uv_FoamTex.x+(col_orig.b*0.5),IN.uv_FoamTex.y+(col_orig.g*0.5)+waveslide)).a;
//				half4 foam1 = tex2D (_FoamTex, float2(IN.uv_FoamTex.x+(col_orig.b*0.5),IN.uv_FoamTex.y+(col_orig.g*0.5)+waveslide)).a;
				
				// bottom
				half3 col2 = tex2D (_BottomTex, IN.uv_BottomTex).rgb*1.5;
				//half3 norm2 = UnpackNormal (tex2D (_BottomTex, IN.uv_BottomTex));
			
//				float distance = length(_StonePos - IN.worldPos);
				//float distcol = 0; //_StonePos.xyz - IN.worldPos.xyz;
				//if (_StonePos.z>IN.worldPos.z) distcol = 100;

			// top surface
  			if (IN.worldPos.y>=58.6)
			{
			
				
//				o.Albedo = tex2D (_MainTex, float2(IN.uv_MainTex.x,IN.uv_MainTex.y+(_Time.x*0.3))).rgb;
				half3 col = col1+col1+float3(0,0,-0.07);
				o.Albedo = col; //*distance; //float3(distcol,0,0); //col*distcol;
				// alpha depends on distance from middle? (rannassa l‰pin‰kym‰mp‰‰)
				o.Alpha = 0.3+norm1*0.2+col;
			//	o.Specular = 0;
			//	o.Gloss = 0;
				
				// cubemap, should have transparent areas? use col_orig mask?
				
//				o.Emission = texCUBE (_Cube, IN.worldRefl).rgb*0.3;
				o.Normal = norm1;
				o.Emission = emis1*emis1; //+foam1*0.5;
				
			}else{
				// bottom texture (and sides?)
			
				//o.Albedo = float3(0,1,0); 
				

				
				
				o.Albedo = col2+col2; //*(saturate(IN.worldPos.y)*4);
				
				o.Alpha = 1;
				//o.Normal = norm2; 
				
			}
			
			//o.Normal = UnpackNormal(tex2D(_MainTex, IN.uv_MainTex));
		  
      }
      ENDCG
    }
    Fallback "Diffuse"
  }