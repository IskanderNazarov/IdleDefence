Shader "Custom/Radar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed("Speed", Range(0, 0.6)) = 1
        _Width("Arc width", Range(0, 1)) = 0.3
        _BgCol("Bg color", Color) = (1, 1, 1, 1)
        _CircW("Circle outside width", Range(0, 0.2)) = 0.1
        _CircCol("Circ color", Color) = (1, 1, 1, 1)
        
        
        //_A("Line", Range(0, 2.3)) = 0
    }

    SubShader
    {
        // No culling or depth


        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        Pass
        {
            ZTest off
            Blend SrcAlpha one
            //Blend SrcAlpha OneMinusSrcAlpha
            //Blend zero SrcColor

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            sampler2D _MainTex;
            int _C;
            float _A;
            float _Width;
            float _Speed;
            float _R;
            float _CircW;
            float4 _CircCol;
            float4 _BgCol;
            #define PI UNITY_PI

            float inverseLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv * 2 - 1;
                float4 col = i.color;

                _R = 1;
                float d = length(uv);
                float circ = step(d, _R);

                float arc = _Width;
                float angle = atan2(uv.y, uv.x);
                angle = (angle + PI) / (2 * PI);

                _A = _Time.y * _Speed;
                _A = frac(_A);
                float t;
                if(_A < arc && angle > 1- (arc - _A))
                {
                    float a = 1 - (arc - _A);
                    float b = 1;
                    float c = 0;
                    float d =  inverseLerp(frac(_A) - arc, frac(_A), 0);
                    t = (angle - a) * (d - c) / (b - a) + c;//[a,b] => [c, d]
                } else
                {
                    t = inverseLerp(frac(_A) - arc, frac(_A), angle);
                    t *= step(0, t) * step(t, 1);
                }  
                


                //col.a = circ * angle;
                //col.a *= circ * (t + (1 - t) * _BgCol.a);
                col.rgb = t * col.rgb + (1 - t) * _BgCol.rgb;
                col.a *= circ * (t + (1 - t) * _BgCol.a);

                //draw circle
                float dist = length(uv);
                float c = smoothstep(1 - _CircW, 1, dist);
                //float c = lerp(1 - _CircW, 1, dist);
                float4 circColor;
                circColor.rgb = c * _CircCol.rgb;// + (1 - c) * col.rgb;
                circColor.a = circ * (c * _CircCol.a/* + (1 - c) * t*/);

                col = circColor.a * circColor + (1 - circColor.a) * col;

                
                return col;
            }
            ENDCG
        }
    }
}