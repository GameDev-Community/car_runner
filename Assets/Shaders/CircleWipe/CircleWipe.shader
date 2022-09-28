Shader "Hidden/CircleWipe"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Range(0.0, 1.0)) = 1
        _Smooth ("Smooth", Range(0.0, 1.0)) = 1
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
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


            float _Radius;
            float _Smooth;
            float2 _Center;

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
                float xl = uv.x - _Center.x;
                xl *= xl;

                float yl = uv.y - _Center.y;
                yl *= yl;

                float sqrDist = xl + yl;
                float sqrRadius = _Radius * _Radius;

                if(sqrDist < _Radius)
                {
                    return 1 - smoothstep(sqrRadius, sqrRadius - _Smooth, sqrDist);
                }
                else
                {
                    //return tex2D(_MainTex, uv).w;;
                    return 1;
                }
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float radius = 0.2;
                float smoothing = 0.03;
                float outputAlpha = drawCircle(i.uv);

                fixed4 col = tex2D(_MainTex, i.uv);
                col.w = outputAlpha;
                return col;
            }
            ENDCG
        }
    }
}
