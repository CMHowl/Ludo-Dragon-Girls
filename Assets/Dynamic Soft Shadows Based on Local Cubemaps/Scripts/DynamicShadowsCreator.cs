/*
 * This confidential and proprietary software may be used only as
 * authorised by a licensing agreement from ARM Limited
 * (C) COPYRIGHT 2016 ARM Limited
 * ALL RIGHTS RESERVED
 * The entire notice above must be reproduced on all authorised
 * copies and copies may only be made to the extent permitted
 * by a licensing agreement from ARM Limited.
 */

using UnityEngine;
using System.Collections;

/*
 * Dynamic Shadows Creator script
 * 
 * This class is responsible for creating the camera for rendering shadows and
 * the shadow texture. This script places the camera at the light position
 * and orients it to the center of the chess board (shadowCamTarget).
 * The shadow camera only renders the shadows from dynamic objects, i.e.
 * the chess pieces.
 * The dynamic objects must be in the layer DynObjects.
 * When rendering the shadows the camera uses a replacement shader (shadowMapShader)
 * for all chess pieces.
 * 
 */

[ExecuteInEditMode]
public class DynamicShadowsCreator : MonoBehaviour
{
	public int					shadowResolution 	= 1024;
	public LayerMask 			layerMask 			= -1;
	public RenderTextureFormat 	format 				= RenderTextureFormat.ARGB32;
	public int 					antialiasSamples 	= 4;
	public Shader 				shadowMapShader 	= null;
	public GameObject 			shadowCamTarget 	= null;
	
	private RenderTexture 		shadowTexture 		= null;
	private GameObject 			shadowCamera 		= null;
	
	void OnValidate()
	{
		throwExceptionIfInvalidParams();
		if (shadowTexture != null)
		{
			SetShadowTextureParams();
		}
		if (shadowCamera != null)
		{
			SetCameraParams();
		}
	}
	
	private void throwExceptionIfInvalidParams()
	{
		if (shadowCamTarget == null)
		{
			throw new UnityException(name + "'s cube shadow source must not be null");
		}
		if (shadowResolution <= 0 || shadowResolution > 2048)
		{
			throw new UnityException(name + "'s shadow resolution must be between 1 and 2048 inclusive");
		}
		if (shadowMapShader == null)
		{
			throw new UnityException(name + "'s shadow map shader must be non null");
		}
	}
	
	void LateUpdate()
	{

		// Only update the dynamic shadows if it has been created - we lazily create it
		if (shadowTexture != null && shadowCamera != null)
		{
			RenderShadows();	
		}
	}
	
	private void InitializeIfNotAlreadyInitialized()
	{
		if (shadowTexture == null)
		{
			CreateShadowTexture();
			SetShadowTextureParams();
		}
		
		if (shadowCamera == null)
		{
			CreateShadowCamera();
			SetCameraParams();
		}
	}
	
	private void CreateShadowTexture()
	{
		shadowTexture = new RenderTexture(1, 1, 1, format);
	}
	
	private void SetShadowTextureParams()
	{
		int tempTexRes = shadowResolution;
		
		// Use 128 * 128 textures for in editor, to allow per frame generation with low GPU usage
		if (!Application.isPlaying)
		{
			tempTexRes = 128;
		}
		
		shadowTexture.Release();
		shadowTexture.hideFlags = HideFlags.HideAndDontSave;
		shadowTexture.width = tempTexRes;
		shadowTexture.height = tempTexRes;
		shadowTexture.depth = 0;
		shadowTexture.format = format;
		shadowTexture.autoGenerateMips = true;
		shadowTexture.useMipMap = true;
		shadowTexture.filterMode = FilterMode.Trilinear;
		shadowTexture.isPowerOfTwo = true;
		shadowTexture.antiAliasing = antialiasSamples;

		shadowTexture.name = "ShadowOnStaticObjs";
	}
	
	private void CreateShadowCamera()
	{
		shadowCamera = new GameObject();
		shadowCamera.name = "ShadowCamera";
		shadowCamera.hideFlags = HideFlags.DontSave;
		shadowCamera.AddComponent<Camera>();		
	}
	
	private void SetCameraParams()
	{
		var cam = shadowCamera.GetComponent<Camera>();
		cam.enabled = false;
		cam.backgroundColor = new Color(0, 0, 0, 1);
		cam.clearFlags = CameraClearFlags.SolidColor;
		cam.targetTexture = shadowTexture;
		cam.cullingMask = layerMask;
		cam.SetReplacementShader(shadowMapShader, "");
		UpdateCameraTransform();
	}
	
	private void UpdateCameraTransform()
	{
		var cam = shadowCamera.GetComponent<Camera>();
		cam.transform.position = transform.position;
		cam.transform.LookAt(shadowCamTarget.transform.position);
	}
	
	private void RenderShadows()
	{
		UpdateCameraTransform();
		shadowCamera.GetComponent<Camera>().Render();
	}
	
	public struct DynamicShadowsData
	{
		public RenderTexture shadowTexture;
		public Matrix4x4 viewProjMatrix;
	}
	
	public DynamicShadowsData GetShadowData()
	{
		InitializeIfNotAlreadyInitialized();

		DynamicShadowsData data;
		data.shadowTexture = shadowTexture;
		var cam = shadowCamera.GetComponent<Camera>();
		data.viewProjMatrix = cam.projectionMatrix * cam.worldToCameraMatrix;
		return data;
	}
	
	void OnDestroy()
	{
		DestroyImmediate(shadowTexture);
		DestroyImmediate(shadowCamera);
	}
}

