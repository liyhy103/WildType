Shader "UI/HoleMask"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,0.7)
        _HoleCenter("Hole Center", Vector) = (0.5, 0.5, 0, 0)
        _HoleSize("Hole Size", Vector) = (0.1, 0.1, 0, 0)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            float4 _HoleCenter;
            float4 _HoleSize;
            fixed4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 dist = abs(uv - _HoleCenter.xy);
                if (dist.x < _HoleSize.x * 0.5 && dist.y < _HoleSize.y * 0.5)
                    discard;
                return _Color;
            }
            ENDCG
        }
    }
}