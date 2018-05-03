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
using System.Collections.Generic;

/*
 * Info to Shaders script
 * 
 * This script sends the necessary information to materials that 
 * receive static shadows i.e. the static geometry of the room and
 * the chess piece which is a dynamic geometry.
 * The info is related to light position, cubemap position and its min/max points.
 * 
 */

[ExecuteInEditMode] 
public class InfoToShaders: MonoBehaviour
{
	public Material[]	shadowsMats;
	public GameObject	shadowsLight;
	public GameObject	roomBBox;

	// Use this for initialization
	void Start()
	{
		// The BBox size will be constant and can be passed in the Start
		Vector3 BBoxCenter = roomBBox.transform.position;
		Vector3 bboxLenght = roomBBox.transform.localScale;
		// In world coordinates
		Vector3 BBoxMin = BBoxCenter - bboxLenght/2;
		Vector3 BBoxMax = BBoxCenter +	bboxLenght/2;

		// Pass BBox data to materials static geometry
		for (int i = 0; i < shadowsMats.Length; i++)
		{
			shadowsMats[i].SetVector("_ShadowsCubeMapPos", BBoxCenter);
			shadowsMats[i].SetVector("_BBoxMin", BBoxMin);
			shadowsMats[i].SetVector("_BBoxMax", BBoxMax);
		}

		PassLightPositionToShaders ();
	}
	
	// Update is called once per frame
	void Update()
	{
		PassLightPositionToShaders ();
	}

	void PassLightPositionToShaders()
	{
		if (shadowsLight != null)
		{
			Vector3 lightPosition01 = shadowsLight.transform.position;
			// Pass the light position to materials
			for (int i = 0; i < shadowsMats.Length; i++)
			{
				shadowsMats[i].SetVector("_ShadowsLightPos", lightPosition01);
			}
		}

	}
}
