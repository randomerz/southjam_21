// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/PaperShadeEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Intensity ("Intensity", range(0.0, 1.0)) = 1
		_TintColor ("Tint Color", Color) = (1, 1, 1, 0.25)
		[Toggle] _BDoLerp ("Do Lerp", float) = 1
		_SpotlightRadius ("Spotlight Radius", float) = 75
		_PlayerPosition ("Player Position", Vector) = (0, 0, 0)
		_TargetPosition ("Target Position", Vector) = (0, 0, 0)
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

			float _Intensity;
			fixed4 _TintColor;
			float _BDoLerp;
			float _SpotlightRadius;
			uniform float3 _PlayerPosition;
			uniform float4 _TargetPosition;


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

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

				// this is probably not very effecient
				float4 playerPos = UnityObjectToClipPos(_PlayerPosition);
				float4 screenPos = ComputeScreenPos(playerPos);
				//float3 worldPos = mul(unity_ObjectToWorld, float4(i.vertex.xyz, 1.0)).xyz;
				if (distance(i.vertex, screenPos) < _SpotlightRadius) {
					return col;
				}

				// lerp
				if (_BDoLerp == 1) {
					col = (1 - _Intensity) * col + _Intensity * _TintColor;
				}
				// wacky
				else {
					col = col + _Intensity * _TintColor;
				}

                return col;
            }
            ENDCG
        }
    }
}
