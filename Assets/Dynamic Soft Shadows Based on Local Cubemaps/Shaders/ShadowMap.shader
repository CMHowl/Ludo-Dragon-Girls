// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
 * This confidential and proprietary software may be used only as
 * authorised by a licensing agreement from ARM Limited
 * (C) COPYRIGHT 2016 ARM Limited
 * ALL RIGHTS RESERVED
 * The entire notice above must be reproduced on all authorised
 * copies and copies may only be made to the extent permitted
 * by a licensing agreement from ARM Limited.
 */
 
/*
 * This is the replacement shader used when rendering shadows.
 *
 */
 
Shader "Custom/ctShadowMap" {
   SubShader {
      Pass {
  		 ZWrite off
  		 ZTest off
  		 ColorMask R
         
         CGPROGRAM
         #pragma target 3.0
		 #pragma glsl

         #pragma vertex vert
         #pragma fragment frag

         struct vertexInput {
            float4 vertex : POSITION;
         };

         struct vertexOutput {
            float4 pos : SV_POSITION;
         };

         vertexOutput vert(vertexInput input)
         {
            vertexOutput output;
            output.pos = UnityObjectToClipPos(input.vertex);
            return output;
         }

         float4 frag(vertexOutput input) : COLOR
         {
			return float4(1,1,1,1);
         }

         ENDCG
      }
   }
}
