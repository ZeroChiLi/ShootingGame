
// 冲击波
Shader "Custom/BlastWave2DEffect" {
	Properties{
		_TintColor("Tint Color", Color) = (1,1,1,1)		// 给粒子系统做隐藏
		_BlastWaveMap("BlastWave Map (R)", 2D) = "black" {}
		_Amount("Amount",Float) = 1
	}
	SubShader{
		
		Tags{ "Queue" = "Transparent+10"  "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Lighting Off
		Cull Off
		ZTest On
		ZWrite Off

		// 抓取屏幕图像的Pass，存入到纹理变量中。
		GrabPass { "_GrabTex" }

		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _GrabTex;			// 对应GrabPass指定的纹理名
			sampler2D _BlastWaveMap;
			float4 _BlastWaveMap_ST;
			fixed _Amount;
			half4 _TintColor;

			struct a2v {
				float4 vertex : POSITION;
				float2 texcoord: TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float4 scrPos : TEXCOORD0;
				float2 uv : TEXCOORD1;
			};

			v2f vert(a2v v) {
				v2f o;
				o.uv = TRANSFORM_TEX(v.texcoord, _BlastWaveMap);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.scrPos = ComputeGrabScreenPos(o.pos);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				// 扭曲贴图
				fixed4 waveValue = tex2D(_BlastWaveMap, i.uv);

				// 根据贴图RG值作偏移
				i.scrPos.xy = i.scrPos.xy + waveValue.rr * _TintColor.a * _Amount;

				// 计算屏幕坐标
				fixed2 uv = (i.scrPos.xy / i.scrPos.w);
				fixed3 finalColor = tex2D(_GrabTex, uv);
				return fixed4(finalColor, 1);
			}

			ENDCG
		}
	}

	FallBack "Diffuse"
}
