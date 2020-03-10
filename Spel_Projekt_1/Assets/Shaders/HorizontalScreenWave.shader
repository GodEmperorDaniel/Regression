Shader "Regression/HorizontalScreenWave"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		[HideInInspector] _startT("Start Time", Float) = 0
		_fT("Time Frequency", Float) = 1
		_fD("Space Frequency", Float) = 1
		_amp("Amplitude", Float) = 0.0625
		_scale("Scale", Float) = 0.875
		_fadeT("Fade In Time", Float) = 1
		_fadeD("Fade In Distance", Float) = 1
		_fadeO("Fade Out Time", Float) = 10
	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

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

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				sampler2D _MainTex;
				float _startT;
				float _fT;
				float _fD;
				float _amp;
				float _scale;
				float _fadeT;
				float _fadeD;
				float _fadeO;

				fixed4 frag(v2f i) : SV_Target
				{
					float a1 = (_Time.y - _startT) / _fadeT + (i.uv.y - 1) * _fadeD;
					float a2 = (_fadeO + _startT - _Time.y) / _fadeT + (1 - i.uv.y) * _fadeD;

					float t = max(0, min(1, min(a1, a2)));
					
					float dx = _amp * t * sin((_fT * _Time.y + _fD * i.uv.y));
					float s = 1 - (1 - _scale) * t;
					float o = 0.5 * (1 - s);
					fixed4 col = tex2D(_MainTex, float2(o + s * (i.uv.x + dx), i.uv.y));
					return col;
				}
				ENDCG
			}
		}
}
