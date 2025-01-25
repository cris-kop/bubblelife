Shader "MM/General/Color"
{
    Properties
    {
        [HDR] _Color ("Color", COLOR) = (1,1,1,1)
        [KeywordEnum(Default, Radial, Horizontal, Vertical, Fresnel)] _ColorDisplay("Color Display Mode", Float) = 0
        [Toggle] _INVERT ("Invert", Float) = 0
        [Toggle] _AFFECTALPHA("AffectAlpha", Float) = 0
        _Remap("Remap", Vector) = (0, 1, 0, 0)
        [PowerSlider(3.0)] _Pow("Pow", Range(0.25, 4)) = 1

        [Header(Blending)]
        [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp("Advanced: Blend Operation", Float) = 0      //"Add"      
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Source Blend", Float) = 5                // "SrcAlpha"
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Destination Blend", Float) = 10          // "OneMinusSrcAlpha"

        [Header(DepthFading)]
        [Toggle] _DEPTHFADE("Depth Fade", Float) = 0
        [Toggle] _INVERTDEPTHFADE("Invert Depth Fade", Float) = 0
        _DepthFactor ("Depth Factor", Range(0, 10)) = 2
        [PowerSlider(3.0)] _DepthPow("DepthPow", Range(0.25, 4)) = 1
            
        [Header(Culling)]
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2                             // ""Back"

        [Header(Depth)]
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("Depth Test", Float) = 4               // "LessEqual"
        [Toggle(_WRITE_Z_DEPTH)] _ZWrite("Depth Write", Float) = 1

        [Header(Stencil)]
        [IntRange] _StencilRef ("Stencil Reference Value", Range(0,255)) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilCompare("Stencil Compare", Float) = 4                     // "LessEqual"
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilPassOperation("Stencil Pass Operation", Float) = 0              // "Keep"
        [Space]
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilPassFailOperation("Stencil Pass Fail Operation", Float) = 0     // "Keep"
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilPassZFailOperation("Stencil Pass ZFail Operation", Float) = 0   // "Keep"
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        LOD 100

        BlendOp [_BlendOp]
        Blend   [_SrcBlend][_DstBlend]
        ZTest   [_ZTest]
        ZWrite  [_ZWrite]
        Cull[_Cull]

        Pass
        {
            Stencil
            {
                Ref     [_StencilRef]
                Comp    [_StencilCompare]
                Pass    [_StencilPassOperation]
                Fail    [_StencilPassFailOperation]
                Zfail   [_StencilPassZFailOperation]
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _COLORDISPLAY_DEFAULT _COLORDISPLAY_RADIAL _COLORDISPLAY_HORIZONTAL _COLORDISPLAY_VERTICAL _COLORDISPLAY_FRESNEL
            #pragma multi_compile __ _INVERT_ON
            #pragma multi_compile __ _AFFECTALPHA_ON
            #pragma multi_compile __ _DEPTHFADE_ON
            #pragma multi_compile __ _INVERTDEPTHFADE_ON

            #include "UnityCG.cginc"
            #include "./CGINC/IncludeGeneral.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 color : COLOR;
                float3 normal : NORMAL;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 color : COLOR;
                float3 worldNormal : NORMAL;
                float4 screenPos : TEXCOORD1;
                float3 worldPos : TEXCOORD2;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            half4 _Color;
            float _Pow;
            float2 _Remap;

#ifdef _DEPTHFADE_ON
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            float _DepthFactor;
            float _DepthPow;
#endif

            v2f vert (appdata v)
            {
                v2f o;

                //https://docs.unity3d.com/2018.1/Documentation/Manual/SinglePassInstancing.html
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                o.worldPos = LocalToWorldPos(v.vertex);
                o.worldNormal = LocalToWorldNormal(v.normal);

                o.screenPos = ClipToScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.screenPos.z);


                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float displayAdjustment = 1;

                #ifdef _COLORDISPLAY_RADIAL
                    displayAdjustment = length(i.uv - 0.5) * 2;
                #elif _COLORDISPLAY_HORIZONTAL
                    displayAdjustment = i.uv.x;
                #elif _COLORDISPLAY_VERTICAL
                    displayAdjustment = i.uv.y;
                #elif _COLORDISPLAY_FRESNEL
                    float3 lookDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                    float fresnel = dot(i.worldNormal, lookDir);
                    fresnel = saturate(1 - fresnel);
                    displayAdjustment = fresnel;
                #endif

                #ifdef _INVERT_ON
                    displayAdjustment = 1 - displayAdjustment;
                #endif

                displayAdjustment = InverseLerp(_Remap.x, _Remap.y, displayAdjustment);
                displayAdjustment = pow(displayAdjustment, _Pow);


                half4 col = half4(i.color, 1) * _Color;


                 #ifdef _DEPTHFADE_ON
                     float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                     float depth = sceneZ - i.screenPos.z;
                     depth = saturate(depth / _DepthFactor);
                
                     #ifdef _INVERTDEPTHFADE_ON
                         depth = 1 - depth;
                     #endif
                
                     float depthFading = pow(depth, _DepthPow);
                     displayAdjustment *= depthFading;
                 #endif


                #ifdef _AFFECTALPHA_ON
                    col *= displayAdjustment;
                    return col;
                #endif

                col.rgb *= displayAdjustment;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
