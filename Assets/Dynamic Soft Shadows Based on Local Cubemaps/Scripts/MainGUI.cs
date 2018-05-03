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
 * Main GUI script.
 * 
 * The class controls the GUI elements: slides and logo.
 * In Editor Mode the FlyingCamera script controls the
 * camera by means of the keyboard arrow keys and the mouse. In this mode
 * the joysticks are disabled as well as the logo.
 * 
 * When running on the device the camera uses the GameCamera script.
 * In this mode the joysticks are active and they are used to control the camera.
 * The Mali logo is active.
 * Right slider controls the shadows LOD factor.
 * Left slider controls the Z value of light position coodinates. 
 * 
 */

public class MainGUI : MonoBehaviour
{
	// Game objects
	public GameObject	lightSource;

	public int			logoSpaceX		= 5;
	public int			logoSpaceY		= 5;
	public GameObject	unifiedController;
	public GameObject	screenResolution;

	float	vRightSliderValue;
	float	vLeftSliderValue;

	// For shadows LOD factor
	float	minRightSliderVal	= 0;
	float	maxRightSliderVal	= 5;

	// For light position
	float	minLeftSliderVal	= -60;
	float	maxLeftSliderVal 	= 60;

	// Width of the slider collider
	int		sliderWidth			= 60;
	int		sliderHeight 		= 300; 
	int 	sliderSpacingLeft	= 0;
	int 	sliderSpacingRight 	= 10;

	Rect 	rightSliderRect;
	Rect	leftSliderRect;

	float	resRatio;
	GameObject maliLogo;

	public GUISkin customSkin;

	void Awake()
	{
		// Initialize right slider to the value of shadows LOD factor
		vRightSliderValue = unifiedController.GetComponent<UnifiedShaderControl>().shadowsLodFactor;
		// Initialize left slider to the value of light position
		vLeftSliderValue = lightSource.transform.position.z;
	}

	void Start()
	{
		maliLogo = GameObject.Find("MaliLogo");
		resRatio = screenResolution.GetComponent<ScreenResolution>().GetResRatio();

		// GUI elements are active only in the device
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			// Disable left joystick
			GameObject leftJoystick = GameObject.Find("LeftJoystick");
			leftJoystick.SetActive(false);
			GameObject leftJoystickFixed = GameObject.Find("LeftJoystickFixed");
			leftJoystickFixed.SetActive(false);
			// Disable right joystick
			GameObject rightJoystick = GameObject.Find("RightJoystick");
			rightJoystick.SetActive(false);
			GameObject rightJoystickFixed = GameObject.Find("RightJoystickFixed");
			rightJoystickFixed.SetActive(false);

			// Disable GameCamera script			
			GameObject mainCamera = GameObject.Find("MainCamera");
			mainCamera.GetComponent<GameCamera>().enabled = false;
			// Enable FlyingCamera script
			mainCamera.GetComponent<FlyingCamera>().enabled = true;

			// Disable Logo
			maliLogo.SetActive(false);
		}
		else
		{
			// Place the Logo
			int logoWidth = (int)maliLogo.GetComponent<GUITexture>().pixelInset.width;
			int logoHeight = (int)maliLogo.GetComponent<GUITexture>().pixelInset.height;
			maliLogo.GetComponent<GUITexture>().pixelInset = new Rect(0, 0.95f * resRatio, logoWidth * resRatio, logoHeight * resRatio);
			maliLogo.GetComponent<GUITexture>().pixelInset = new Rect(0, logoSpaceY, logoWidth * resRatio, logoHeight * resRatio);
		}

		// Initialize right slider to the value of shadows LOD factor
		vRightSliderValue = unifiedController.GetComponent<UnifiedShaderControl>().shadowsLodFactor;
		// Initialize left slider to the value of light position
		vLeftSliderValue = lightSource.transform.position.z;
	}

	// Update is called once per frame
	void Update()
	{

		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			return;
		}

		Vector3 lightPos = lightSource.transform.position;
		//Vector3 newLightPos = new Vector3(lightPos.x, vRightSliderValue, vLeftSliderValue);
		Vector3 newLightPos = new Vector3(lightPos.x, lightPos.y, vLeftSliderValue);
		lightSource.transform.position = newLightPos;

		// Update the value of the unifiedController shadows LOD factor
		unifiedController.GetComponent<UnifiedShaderControl>().shadowsLodFactor = vRightSliderValue;

		float rightSliderXLeft = (float)Screen.width -  (sliderSpacingRight + sliderWidth) * resRatio;
		float rightSliderYTop = (float)Screen.height/2  - (sliderHeight * resRatio);
		
		// Left slider
		float leftSliderXLeft = (sliderSpacingLeft + sliderWidth) * resRatio;
		float leftSliderYTop = (float)(Screen.height)/2 - (sliderHeight * resRatio);
		
		rightSliderRect = new Rect(rightSliderXLeft, rightSliderYTop, sliderWidth * resRatio, sliderHeight  * resRatio);
		
		leftSliderRect = new Rect(leftSliderXLeft, leftSliderYTop, sliderWidth * resRatio, sliderHeight  * resRatio);
	}

	// Events correspond to user input (key presses, mouse actions).
	// For each event OnGUI is called in the scripts.
	// We will simulate here the touch event that is implemented in Update
	void OnGUI()
	{
		GUI.depth = 0;
		GUI.skin = customSkin;

		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			return;
		}

		vRightSliderValue = GUI.VerticalSlider(rightSliderRect, vRightSliderValue, maxRightSliderVal, minRightSliderVal);
		vLeftSliderValue = GUI.VerticalSlider(leftSliderRect, vLeftSliderValue, maxLeftSliderVal, minLeftSliderVal);
	}

}
