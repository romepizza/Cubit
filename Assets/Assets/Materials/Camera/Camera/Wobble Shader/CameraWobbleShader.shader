Shader "Custom/CameraWobble"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_waveLengthX("Wave Length X", float) = 300
		_waveTimeX("Wave Time X", float) = 1
		_waveAmplitudeX("Wave Amplitude X", float) = 1
		_waveLengthY("Wave Length Y", float) = 300
		_waveTimeY("Wave Time Y", float) = 1
		_waveAmplitudeY("Wave Amplitude Y", float) = 1

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
			float _waveLengthX;
			float _waveLengthY;
			float _waveTimeX;
			float _waveTimeY;
			float _waveAmplitudeX;
			float _waveAmplitudeY;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv + float2(cos(i.vertex.y / _waveLengthX + _Time[1] / _waveTimeX) / _waveAmplitudeX, sin(i.vertex.x / _waveLengthY + _Time[1] / _waveTimeY) / _waveAmplitudeY));
				// just invert the colors
				
				return col;
			}
			ENDCG
		}
	}
}
