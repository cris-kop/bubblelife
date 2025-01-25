Shader "Unlit/Bubble"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _DisplacementTex ("_DisplacementTex", 2D) = "white" {}
        _DisplacementMagnitude("DisplacementMagnitude", Range(0, 1)) = 0.1

        _DisplacementScale("_DisplacementScale", Float) = 1.0
        _DisplacementSpeed("_DisplacementSpeed", Float) = 0.05
        _FluxSpeed("_FluxSpeed", Float) = 5.0
        _FluxStrength("_FluxStrength", Float) = 1.0

        _MinRange("_MinRange", Range(0, 1)) = 0.3
        _MaxRange("_MaxRange", Range(0, 1)) = 0.5

    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha

		Tags
		{ 
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}

		// No culling or depth
		Cull Off
		ZWrite Off
		ZTest Always

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _DisplacementTex;
            float _DisplacementMagnitude;
            float _DisplacementScale;
            float _DisplacementSpeed;

            float _FluxSpeed;
            float _FluxStrength;


            float _MinRange;
            float _MaxRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float2 toPolar(float2 cartesian){
                float distance = length(cartesian);
                float angle = atan2(cartesian.y, cartesian.x);
                return float2(angle / UNITY_TWO_PI, distance);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float scale = 2.0;
                i.uv *= scale;
                i.uv -= 0.5;

                float2 displaceUV = toPolar((i.uv - (0.5 * scale)) * _DisplacementScale);
                displaceUV.y -= _Time.y * _DisplacementSpeed;
                float4 displacementCol = tex2D(_DisplacementTex, displaceUV);
                float2 displacement = (displacementCol.xy * 2) - 1;
                displacement += 0.4;

                // sample the texture

                float dist = length(i.uv.xy - 0.5);

                float strength = smoothstep(_MinRange, _MaxRange, dist);
                float strengthFlux = 1 + sin(_Time.y * _FluxSpeed) * _FluxStrength;

                float2 finalUV = i.uv + displacement * _DisplacementMagnitude * strength * strengthFlux;

                finalUV.x = saturate(finalUV.x);
                finalUV.y = saturate(finalUV.y);

                fixed4 col = tex2D(_MainTex, finalUV);

                return col;
            }
            ENDCG
        }
    }
}
