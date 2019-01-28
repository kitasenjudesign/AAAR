// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Panfu/panfu" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2 ("Albedo (RGB)", 2D) = "white" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Amount ("_Amount", Range(0,3.14)) = 0.0
		_Offset ("_Offset", Vector) = (0,0,0,0)
		_Rot("_Rot",Vector) = (0,0,0,0)
		_Ratio("_Ratio",Range(0,1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		cull off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		#pragma surface surf Standard addshadow vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;


		struct Input {
			float2 uv_MainTex;
			float IsFacing:VFACE;
		};

		half _Glossiness;
		half _Metallic;
		float _Amount;
		//float _Ratio;
		float4 _Offset;
		float4 _Rot;

        UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color) // Make _Color an instanced property (i.e. an array)
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Rand) // Make _Color an instanced property (i.e. an array)
            UNITY_DEFINE_INSTANCED_PROP(float, _Ratio)
			//UNITY_DEFINE_INSTANCED_PROP(fixed4, _Offset) // Make _Color an instanced property (i.e. an array)
            //UNITY_DEFINE_INSTANCED_PROP(fixed4, _Rot) // Make _Color an instanced property (i.e. an array)

			#define _Color_arr Props
        UNITY_INSTANCING_BUFFER_END(Props)

		// オイラー角（ラジアン）を回転行列に変換
		float4x4 eulerAnglesToRotationMatrix(float3 angles)
		{
			float ch = cos(angles.y); float sh = sin(angles.y); // heading
			float ca = cos(angles.z); float sa = sin(angles.z); // attitude
			float cb = cos(angles.x); float sb = sin(angles.x); // bank

			// Ry-Rx-Rz (Yaw Pitch Roll)
			return float4x4(
				ch * ca + sh * sb * sa, -ch * sa + sh * sb * ca, sh * cb, 0,
				cb * sa, cb * ca, -sb, 0,
				-sh * ca + ch * sb * sa, sh * sa + ch * sb * ca, ch * cb, 0,
				0, 0, 0, 1
			);
		}

        void vert(inout appdata_full v, out Input o )
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
			

            //インスタンス ID がシェーダー関数にアクセス可能になります。頂点シェーダーの最初に使用する必要があります
            UNITY_SETUP_INSTANCE_ID (v);

            //頂点シェーダーで入力構造体から出力構造体へインスタンス ID をコピーします。
            //フラグメントシェーダーでは、インスタンスごとのデータにアクセスするときのみ必要です
            //UNITY_TRANSFER_INSTANCE_ID (v, o);            

			float ratio = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Ratio);
			//ratio = 1 - ratio;
			fixed4 offset = fixed4(
				0,
				0,
				-_Offset.z * (1-ratio),
				0
			);//_Offset;
			fixed4 rot = fixed4(
				3.1415/2 * (1-ratio),
				0,
				0,
				0
			);//_Rot;



			//ratio

			// オブジェクト座標からワールド座標に変換する行列を定義
			float4x4 object2world = (float4x4)0; 
			// スケール値を代入
			object2world._11_22_33_44 = float4(1.0,1.0,1.0,1.0);
			
			// 速度からY軸についての回転を算出
			//float rotY = cubeData.velocity.x * 0.8 * 3.14 * _Amount;
			//float rotX = cubeData.velocity.y * 0.8 * 3.14 * _Amount;
			//float rotZ = cubeData.velocity.z * 0.5 * 3.14 * _Amount;
			// オイラー角（ラジアン）から回転行列を求める
			float4x4 rotMatrix = eulerAnglesToRotationMatrix(rot.xyz);
			// 行列に回転を適用
			object2world = mul(rotMatrix, object2world);
			
			object2world._14_24_34 += offset.xyz;// + float3(0,0,zz);
            
			


			//頂点をてきとうに、ごちゃごちゃする、ここではノーマル方向に値を足してる

			float amp = length( v.vertex.yz );
			fixed4 rand = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Rand);

			//float rad = atan2( v.vertex.y, v.vertex.z ) +  (0.5+0.5*sin(_Time.z*20 + r.x * 2 * 3.1415));
			float rad = atan2( v.vertex.y, v.vertex.z ) + _Amount;

			//ばたばた
			float speed = _Time.z*(8 + rand.x*6);
			float theta = speed + rand.x*2*3.14 + v.vertex.x * (2+10*rand.z);
			float hiraki = 0.35 + 0.13 * rand.w;//max=0.5
			rad += 3.14*hiraki* ( 0.5+0.5*sin(theta ) ) * ratio;
			rad *= sign( v.vertex.z );

			
			float yuragiY = sin(theta*0.2) * 0.03 * ratio;
			v.vertex.y = amp * sin(rad) + yuragiY;// + // + sin(v.vertex.x*10) * 0.01;
			v.vertex.z = amp * cos(rad);

			v.vertex = mul(object2world, v.vertex);

			//v.vertex += _Offset;
			//v.vertex.xyz += v.normal.xyz * _Amount;
			
			//v.vertex.z wo input
			//v.vertex.xy

		}

		void surf (Input IN, inout SurfaceOutputStandard o) {

            //UNITY_SETUP_INSTANCE_ID (IN);

            fixed4 col = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);

			// Albedo comes from a texture tinted by color
			fixed4 c1 = tex2D (_MainTex, IN.uv_MainTex);//* col;
			fixed4 c2 = tex2D (_MainTex2, IN.uv_MainTex);//* col;

			float4 color = (IN.IsFacing>0) ? c1 : c2;

			o.Albedo = fixed3(0,0,0);
			o.Emission = color.rgb;
		
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = color.a;
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}