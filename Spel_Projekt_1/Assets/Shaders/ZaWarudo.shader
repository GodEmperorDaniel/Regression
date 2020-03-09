Shader "Regression/ZaWarudo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_screenRatio("Screen Ratio", Float) = 0
		[HideInInspector] _startT("Start Time", Float) = 0
		_bounceT("Bounce Time", Float) = 2
		_speed("Speed", Float) = 5
		_sharpness("Sharpness", Float) = 4
		_saturation("Saturation", Float) = 0.5
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float _screenRatio;
			float _startT;
			float _bounceT;
			float _speed;
			float _sharpness;
			float _saturation;

			fixed4 frag(v2f i) : SV_Target
			{
				float dx = (i.uv.x - 0.5) * _screenRatio;
				float dy = (i.uv.y - 0.5);
				float dist = sqrt(dx * dx + dy * dy);

				float dt = _speed * max(0, min(_Time.y - _startT, _startT + _bounceT - _Time.y));
				float bounceDt = _speed * max(0, _startT + _bounceT - _Time.y);
				float c = dt * dt - dist;
				float bounceC = bounceDt * bounceDt - dist;
				float t = max(0, min(1, (c - 0.5) * _sharpness + 0.5));
				float bounceT = max(0, min(1, (bounceC - 0.5) * _sharpness + 0.5));

				float s = bounceT + (1 - bounceT) * _saturation;
				
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed bw = (1 - s) * ((col.r / 3) + (col.g / 3) + (col.b / 3));
				col.rgb = fixed3(s * col.r + bw, s * col.g + bw, s * col.b + bw);

                col.rgb = t + (1 - (2 * t)) * col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
