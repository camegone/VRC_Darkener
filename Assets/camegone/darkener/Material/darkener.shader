Shader "camegone/darkener"
{
    Properties
    {
        [MainColor] _UdonDarkenerColor("Overlay Color", Color) = (1.0, 1.0, 1.0, 1.0)
        [Toggle] _IsShownInNonUserCamera("Is Shown In Non-User Camera", Float) = 0.0
    }
        SubShader
    {
        Tags {"Queue" = "Transparent+10000"}
        // multiplicative blend (Src * Zero) + (Dst *SrcColor)
        BlendOp Add
        Blend Zero SrcColor
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // add multi compile
            #pragma multi_compile_local _ _ISSHOWNINNONUSERCAMERA_ON

            struct appdata
            {
                float4 vertex : POSITION;
                // float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                // float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // vrchat shader globals
            float _VRChatCameraMode;
            float _VRChatMirrorMode;
            float _VRChatFaceMirrorMode;
            // end vrchat shader globals

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.uv = v.uv;

                #ifndef _ISSHOWNINNONUSERCAMERA_ON
                // set 0.0 to vertex position to make to be transparent
                if (_VRChatCameraMode != 0)
                    o.vertex = float4(0.0, 0.0, 0.0, 0.0);
                #endif
                // always exclude in the mirrors
                if( _VRChatMirrorMode != 0 || _VRChatFaceMirrorMode != 0)
                    o.vertex = float4(0.0, 0.0, 0.0, 0.0);
                
                return o;
            }

            fixed4 _UdonDarkenerColor;

            fixed4 frag(v2f i) : SV_Target
            {
                return _UdonDarkenerColor;
            }
            ENDCG
        }
    }
}
