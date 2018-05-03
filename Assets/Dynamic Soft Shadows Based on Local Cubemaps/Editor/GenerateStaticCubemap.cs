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
using UnityEditor;
using System.Collections;

/**
 * Generate Static Cubemap script
 * 
 * This class bakes the static cubemap for shadows. It renders the scene in the RGB channels
 * and the transparency of the scene in the alpha channel. The value rendered in
 * the alpha channel is used for rendering shadows from static geometry.
 * 
 * Before rendering the cubemap tick the checkbox "Rendering Cubemaps" in
 * the Unified Shader Control script.  
 * 
 */
public class GenerateStaticCubemap : ScriptableWizard
{
	public GameObject	renderPosition;
	public Cubemap		cubemap;
	// Camera settings
	public int			cameraDepth = 24;
	public LayerMask	cameraLayerMask = -1;
	public Color		cameraBackgroundColor = new Color(1, 1, 1, 0);
	public float		cameraNearPlane = 0.1f;
	public float		cameraFarPlane = 100.0f;
	public float		cameraFOV = 60.0f;
	public bool			cameraUseOcclusion = true;
	//Cubemap settings
	public FilterMode 	cubemapFilterMode = FilterMode.Trilinear;

	void OnWizardUpdate()
	{
		helpString = "Select transform to render from" + "and cubemap to render into";
		if (renderPosition != null && cubemap != null)
		{
			isValid = true;
		}
		else
		{
			isValid = false;
		}
	}

	void OnWizardCreate()
	{
		// Set antialiasing
		QualitySettings.antiAliasing = 4;

		Material currentSkyboxMat = RenderSettings.skybox;
		// Remove the skybox
		RenderSettings.skybox = null;

		// Create temporary camera for rendering
		GameObject go = new GameObject("CubeCam", typeof(Camera));
		go.GetComponent<Camera>().depth = cameraDepth;

		go.GetComponent<Camera>().backgroundColor = cameraBackgroundColor;
		go.GetComponent<Camera>().cullingMask = cameraLayerMask;
		go.GetComponent<Camera>().nearClipPlane = cameraNearPlane;
		go.GetComponent<Camera>().farClipPlane = cameraFarPlane;
		go.GetComponent<Camera>().fieldOfView = cameraFOV;
		go.GetComponent<Camera>().useOcclusionCulling = cameraUseOcclusion;

		cubemap.filterMode = cubemapFilterMode;

		// Place camera on the render position
		go.transform.position = renderPosition.transform.position;
		go.transform.rotation = Quaternion.identity;					

		// Render the cubemap
		go.GetComponent<Camera>().RenderToCubemap(cubemap);

		// Restore skybox
		RenderSettings.skybox = currentSkyboxMat;
		
		// Destroy temporary camera
		DestroyImmediate(go);
	}

	[MenuItem("Tools/Render into Cubemap")]
	static void RenderCubemap()
	{
		ScriptableWizard.DisplayWizard("Render Cubemap", typeof(GenerateStaticCubemap),"Render!");
	}

}
