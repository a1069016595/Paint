 Shader "Custom/MoveUV" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTexShaZi ("Albedo (RGB)", 2D) = "white" {}
		_MainTexHeight ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_BumpMap("Bumpmap",2D)="bump"{}
	}
	SubShader {
	Tags{"Queue" = "Transparent" "RenderType"="Transparent"}
		pass{
		CGPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		float4 _MainTex_ST;
		sampler2D _MainTex;
		sampler2D _MainTexShaZi;
		sampler2D _MainTexHeight;
		float4 _Color;
		float val;
		sampler2D _BumpMap;
		float _Metallic;
		struct v2f
		{
			float4 pos:SV_POSITION;
			float2 uv:TEXCOORDO;
		};

		v2f vert (appdata_base v)
{
   //和之前一样
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
    return o;
}
		half4 frag (v2f i) : COLOR
       {

          //debug.log(u_x);
         // float2 uv_grass=float2(u_x,i.uv.y);
         // half4 texcolor_grass=tex2D(_MainTex,uv_grass);
        // float4 color=
       // float val=sin(_Time);
      // debug.log(_Time);
     // float val=sin(_Time*10);
      // _Color=(1,1,1,val);

        //float u_x=i.uv.x+-0.1*_Time*10;
        float2 uv_grass=float2(i.uv.x,i.uv.y);
      float4 texcolor_grass=tex2D(_MainTex,i.uv);
       
        float4 texcolor_height=tex2D(_MainTexHeight,i.uv);

		 float4 texcolor_sha=tex2D(_MainTexShaZi,i.uv);
       // texcolor_height=texcolor_height*sin(_Time);

       float4 result=lerp(texcolor_grass,texcolor_sha,_Metallic);
     
                    return result;
            //   float4 val={(0,0,1,1),(0,0,1,1),(0,0,1,1),(0,0,1,1)};
               // float4 val=(0,1,0,0);
               //return val; 
		}

		ENDCG
	}

  }
 }