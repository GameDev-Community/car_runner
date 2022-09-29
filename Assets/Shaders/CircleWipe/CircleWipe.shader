Shader "Hidden/CircleWipe"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", color) = (1,1,1,1)
        //_Radius ("Radius", Range(0.0, 1.0)) = 1
        //_Smooth ("Smooth", Range(0.0, 1.0)) = 1
        //_Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Settings ("Settings (radius, smooth, centerX, centerY)", Vector) = (0.5, 0.5, 0.5, 0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"  "Queue" = "Transparent"}
        
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"


            //float _Radius;
            //float _Smooth;
            //float2 _Center;
            float4 _Settings;
            half4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float drawCircle(in float2 uv)
            {
                //float xl = uv.x - _Center.x;
                float xl = uv.x - _Settings.z;
                xl *= xl;

                //float yl = uv.y - _Center.y;
                float yl = uv.y - _Settings.w;
                yl *= yl;

                float sqrDist = xl + yl;
                //float sqrRadius = _Radius * _Radius;
                float sqrRadius = _Settings.x * _Settings.x;

                if(sqrDist < _Settings.x)
                {
                    //return 1 - smoothstep(sqrRadius, sqrRadius - _Smooth, sqrDist);
                    return 1 - smoothstep(sqrRadius, sqrRadius - _Settings.y, sqrDist);
                }
                else
                {
                    //return tex2D(_MainTex, uv).w;;
                    return 1;
                }
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float outputAlpha = drawCircle(i.uv);
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.w = outputAlpha;
                return col;
            }
            ENDCG
        }
    }
}
