/********************************************************************
 FileName: DeferredDecal.shader
 Description: 延迟贴花效果
 history: 5:1:2019 by puppet_master
 https://blog.csdn.net/puppet_master
*********************************************************************/
Shader "Decal/DeferredDecal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NormalClipThreshold("NormalClip Threshold", Range(0,0.3)) = 0.1
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent+100"}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
 
			struct appdata
			{
				float4 vertex : POSITION;
			};
 
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
				float3 ray : TEXCOORD2;
				float3 yDir : TEXCOORD3;
			};
 
			sampler2D _MainTex;
			sampler2D_float _CameraDepthTexture;
			sampler2D _CameraGBufferTexture2;
			float _NormalClipThreshold;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.ray =UnityObjectToViewPos(v.vertex) * float3(-1,-1,1);
				//将立方体y轴方向转化到世界空间，GBuffer的Normal是世界空间的
				o.yDir = mul((float3x3)unity_ObjectToWorld, float3(0,1,0));
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//深度重建视空间坐标
				float2 screenuv = i.screenPos.xy / i.screenPos.w;
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenuv);
				float linear01Depth = Linear01Depth(depth);
				float viewDepth = linear01Depth * _ProjectionParams.z;
				float3 viewPos = i.ray * viewDepth / i.ray.z;
				//转化到世界空间坐标
				float4 worldPos = mul(unity_CameraToWorld, float4(viewPos, 1.0));
				//转化为物体空间坐标
				float3 objectPos = mul(unity_WorldToObject, worldPos);
				//剔除掉在立方体外面的内容
				clip(float3(0.5, 0.5, 0.5) - abs(objectPos));
				
				//GBuffer取世界空间法线方向
				float3 worldNormal = tex2D(_CameraGBufferTexture2, screenuv).rgb * 2.0 - 1.0;
				//根据法线方向剔除与xz垂直的面投影的内容
				float3 yDir = normalize(i.yDir);
				clip(dot(yDir, worldNormal) - _NormalClipThreshold);
				
				//使用物体空间坐标的xz坐标作为采样uv
				float2 uv = (objectPos.xz + 0.5);
				
				fixed4 col = tex2D(_MainTex, uv);
				return col;
			}
			ENDCG
		}
	}
}
