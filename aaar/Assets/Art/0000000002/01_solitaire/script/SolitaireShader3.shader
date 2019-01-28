Shader "Custom/Solitaire3" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Offset ("_Offset", float) = 0.5
        //_MainTex2 ("Albedo (RGB)", 2D) = "white" {}

    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf ToonRamp addshadow noambient
        //#pragma target 3.0
        #pragma multi_compile_instancing

        sampler2D _MainTex;
        sampler2D _MainTex2;
        float _Offset;

        struct Input {
            float2 uv_MainTex;
        };

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color) // Make _Color an instanced property (i.e. an array)
#define _Color_arr Props
                UNITY_DEFINE_INSTANCED_PROP(fixed4, _Uv) // Make _Color an instanced property (i.e. an array)
#define _Uv_arr Props
            UNITY_INSTANCING_BUFFER_END(Props)

        fixed4 LightingToonRamp (SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            half d = dot(s.Normal, lightDir)*0.5 + 0.5;//0-1
            //fixed3 ramp = tex2D(_RampTex, fixed2(d, 0.5)).rgb;
            fixed4 c;

            c.rgb = fixed4(1.0,0,0,0);
            
			c.rgb = s.Albedo * _LightColor0.rgb * ( d * _Offset + (1.0 - _Offset) );
            c.a = 0;

            return c;
        }

        void surf (Input IN, inout SurfaceOutput o) {
            
            fixed4 col = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
            fixed4 uvv = UNITY_ACCESS_INSTANCED_PROP(_Uv_arr, _Uv);

            fixed2 uv1 = uvv.xy + IN.uv_MainTex * fixed2(2.0,1.0)*uvv.zw;
            fixed2 uv2 = uvv.xy + (IN.uv_MainTex-fixed2(0.5,0))  * fixed2(2.0,1.0)*uvv.zw;


			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * col;
            fixed4 tex = tex2D(_MainTex, uv1 );
            if(IN.uv_MainTex.x>=0.5){
                tex = tex2D(_MainTex, uv2 );
            }

            if(IN.uv_MainTex.y==1){
                tex = fixed4(0,0,0,1);
            }

            o.Albedo = tex.rgb;
            o.Alpha = tex.a;

        }
        ENDCG
    }
    FallBack "Diffuse"
}