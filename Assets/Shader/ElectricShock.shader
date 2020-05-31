Shader "Custom/ElectricShock" {
	Properties {
		_Threshold ("Threshold", Float) = 0.5
		_EdgePower ("EdgePower", Float) = 2
		[Toggle]_IsLerpEdgeColor("IsLerpEdgeColor", Float) = 0
		_NoiseAmount ("NoiseAmount", Float) = 5
		_Color ("Outline Color", Color) = (0,0,0,1)
		_Width ("Outline Width", Float) = 0.5
		_NoiseMap("Noise Map", 2D) = "white" {}							// 噪声纹理
	}
	SubShader {
		Tags { "Queue" = "Transparent" }
		Pass {
			ZWrite OFF							// 关闭深度写入
			Cull Front							// 剔除正面
			Blend SrcAlpha OneMinusSrcAlpha		// 透明混合颜色

            CGPROGRAM

			bool _IsLerpEdgeColor;
			fixed4 _Color;
			float _EdgePower;
			float _NoiseAmount;
			float _Width;
			float _Threshold;
			sampler2D _NoiseMap;
			float4 _NoiseMap_ST;
 
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
             
            struct v2f 
            {
                float4 pos : SV_POSITION;
				float2 uvNoiseMap : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldViewDir : TEXCOORD2;
            };
             
            v2f vert (appdata_base v) 
            {
                v2f o;
				//v.vertex.xyz += v.normal.xyz * _Width;	// 顶点延法线方向外扩
                //o.pos = UnityObjectToClipPos(v.vertex);
				float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
				float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				normal.z = -0.5;	// 让法线向视角方向外扩，避免物体有背面遮挡正面
				pos = pos + float4(normalize(normal), 0) * _Width;		//对外扩展，出现轮廓
				o.pos = mul(UNITY_MATRIX_P, pos);
				o.uvNoiseMap = TRANSFORM_TEX(v.texcoord, _NoiseMap);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldViewDir = UnityWorldSpaceViewDir(mul(unity_ObjectToWorld, v.vertex));
                return o;
            }

			half4 frag(v2f i) :COLOR {
				fixed3 noise = tex2D(_NoiseMap, i.uvNoiseMap).rgb * _NoiseAmount;
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldViewDir = normalize(i.worldViewDir);
				fixed finalScale = (_Color.a);
				fixed edge = abs(dot(worldNormal, worldViewDir));
				edge = 1 - pow(1 - edge, _EdgePower);
				if (_IsLerpEdgeColor)
					return _Color * _Color.a * edge * noise.r; 
				else
				{
					clip(edge + noise.r - _Threshold);
					return _Color * _Color.a; 
				}
			}
			ENDCG
		}
	}
 
	FallBack OFF
}