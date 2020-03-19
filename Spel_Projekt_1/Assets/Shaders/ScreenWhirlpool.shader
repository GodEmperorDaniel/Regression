Shader "Regression/ScreenWhirlpool"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		[HideInInspector] _screenRatio("Screen Ratio", Float) = 0
		[HideInInspector] _startT("Start Time", Float) = 0
		_speed("Speed", Float) = 5
		_radiDrag("Radial Drag", Float) = 1
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
				float _screenRatio;
				float _startT;
				float _speed;
				float _radiDrag;

				fixed4 frag(v2f i) : SV_Target
				{
					float angle = atan2(i.uv.y - 0.5, (i.uv.x - 0.5) * _screenRatio);
					float dist = distance(float2(0.5 * _screenRatio, 0.5), float2(i.uv.x * _screenRatio, i.uv.y));
					float angleD = _speed * max(0, (_Time.y - _startT) * (1 - dist * _radiDrag));
					float newAngle = angle + angleD;

					//return fixed4(i.uv.x, i.uv.y, 0, 0);
					//return fixed4(0.5 + dist * cos(newAngle) / _screenRatio, 0.5 + dist * sin(newAngle),0,1);
					//return fixed4(angle, newAngle, 0, 1);

					return tex2D(_MainTex, float2(0.5 + dist * cos(newAngle) / _screenRatio, 0.5 + dist * sin(newAngle)));
				}
				ENDCG
			}
		}
}
