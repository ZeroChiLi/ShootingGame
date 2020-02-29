

//X光效果
 
Shader "Custom/OcclusionHighLight"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Amount("Amount", float) = 5
	}
 
	SubShader
	{
		Tags{ "Queue" = "Geometry+100" "RenderType" = "Opaque" }
		
		Pass
		{
			Blend SrcAlpha One
			ZWrite Off
			ZTest Greater
 
			CGPROGRAM
			#include "Lighting.cginc"
			fixed4 _Color;
			fixed _Amount;
			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 normal : normal;
				float3 viewDir : TEXCOORD0;
			};
 
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.viewDir = ObjSpaceViewDir(v.vertex);
				o.normal = v.normal;
				return o;
			}
 
			fixed4 frag(v2f i) : SV_Target
			{
				float3 normal = normalize(i.normal);
				float3 viewDir = normalize(i.viewDir);
				float rim = 1 - dot(normal, viewDir);
				return _Color * rim * _Amount;
			}
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
