Shader "Custom/TransparentShield"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "black" {}
		_TextureAlpha("TextureAlpha", Range(0.0, 1)) = 0 
		_Threshold("Threshold", Range(0.0, 1)) = 1 
		_ViewThreshold("ViewThreshold", Range(0.0, 2)) = 1 
		_HighlightColor("HighlightColor", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { 
				"RenderType"="Transparent"
				"Queue" = "Transparent"
			}
		LOD 100

		Pass
		{
			Cull Off
			ZWrite Off
			ZTest On
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			//定义深度图，_CameraDepthTexture 存储的是屏幕深度信息
       		uniform sampler2D_float _CameraDepthTexture;

       		fixed _Threshold;
       		fixed _ViewThreshold;
       		fixed _TextureAlpha;

       		fixed4 _HighlightColor;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
            	float3 worldViewDir : TEXCOORD4;

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.worldNormal = UnityObjectToWorldNormal(v.normal);
            	o.worldViewDir = UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex));

				o.screenPos = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.screenPos.z);			// 同 -UnityObjectToViewPos( v.vertex ).z
				
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float4 final = tex2D(_MainTex, i.uv);
				final.a = _TextureAlpha;

				fixed3 worldNormal = normalize(i.worldNormal);
            	fixed3 worldViewDir = normalize(i.worldViewDir);

				//法线方向和视角方向的点积
            	fixed edge = 1 - abs(dot(worldNormal, worldViewDir)) * _ViewThreshold;
				
            	//根据自身的场景坐标采样深度图得到当前位置的场景深度
				float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
				float selfZ = i.screenPos.z;

				float intersect = 1- min ( (abs(sceneZ - selfZ)) / _Threshold, 1);		// 场景深度坐标和自身坐标接近 说明有相交

				return lerp(final,_HighlightColor,max(edge,intersect));
			}
			ENDCG
		}
	}
}