// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
Shader "Custom/Billboarding"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color Tint",color) = (1,1,1,1)
        _Width("Width", float) = 0.05
    }
        SubShader
        {
            //对顶点进行变换需禁用合P
            Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "true" "DisableBatching" = "True" }

            Pass
            {
                //透明度混合
                Tags{ "lightmode" = "forwardbase" }
                ZWrite off
                ZTest Off
                Blend SrcAlpha OneMinusSrcAlpha
                Cull off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

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
                fixed4 _Color;
                float _Width;

                v2f vert(appdata v)
                {
                    v2f o;
                    //计算模型空间中的视线方向
                    float3 objViewDir = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1));

                    //计算旋转矩阵的各个基向量
                    float3 normalDir = normalize(objViewDir);
                    float3 upDir = float3(0, 1, 0);
                    float3 rightDir = normalize(cross(normalDir, upDir));
                    upDir = normalize(cross(normalDir, rightDir));

                    //用旋转矩阵对顶点进行偏移
                    float3 localPos = rightDir * v.vertex.x + upDir * v.vertex.y + normalDir * v.vertex.z;

                    //将偏移之后的值作为新的顶点传递计算
                    o.vertex = UnityObjectToClipPos(float4(localPos,1));
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    if ((i.uv.x > (1 - _Width) || i.uv.x < _Width) || (i.uv.y < _Width || i.uv.y > (1 - _Width)))
                        col.rgb *= _Color.rgb;
                    else
                        discard;
                    return col;
                }
                ENDCG
            }
        }
            fallback "Transparent/VertexLit"
}