Shader "MM/General/DepthOnly"
{
    Properties
    {
        [Toggle(_WRITE_Z_DEPTH)] _ZWrite("Depth Write", Float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("Depth Test", Float) = 4
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2
        [Toggle(_WRITE_ALPHA)] _Alpha("Alpha", Float) = 1
    }
        SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Blend One One
        ZTest[_ZTest]
        ZWrite[_ZWrite]
        Cull[_Cull]

        LOD 100

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

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            float _Alpha;

            v2f vert(appdata v)
            {
                v2f o;

                // https://docs.unity3d.com/2018.1/Documentation/Manual/SinglePassInstancing.html
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(0, 0, 0, _Alpha);
            }
            ENDCG
        }
    }
}
