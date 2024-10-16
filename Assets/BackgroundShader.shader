Shader "Unlit/BackgroundShader"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (1,1,1,1)
        _BottomColor ("Bottom Color", Color) = (0,0,0,1)
        _CenterColor ("Center Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _TopColor;
            fixed4 _BottomColor;
            fixed4 _CenterColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                // UV座標を計算
                o.uv = o.position.xy * 0.5 + 0.5;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UVを使ってグラデーションを生成
                // 上下グラデーション
                float gradientFactor = i.uv.y;
                fixed4 gradientColor = lerp(_BottomColor, _TopColor, gradientFactor);

                // 中心を薄くする効果
                float2 center = float2(0.5, 0.5);
                float distanceFromCenter = distance(i.uv, center);
                float fadeFactor = smoothstep(0.0, 10, distanceFromCenter);

                // 中心色とグラデーション色をブレンド
                fixed4 finalColor = lerp(_CenterColor, gradientColor, fadeFactor);
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
