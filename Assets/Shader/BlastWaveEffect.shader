
// 冲击波
Shader "Custom/BlastWaveEffect" {
	Properties{
		_NoiseMap("NoiseMap", 2D) = "white"{}
		_NoiseAmount("NoiseAmount", Range(0, 1.0)) = 0.5
		_BlastAmount("BlastAmount", Range(-1.0, 1.0)) = -0.01
		_RimSmooth("RimSmooth", Range(0, 1.0)) = 1
		_Alpha("Alpha",Range(0,1)) = 0.8
		_TestColor("TestColor", Color) = (0,0,0,1)
	}
	SubShader{
		
		Tags { "Queue" = "Transparent"/* "RenderType" = "Geometry"*/ }
		//Cull Off 
		ZWrite Off 
		//ZTest Off

		// 抓取屏幕图像的Pass，存入到纹理变量中。
		GrabPass { "_GrabTex" }

		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _GrabTex;			// 对应GrabPass指定的纹理名
			sampler2D _NoiseMap;
			float4 _NoiseMap_ST;
			fixed _NoiseAmount;
			fixed _BlastAmount;
			fixed _RimSmooth;
			fixed _Alpha;
			float4 _TestColor;

			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 texcoord: TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float4 scrPos : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float2 noiseUV : TEXCOORD2;
				float3 worldViewDir : TEXCOORD3;
			};

			v2f vert(a2v v) {
				v2f o;
				o.noiseUV = TRANSFORM_TEX(v.texcoord, _NoiseMap);
				o.pos = UnityObjectToClipPos(v.vertex);

				o.scrPos = ComputeGrabScreenPos(o.pos);			// 计算抓取的屏幕图像采样坐标，在UnityCG.cginc中定义。

				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				o.worldViewDir = _WorldSpaceCameraPos.xyz - UnityObjectToWorldDir(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				// 噪声偏移
				fixed4 noiseValue = tex2D(_NoiseMap, i.noiseUV);
				fixed2 noiseOffset = (noiseValue.r - 0.5) * _NoiseAmount;
				
				// 法线外扩
				fixed2 normalOffset = _BlastAmount * (i.worldNormal);

				// 边缘平滑
				fixed rim = max(0, dot(normalize(i.worldViewDir), i.worldNormal));
				rim = lerp(1, rim, _RimSmooth);

				// 计算屏幕坐标
				fixed2 uv = (i.scrPos.xy / i.scrPos.w) + (normalOffset + noiseOffset) * lerp(1, rim, _RimSmooth);
				fixed3 finalColor = tex2D(_GrabTex, uv);
				return fixed4(finalColor, _Alpha);
			}

			ENDCG
		}
	}

	FallBack "Diffuse"
}
