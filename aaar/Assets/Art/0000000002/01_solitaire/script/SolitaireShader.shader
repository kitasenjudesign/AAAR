// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Solitaire"
{
    Properties
    {
        _Color ("Color", Vector) = (1.0, 0.0, 0.0, 1.0)
        _MainTex ("_MainTex", 2D) = "white" {}
        _MainTex2 ("_MainTex", 2D) = "black" {}

        _Th ("_Th", Range(0,1)) = 0.5

    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
        cull off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #pragma multi_compile_instancing

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            sampler2D _MainTex2;
            
            float4 _MainTex_ST;
            float _Th;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color) // Make _Color an instanced property (i.e. an array)
#define _Color_arr Props
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _Uv) // Make _Color an instanced property (i.e. an array)
#define _Uv_arr Props
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;

                //インスタンス ID がシェーダー関数にアクセス可能になります。頂点シェーダーの最初に使用する必要があります
                UNITY_SETUP_INSTANCE_ID (v);

                //頂点シェーダーで入力構造体から出力構造体へインスタンス ID をコピーします。
                //フラグメントシェーダーでは、インスタンスごとのデータにアクセスするときのみ必要です
                UNITY_TRANSFER_INSTANCE_ID (v, o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                UNITY_SETUP_INSTANCE_ID (i);
                // 変数にアクセス

                fixed4 col = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);

                fixed4 uvv = UNITY_ACCESS_INSTANCED_PROP(_Uv_arr, _Uv);

                fixed2 uv2 = uvv.xy + i.uv*fixed2(2.0,1.0)*uvv.zw;

                fixed4 tex = tex2D(_MainTex2, fixed2(2.0,1.0) * (i.uv-fixed2(0.5,0)) );//, tex2D(_MainTex2,i.uv), step(0.5, i.uv.x) );
                if(i.uv.x<=0.5){
                    tex = tex2D(_MainTex, uv2 );
                }

                return tex;
            }
            ENDCG
        }
    }
}