// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
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
 * This shader is used to render the geometry of the room and the chessboard.
 *
 */

Shader "Custom/roomShadows" {
	Properties {
	  	_MainTex ("Base (RGB)", 2D) = "white" {}
	  	// ------------ Dynamic shadows ------------------
    	_ShadowsTex ("Dyn Runtime Shadows (RGB)", 2D) = "" {}
	  	// ------------ Static shadows ---------------------
    	_Cube("Static Shadow Map", Cube) = "" {}
    	_ShadowFactor("Static Shadow Factor", Float) = 1.0
    	_AngleThreshold("Static Shadow Angle Threshold", Float) = 0.0
   }
   
   SubShader {
      Pass {    
        Tags { "LightMode" = "ForwardBase" } 
  
        CGPROGRAM
		
		#pragma target 3.0
		//#pragma glsl
		#pragma vertex vert  
		#pragma fragment frag
		// If no keyword is enabled then the first declared here will be considered as enabled
	    #pragma multi_compile CUBEMAP_RENDERING_OFF CUBEMAP_RENDERING_ON

		#include "UnityCG.cginc" 
		#include "Common.cginc" 
		
		uniform float4		_LightColor; 
		// User-specified properties
		uniform sampler2D	_MainTex;
		uniform float4		_AmbientColor; 
		         
		// ------------ Shadows ---------------------------
		uniform float3		_BBoxMin;
		uniform float3		_BBoxMax;
		uniform float3		_ShadowsCubeMapPos;

		uniform samplerCUBE _Cube;
		
		uniform float3		_ShadowsLightPos;
		
		uniform float		_ShadowFactor;
		uniform float		_AngleThreshold;
		uniform float4		_ShadowsTint;
		uniform float		_ShadowLodFactor;
		
		// Dynamic shadows
        uniform sampler2D	_ShadowsTex;
        uniform float4x4	_ShadowsViewProjMat;
         
        uniform float		_LightToShadowsContrast;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float4 texcoord : TEXCOORD0;
            float3 normal : NORMAL;
         };
         
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
            float4 vertexInWorld : TEXCOORD1;
            float3 normalWorld : TEXCOORD2;
            // ------- Static Shadows ------
            float3 vertexToLightInWorld : TEXCOORD3;
            // -------- Dynamic Shadows -------------
	        float4 shadowsVertexInScreenCoords :  TEXCOORD4;

         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = unity_ObjectToWorld;
            float4x4 modelMatrixInverse = unity_WorldToObject; 
              
			output.vertexInWorld = mul(modelMatrix, input.vertex);
			
            output.normalWorld = normalize((mul(float4(input.normal, 0.0), modelMatrixInverse)).xyz);
       
            output.tex = input.texcoord;
            output.pos = UnityObjectToClipPos(input.vertex);
                        
            
            // ------------ Shadows ---------------------------
            output.vertexToLightInWorld = _ShadowsLightPos - output.vertexInWorld.xyz;
            
            
            // ------------ Runtime shadows texture ----------------
			// ApplyMVP transformation from shadow camera to the vertex
			float4 vertexShadows = mul(_ShadowsViewProjMat, output.vertexInWorld);

			output.shadowsVertexInScreenCoords = ComputeScreenPos(vertexShadows);
            
            return output;
         }
         
  
         float4 frag(vertexOutput input) : COLOR
         {
         	float4 finalColor = float4(1.0, 1.0, 0.0, 1.0);
            float3 normalDirection = input.normalWorld;
            float4 texColor = tex2D(_MainTex, input.tex.xy);
           	
           	// ------------ Static shadows ---------------------------
           	float shadowColor = 0.0;
         	// The interpolated vertex position, which is the pixel position in WC
         	float3 PositionWS = input.vertexInWorld;
           	
           	// Check if this pixel could be affected by shadows from light source
           	float3 vertexToLightWS = normalize(input.vertexToLightInWorld);
           	float dotProd = dot(normalDirection, vertexToLightWS);

           	if (dotProd > _AngleThreshold)
           	{
				// Apply local correction to vertex-light vector
				float4 correctVecAndLodDist = LocalCorrectAndLodDist(vertexToLightWS, _BBoxMin, _BBoxMax, PositionWS, _ShadowsCubeMapPos);
				
				// Fetch the local corrected vector 
				float3 correctVertexToLightWS = correctVecAndLodDist.xyz;
				// Fetch the distance from the pixel to the intersection point in the BBox which
				// will be used as a LOD level selector
				float lodDistance = correctVecAndLodDist.w;
				// Apply the factor which can be 
				lodDistance *= 0.01 * _ShadowLodFactor;
				
				// The LOD level is passed to the texCUBElod in the w component of the vector.
				// Form that vector
				float4 tempVec = float4(correctVertexToLightWS,  lodDistance);
				// Fetch the color at a given LOD
				float4 tempCol = texCUBElod(_Cube, tempVec);
				
				// The shadow color will be the alpha component.
				shadowColor = (1.0 - tempCol.a) * _ShadowFactor;
				// Smooth cut out between light and shadow 
				shadowColor *= (1.0 - smoothstep(0.0, _AngleThreshold, dotProd));
			}
			
			// ---------------- Dynamic shadows of chess pieces ------------
			float4 dynShadowsColor = tex2Dproj( _ShadowsTex, UNITY_PROJ_COORD(input.shadowsVertexInScreenCoords) );
			// -------------- Combine static and dynamic shadows -----------
			float4 shadowsCombiColor;
			shadowsCombiColor.rgb = shadowColor * (1.0 - dynShadowsColor.r) * _ShadowsTint;
			
			shadowsCombiColor.a = 1.0;
	
			#if CUBEMAP_RENDERING_ON
				finalColor = texColor;
			#endif

			#if CUBEMAP_RENDERING_OFF
	           	// Combine colors to get the final pixel color
		        finalColor = _AmbientColor * texColor   +  texColor *  shadowsCombiColor * _LightToShadowsContrast;
            #endif
            
            
            return finalColor;
            
         } //end of frag
 
         ENDCG
      } //end of pass
 
      
   }// end of subshader
  
}