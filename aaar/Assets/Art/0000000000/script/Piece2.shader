// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Piece2" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Amount ("_Amount", Range(0,5)) = 0.0

        [Toggle] _Positive("Cull Positive", Float) = 1
        _PlaneCount("Plane count", Range(0, 10)) = 1

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		#pragma surface surf Standard addshadow vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float4 viewVertex : TEXCOORD2;
		};

		half _Glossiness;
		half _Metallic;
		//fixed4 _Color;
		float _Amount;

        float _Positive;
        uniform float _PlaneCount;
        uniform float4 _ClippingPlanes[10];		

        UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(fixed4, _Random) // Make _Color an instanced property (i.e. an array)
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color) // Make _Color an instanced property (i.e. an array)
#define _Color_arr Props
        UNITY_INSTANCING_BUFFER_END(Props)

		#include "./noise/SimplexNoise3D.hlsl"


        void vert(inout appdata_full v, out Input o )
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
			

            //インスタンス ID がシェーダー関数にアクセス可能になります。頂点シェーダーの最初に使用する必要があります
            UNITY_SETUP_INSTANCE_ID (v);

            //頂点シェーダーで入力構造体から出力構造体へインスタンス ID をコピーします。
            //フラグメントシェーダーでは、インスタンスごとのデータにアクセスするときのみ必要です
            //UNITY_TRANSFER_INSTANCE_ID (v, o);            

			float4 p = UnityObjectToClipPos(v.vertex);
			float4 rand = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Random);

			v.vertex.y = snoise( p.xyz*3.0 + rand * 100.0 + _Time.y ) * 0.1;

			//ゆがます。


			o.viewVertex = mul(UNITY_MATRIX_MV, v.vertex);
            
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {

            //UNITY_SETUP_INSTANCE_ID (IN);

                //discard
                int count = int(_PlaneCount);
                for (int idx = 0; idx < count; idx++)
                {
                    float4 plane = _ClippingPlanes[idx];
                    if (_Positive == 0)
                    {
                        if (dot(plane.xyz, IN.viewVertex.xyz) > plane.w)
                        {
                            discard;
                        }
                    }
                    else 
                    {
                        if (dot(plane.xyz, IN.viewVertex.xyz) < plane.w)
                        {
                            discard;
                        }
                    }
                }


            fixed4 col = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * col;
			o.Albedo = fixed3(0,0,0);//c.rgb;
			o.Emission = c.rgb;
			
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}