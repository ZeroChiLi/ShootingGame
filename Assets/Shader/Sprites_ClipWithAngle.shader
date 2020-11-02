// 支持角度剔除的Sprite(扇形)，修改至"Sprites/Default"
Shader "Sprites/ClipWithAngle"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
		_Angle ("Angle", Range(0, 360)) = 360
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFragWithAngle
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

			float _Angle;
			
			fixed4 SpriteFragWithAngle(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;

				// 容错接近0和360度
				if (_Angle < 0.001)
				{
					discard;
				}
				else if (_Angle < 359.999)
				{
					// 剔除小于指定范围角度的
					clip(-atan2((IN.texcoord.y - 0.5) , (IN.texcoord.x - 0.5)) + ((_Angle - 180) / 57.32484));
				}
				return c;
			}
        ENDCG
        }
		

    }
}
