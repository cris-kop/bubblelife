Shader "ScreenTransition"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Glow ("Glow", Color) = (1,1,1,1)
        _TransitionRange("_TransitionRange", Float) = 0.05

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
            sampler2D _FromTex;
            sampler2D _ToTex;

            float _Progress;
            float4 _Center;
            fixed4 _Glow;
            float _TransitionRange;

            float _AspectRatio;


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 fromCol = tex2D(_FromTex, i.uv);
                fixed4 toCol = tex2D(_ToTex, i.uv);

                float2 deltaFromPlayer = i.uv.xy - _Center.xy;
                deltaFromPlayer.y /= _AspectRatio;
                float distanceFromPlayer = length(deltaFromPlayer);

                float progress = pow(_Progress, 1.5);

                float range = _TransitionRange;
                float min = 0.0 + progress * 2;
                float max = min + range;
                float fade = smoothstep(min, max, distanceFromPlayer);
                fade = 1 - fade;
                
                float glowStrength = smoothstep(min, max, distanceFromPlayer);
                glowStrength = 1 - glowStrength * 2;
                glowStrength = abs(glowStrength);
                glowStrength = 1 - glowStrength;
                

                //float fade = length(i.uv.xy - _Center.xy) * _Progress * 10;



                //float progress = lerp(saturate(fade), 1.0, _Progress);
                fixed4 col = lerp(fromCol, toCol, fade);
                col = lerp(col, _Glow, glowStrength);
                return col;
            }
            ENDCG
        }
    }
}
