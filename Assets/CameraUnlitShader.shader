Shader "Unlit/CameraUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;

            };

            struct v2f
            {
        	    float3 normal : NORMAL;
	            float2 texcoord : TEXCOORD0;
	            UNITY_VPOS_TYPE vpos : VPOS;
                out float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert( appdata v, out float4 vertex : SV_POSITION)
            {
	            v2f o = (v2f)0;
	            vertex = UnityObjectToClipPos( v.vertex);
	            o.texcoord = float2( v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw);
	            return o;
            }

            sampler2D _GradationMap;
            sampler2D _ToonMap;
            float4 _GradationMap_ST;
            float4 _GradationMap_TexelSize;
            float _Alpha;

            half4 frag( v2f i) : SV_Target
            {
	            if( _Alpha == 0)
	                {
	                	discard; // ’†Ž~
	                }
	            float _CellSize = _GradationMap_TexelSize.w;

	            float2 localuv = fmod( i.vpos.xy, _CellSize) / _GradationMap_TexelSize.zw;
	            float alpha = floor(_Alpha * 32.0) / 32.0;
	            localuv.x += alpha;

	            float3 gradation = tex2D( _GradationMap, localuv).rgb;
	            if( gradation.r < 0.5)
	                {
	                	discard; // ’†Ž~
	                }
	            float3 sample = tex2D( _MainTex, i.texcoord).rgb;
	            return float4( sample, 1);
            }
            ENDCG
        }
    }
}
