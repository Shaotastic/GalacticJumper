Shader "Hidden/InternalErrorShader"
{
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON
			#include "UnityCG.cginc"
			struct a2v {
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct v2f {
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert (a2v v) { 
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target { return fixed4(1,0,1,1); }
			ENDCG
		}
	}
	Fallback Off
}
