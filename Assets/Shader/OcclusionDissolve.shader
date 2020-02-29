
//遮挡溶解效果
 
Shader "Custom/OcclusionDissolve"
{
	Properties{
		_CenterX("Center X", float) = 0.5
		_CenterY("Center Y", float) = 0.5
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_DissolveColorA("Dissolve Color A", Color) = (0,1,1,0)
		_DissolveColorB("Dissolve Color B", Color) = (0.3,0.3,0.3,1)
		_MainTex("Base 2D", 2D) = "white"{}
		_DissolveMap("DissolveMap", 2D) = "white"{}
		_DissolveThreshold("DissolveThreshold", float) = 2
		_DissolveDistance("DissolveDistance", float) = 14
	}
	
	CGINCLUDE
	#include "Lighting.cginc"
	uniform fixed _CenterX;
	uniform fixed _CenterY;
	uniform fixed4 _Diffuse;
	uniform fixed4 _DissolveColorA;
	uniform fixed4 _DissolveColorB;
	uniform sampler2D _MainTex;
	uniform float4 _MainTex_ST;
	uniform sampler2D _DissolveMap;
	uniform float _DissolveThreshold;
	uniform float _DissolveDistance;
	uniform float _DissolveDistanceFactor;
	
	struct v2f
	{
		float4 pos : SV_POSITION;
		float3 worldNormal : TEXCOORD0;
		float2 uv : TEXCOORD1;
		float4 screenPos : TEXCOORD2;
		float3 viewDir : TEXCOORD3;
	};
	
	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
		o.pos = UnityObjectToClipPos(v.vertex);
		o.viewDir = ObjSpaceViewDir(v.vertex);
		//计算屏幕坐标
		o.screenPos = ComputeGrabScreenPos(o.pos);
		return o;
	}
	
	fixed4 frag(v2f i) : SV_Target
	{
		float2 screenPos = i.screenPos.xy / i.screenPos.w;
		//计算距离中心点距离作为一个控制系数
		float2 dir = float2(_CenterX, _CenterY) - screenPos;
		float screenSpaceDistance = 0.5 - sqrt(dir.x * dir.x + dir.y * dir.y);
		//计算一下像素点到相机距离作为另一个控制系数
		float viewDistance =  max(0,(_DissolveDistance - length(i.viewDir)) / _DissolveDistance) ;
		//用两个控制系数作为最终溶解的系数
		float disolveFactor = viewDistance * screenSpaceDistance * _DissolveThreshold;
		//采样Dissolve Map
		fixed4 dissolveValue = tex2D(_DissolveMap, i.uv);
		//小于阈值的部分直接discard
		if (dissolveValue.r < disolveFactor)
		{
			discard;
		}
		//Diffuse + Ambient光照计算
		fixed3 worldNormal = normalize(i.worldNormal);
		fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
		fixed3 lambert = saturate(dot(worldNormal, worldLightDir));
		fixed3 albedo = lambert * _Diffuse.xyz * _LightColor0.xyz + UNITY_LIGHTMODEL_AMBIENT.xyz;
		fixed3 color = tex2D(_MainTex, i.uv).rgb * albedo;
		//渐变边缘色
		float lerpValue = disolveFactor / dissolveValue.r;
		fixed3 edgeColor = lerp(_DissolveColorA, _DissolveColorB, lerpValue);	
		fixed3 finalColor = lerp(color, edgeColor, lerpValue * step(0.0001, disolveFactor));
		return fixed4(finalColor, 1);
	}
	ENDCG
	
	SubShader
	{
		Tags{ "RenderType" = "Transparent" }
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
