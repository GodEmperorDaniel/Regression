// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Regression/WaterRipples"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_NormalTex("Normalmap", 2D) = "bump" {}
		//_NormalTex2("Normalmap2", 2D) = "bump" {}
		_Magnitude("Magnitude", Range(0,1)) = 0.05
		//_SpecularDir("Specular Direction", Vector) = (0, 0, 0, 0)
		//_SpecularCol("Specular Color", Color) = (1,1,1,1)
		//_SpecularCut("Specular Cutoff", Range(0,1)) = 0.25
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_instancing
				#pragma multi_compile_local _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				//#include "UnitySprites.cginc"
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float4 color : COLOR;
					float2 uv : TEXCOORD0;
				};
				
				struct v2f
				{
					float2 uv : TEXCOORD0;
					float2 worldPos : TEXCOORD2;
					float4 color : COLOR;
					float4 vertex : SV_POSITION;
				};

				fixed4 _Color;
				float4 _NormalTex_ST;

				v2f vert(appdata IN)
				{
					v2f OUT;

					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.uv = IN.uv;
					OUT.color = IN.color * _Color;

					OUT.worldPos = TRANSFORM_TEX( mul(unity_ObjectToWorld, IN.vertex), _NormalTex);

					#ifdef PIXELSNAP_ON
						OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif


					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _NormalTex;
				//sampler2D _NormalTex2;
				float _Magnitude;
				//fixed4 _SpecularDir;
				//float4 _SpecularCol;
				//float _SpecularCut;

				fixed4 frag(v2f i) : SV_Target
				{
					float uv = i.uv;
					half4 bump1 = tex2D(_NormalTex, i.worldPos);
					//half4 bump2 = tex2D(_NormalTex2, i.worldPos);
					half2 distortion = UnpackNormal(bump1).rg;// +UnpackNormal(bump2).rg;
					//float spec = dot(distortion, _SpecularDir.xy);
					//spec = spec > _SpecularCut ? spec : 0;
					//fixed4 specCol = spec * _SpecularCol * _SpecularCol.a;
					//specCol.a = 0;
					fixed4 c = tex2D(_MainTex, i.uv + distortion * _Magnitude) * i.color;// +specCol;
					c.rgb *= c.a;
					return c;
				}
			ENDCG
			}
		}
}