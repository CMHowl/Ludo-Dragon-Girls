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
 * Screen Resolution script.
 * 
 * Helper class to set the target resolution defined by the user.
 * 
 */

public class ScreenResolution : MonoBehaviour 
{
	public bool use1920x1080;

	int 	nativeResH;
	int 	nativeResW;
	float 	resRatio;


	public float GetResRatio()
	{
		resRatio = (float)Screen.currentResolution.height / (float)nativeResH;
		return resRatio;
	}

	// Default target resolution 1280 x 720
	public void Res720()
	{
		float resFactor = 1280.0f / nativeResW;
		int newResHeight = (int)(nativeResH * resFactor);
		Screen.SetResolution(1280, newResHeight, true);
	}

	// 1920 x 1080 
	public void Res1080()
	{
		float resFactor = 1920.0f / nativeResW;
		int newResHeight = (int)(nativeResH * resFactor);
		Screen.SetResolution(1920, newResHeight, true);
	}

	// Calculate the resRatio in the Awake before the Start() function of the 
	// components that use this script: MainGUI and FPSCount.
	void Awake()
	{
		// Capture the native resolution.
		nativeResH = Screen.height;
		nativeResW = Screen.width;

        if (use1920x1080)
        {
            Res1080();
        }
        else
        {
			Res720();
		}
        
		resRatio = (float)Screen.currentResolution.height / nativeResH;

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	void Start() 
	{
		// Set the antialiasing level.
		QualitySettings.antiAliasing = 4;
	}
}