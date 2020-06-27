Shader "Unlit/SimpleSSS"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Tint("Tint",Color)=(1,1,1,1)
		_FrontSurfaceDistortion("FrontSurfaceDistortion",float) = 1
		_BackSurfaceDistortion("BackSurfaceDistortion",float) = 1
		_InteriorColor("InteriorColor",Color) = (1,1,1,1)
		_InteriorColorPower("InteriorColorPower",float) = 1
		_FrontSSSIntensity("FrontSSSIntensity",float) = 1
		_Gloss("Gloss",float)=1
		_RimPower("RimPower",float)=1
		_RimIntensity("RimIntensity",float)=1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal:NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos:TEXCOORD1;
				float3 worldNormal:TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _InteriorColor;
			float _InteriorColorPower;
			float _FrontSurfaceDistortion;
			float _BackSurfaceDistortion;
			float _FrontSSSIntensity;
			float _Gloss;
			float4 _Tint;
			float _RimPower;
			float _RimIntensity;
			
			float SubSurfaceScattering(float3 viewDir,float3 lightDir,float3 normalDir,float frontSubSurfaceDistortion,float backSubSurfaceDistortion,float frontSSSIntensity) {
				//计算正面和背面此表面散射
				float3 frontLitDir = normalDir * frontSubSurfaceDistortion - lightDir;
				float3 backLitDir = normalDir * backSubSurfaceDistortion + lightDir;
				float frontSSS = saturate(dot(viewDir, -frontLitDir));
				float backSSS = saturate(dot(viewDir, -backLitDir));
				float result = saturate(frontSSS * frontSSSIntensity + backSSS);
				return result;
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex,i.uv)*_Tint;
				float3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos).xyz);
				float3 lightDir = normalize(-_WorldSpaceLightPos0.xyz);
				float3 normal = normalize(i.worldNormal.xyz);
				//SSS
				float SSS = SubSurfaceScattering(viewDir, lightDir, normal, _FrontSurfaceDistortion,_BackSurfaceDistortion,_FrontSSSIntensity);
				float3 SSSCol = lerp(_InteriorColor, _LightColor0, saturate(pow(SSS, _InteriorColorPower))).rgb*SSS;
				//Diffuse
				float4 unLitCol = col * _InteriorColor*0.5;
				float diffuse = dot(normal, lightDir);
				float4 diffuseCol = lerp(unLitCol,col,diffuse);
				//Specular
				float specularPow = exp2((1 - _Gloss) * 10 + 1);
				float3 halfDir = normalize(lightDir + viewDir);
				float3 specular = pow(max(0, dot(halfDir, normal)), specularPow);
				specular *= _LightColor0.rgb;
				//Rim
				float rim = 1.0 - max(0, dot(normal, viewDir));
				float rimValue = lerp(rim, 0, SSS);
				float3 rimCol = lerp(_InteriorColor, _LightColor0.rgb, rimValue)*pow(rimValue, _RimPower)*_RimIntensity;

				float3 final = SSSCol + diffuseCol.rgb+specular+rimCol;
				return float4(final,1);
			}
			ENDCG
		}
	}
}
