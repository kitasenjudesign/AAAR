Shader "Custom/SolitaireWindow" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Offset ("_Offset", float) = 0.5
        //_RampTex ("Ramp", 2D) = "white"{}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf ToonRamp noambient
        #pragma target 3.0

        sampler2D _MainTex;
        float _Offset;
        //sampler2D _RampTex;

        struct Input {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        fixed4 LightingToonRamp (SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            half d = dot(s.Normal, lightDir)*0.5 + 0.5;//0-1
            //fixed3 ramp = tex2D(_RampTex, fixed2(d, 0.5)).rgb;
            fixed4 c;

            c.rgb = fixed4(1.0,0,0,0);
            
			c.rgb = s.Albedo * _LightColor0.rgb * (d*_Offset + (1.0 - _Offset));
            c.a = 1.0;

            return c;
        }

        void surf (Input IN, inout SurfaceOutput o) {
            
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);// * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;

        }
        ENDCG
    }
    FallBack "Diffuse"
}