// 溶解特效

Shader "Custom/DissolveEffect"
{
	Properties{
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_DissolveColor("Dissolve Color", Color) = (0,0,0,0)
		_DissolveEdgeColor("Dissolve Edge Color", Color) = (1,1,1,1)
		_MainTex("Base 2D", 2D) = "white"{}
		_DissolveMap("DissolveMap", 2D) = "white"{}
		_DissolveThreshold("DissolveThreshold", Range(0,1)) = 0
		_ColorFactor("ColorFactor", Range(0,5)) = 1
		_DissolveEdge("DissolveEdge", Range(0,1)) = 0.8
		_FlyThreshold("FlyThreshold", Range(0,1)) = 0.5
		_FlyFactor("FlyFactor", Range(0,1)) = 0.2
		_ExplodeThreshold("ExplodeThreshold", Range(0,1)) = 0.5
		_ExplodeFactor("_ExplodeFactor", Range(0,1)) = 0.2
	}

	CGINCLUDE
#include "Lighting.cginc"
	uniform fixed4 _Diffuse;
	uniform fixed4 _DissolveColor;
	uniform fixed4 _DissolveEdgeColor;
	uniform sampler2D _MainTex;
	uniform float4 _MainTex_ST;
	uniform sampler2D _DissolveMap;
	uniform float _DissolveThreshold;
	uniform float _ColorFactor;
	uniform float _DissolveEdge;
	uniform float _FlyThreshold;
	uniform float _FlyFactor;
	uniform float _ExplodeThreshold;
	uniform float _ExplodeFactor;

	//struct appdata_base {
	//	float4 vertex : POSITION;
	//	float3 normal : NORMAL;
	//	float4 texcoord : TEXCOORD0;
	//	UNITY_VERTEX_INPUT_INSTANCE_ID
	//};

	struct v2f
	{
		float4 pos : SV_POSITION;
		float3 worldNormal : TEXCOORD0;
		float2 uv : TEXCOORD1;
	};

	v2f vert(appdata_base v)
	{
		v2f o;

		//v.vertex.xyz += v.normal * _DissolveThreshold * 0.5;
		//o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);

		v.vertex.xyz += v.normal * saturate(_DissolveThreshold - _ExplodeThreshold) * _ExplodeFactor;

		float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
		worldPos.y += saturate(_DissolveThreshold - _FlyThreshold) * _FlyFactor;
		o.pos = UnityWorldToClipPos(worldPos);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		//采样Dissolve Map
		fixed4 dissolveValue = tex2D(_DissolveMap, i.uv);
		//小于阈值的部分直接discard
		if (dissolveValue.r < _DissolveThreshold)
		{
			discard;
		}

		fixed3 color = tex2D(_MainTex, i.uv).rgb * _Diffuse;

		//fixed3 burn = tex2D(_BurnMap, i.uvBurnMap).rgb;
		//clip(burn.r - _BurnAmount);		// 如果小于0，该像素会被剔除

		//_DissolveEdge宽度范围模拟渐变。t为0是正常颜色，为1时位于消融的边界
		fixed t = 1 - smoothstep(0.0, _DissolveEdge, dissolveValue.r - _DissolveThreshold);
		//边缘颜色和渐变颜色做插值
		fixed3 edgeColor = lerp(_DissolveColor, _DissolveEdgeColor, t);	
		//缩放颜色变化值
		edgeColor = edgeColor * _ColorFactor;							
		//原始纹理颜色和边缘颜色插值
		fixed3 finalColor = lerp(color, edgeColor, t * step(0.0001, _DissolveThreshold));

		return fixed4(finalColor, 1);
	}
	ENDCG

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag	
			ENDCG
		}
	}
	FallBack "Diffuse"
}