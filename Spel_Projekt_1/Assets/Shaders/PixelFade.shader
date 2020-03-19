Shader "Regression/PixelFade"
{
	Properties
	{
		_MainTex("From Texture", 2D) = "white" {}
		_ToTex("To Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_speed("Speed", Float) = 1
		[HideInInspector] _startT("Start Time", Float) = 0
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};


			v2f vert(appdata IN)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				#ifdef PIXELSNAP_ON
					//OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _ToTex;
			sampler2D _NoiseTex;
			float _startT;
			float _speed;

			fixed4 frag(v2f i) : SV_Target
			{
				float test = tex2D(_NoiseTex, i.uv) + _speed * (_Time.y - _startT);

				return test > 1 ? tex2D(_ToTex, i.uv) : tex2D(_MainTex, i.uv);
			}
		ENDCG
		}
		}
}