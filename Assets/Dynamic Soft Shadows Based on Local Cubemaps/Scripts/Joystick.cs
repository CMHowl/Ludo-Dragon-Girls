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

public enum JoystickType {RightJoystick, LeftJoystick};

/*
 * Joystick script.
 * 
 * This script implements a joystick.
 * Important!
 * The GUI Textures associated with the Joystick  must have a transform scale
 * set to (0,0,0,) to prevent textures from scaling with screen resolution,
 * and keeping them in a fixed pixel size. 
 * A scaling factor as a fraction of the screen resolution is used to keep
 * the size of the texture constant independent of screen resolution.
 * User's touch emulation by means of mouse events  is implemented in OnGUI() function. 
 * The joystick appears when the user touches the left or right 1/3rd of the screen.
 * The central part of the screen is left available for interaction of the user with the chess piece.
 * 
 */

public class Joystick : MonoBehaviour
{
	// The type of joystick: Right or Left
	public JoystickType joystickType;
	// Maximum value of joystick displacement.
	// It is modulated by the scaling factor.
	public float jTransClampValue;
	// Duration of fade out in seconds
	public float fadeOutDuration;
	// Duration of fade in seconds
	public float fadeInDuration; 
	// How long the user must holds the finger for joystick to fade in
	public float joystickHoldThreshold;
	// The fixed GUI texture 
	public GUITexture fixedJoystickTex;
	// The scale we want to apply to the texture as a percentaje of screen resolution. 
	public float guiTexScaleFactor = 0.001f;
	// The value the object controlled by joystick should read
	public Vector2 jTrans = new Vector2(0,0);
	
	// The joystick's GUI Texture game object.
	GUITexture guiTex;
	// The position/extents of the joystick. 
	Rect guiTexRect;
	// The rectangle where joystick reponds for the touches.
	Rect touchRec;
	// Center of joystick
	Vector2 guiCenter;
	
	// GUI texture size
	int guiTexWidth;
	int guiTexHalfWidth;
	int guiTexHeight;
	int guiTexHalfHeight;
	int fixedJoystickTextWidth;
	int fixedJoystickTextHalfWidth;
	int fixedJoystickTextHeight;
	int fixedJoystickTextHalfHeight;
	
	bool firstTouchInTouchRec = false;
	int fingerId = -1;
	bool clickOverJoystick = false;
	Vector2 clickOverJoystickCoords;
	// For mouse touch emulation.
	bool drag = false;
	
	static float VERY_HIGH_TIME = 1.0e+6f;
	float timeBeganTouch = VERY_HIGH_TIME;
	// Is the GUI control visible
	bool guiVisible = false;
	// When true the object is rendered
	bool guiActive = false;
	// The color used fade the GUI element
	Color fadeControlColor;
	// If true the fade out runs
	bool runFadeOut = false;
	// If true the fade in runs
	bool runFadeIn = false;
	// The time when the fade process begins
	float fadeStartTime;
	
	
	// Properties
	public bool GUIVisible
	{
		get{return guiVisible;}
		set
		{
			guiVisible = value;
			if (value)
			{
				fadeControlColor.a = 1.0f; 
			}
			else
			{
				fadeControlColor.a = 0.0f;
			}
		}
	}
	
	public bool GUIActive
	{
		get{return guiActive;}
		set{guiActive = value;}
	}
	
	// GUI Tex Initialization.
	public void InitializeGUITex()
	{ 
		Rect rect;
		
		if (joystickType == JoystickType.RightJoystick)
		{
			// Set some default rec. It will be overriden by the user touch.
			rect = new Rect(Screen.width - Screen.width / 3  + Screen.width / 6, (Screen.height / 6 ), guiTexWidth, guiTexHeight);
		}
		else
		{
			// Set some default rec. It will be overriden by the user touch.
			rect = new Rect(Screen.width / 6, Screen.height / 6, guiTexWidth, guiTexHeight);
		}
		
		guiTex.pixelInset = rect;
		SetGUITexRec();
		
	}

	void SetJoystickTouchRec()
	{
		
		if (joystickType == JoystickType.RightJoystick)
		{
			touchRec = new Rect(Screen.width - Screen.width / 3, 2 * Screen.height / 3, Screen.width / 3 , Screen.height / 3);
		}
		else
		{

			// Leave space for the slider = Screen.width / 6 and keep the central part = Screen.width/3
			touchRec = new Rect(0, 2 * Screen.height / 3 , Screen.width / 3, Screen.height / 3);
		}
		
	}

	void Awake()
	{
		// The scale factor as a fraction of the screen resolution.
		float guiTexScreenResScaleFactor = Screen.height * guiTexScaleFactor;
		jTransClampValue *= guiTexScreenResScaleFactor;
		
		guiTex = gameObject.GetComponent<GUITexture>();
		guiTexWidth = (int)(gameObject.GetComponent<GUITexture>().pixelInset.width * guiTexScreenResScaleFactor);
		guiTexHalfWidth = guiTexWidth / 2;
		guiTexHeight = (int)(gameObject.GetComponent<GUITexture>().pixelInset.height  * guiTexScreenResScaleFactor);
		guiTexHalfHeight = guiTexHeight / 2;
		
		fixedJoystickTextWidth = (int)(fixedJoystickTex.GetComponent<GUITexture>().pixelInset.width  * guiTexScreenResScaleFactor);
		fixedJoystickTextHalfWidth = fixedJoystickTextWidth / 2;
		fixedJoystickTextHeight = (int)(fixedJoystickTex.GetComponent<GUITexture>().pixelInset.height  * guiTexScreenResScaleFactor);
		fixedJoystickTextHalfHeight = fixedJoystickTextHeight / 2;
		
		// Cache the color to change later only the alpha component.
		fadeControlColor = guiTex.color;
		fadeControlColor.a = 0.0f;
		guiTex.color = fadeControlColor;
		fixedJoystickTex.color = fadeControlColor;
	}
	
	// Use this for initialization
	void Start() 
	{
		InitializeGUITex();
		// We set here the touch region for each Joystick but it is not enough. In the case we change
		// the native resolution of the device the new resolution will not be available until the window
		// is created this is why the SetJoystickTouchRec() function must be called also in the Update() function.
		SetJoystickTouchRec();
		GUIActive = true;
		GUIVisible = false;
	}
	
	void Disable()
	{
		gameObject.SetActive(false);
	}
	
	void ResetJoystick()
	{
		// Release the finger control and set the joystick back to the default position.
		guiTex.pixelInset = guiTexRect;
		fingerId = -1;
		jTrans = Vector2.zero;;
		clickOverJoystick = false;
	}
	
	// Defines the user interaction.
	void OnGUI()
	{
		// Emulate screen touches with mouse events.
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			// If snapshot mode is on draw no joystick.
			if (!guiActive)
			{
				return;
			}
			
			// Set the alpha for fade-in and fade-out.
			// As the joystick textures are semi-transparent (alpha = 0.5) we set only the half of the fade color
			guiTex.color = fadeControlColor / 2;
			fixedJoystickTex.color = fadeControlColor / 2;
			
			// The mouse event
			Event e = Event.current;
			
			if (e.isMouse)// && guiResponsive)
			{
				Vector2 touchPos = new Vector2(e.mousePosition.x, Screen.height - e.mousePosition.y);
				
				// The simulation to place the joystick where the user clicks with the mouse
				// and handle the fade in and fade out effects. 
				if (e.type == EventType.MouseDown)
				{
					// If mouse down is inside the touch rec then the joystick is shown.
					if (touchRec.Contains(e.mousePosition))
					{
						Vector3 screenPos = new Vector3(e.mousePosition.x, Screen.height - e.mousePosition.y, 0);
						
						if ( (!guiVisible && !runFadeIn) || guiVisible )
						{
							// If the Joystick is visible and the fade out is runing.
							if (runFadeOut)
							{
								// We stop the fade out and ...
								runFadeOut = false;
								// Start the fade in.
								fadeStartTime = Time.time;
								runFadeIn = true;
							}
							
							// If the Joystick is visible.
							if (guiVisible)
							{
								// Check if the event takes place on the joystick.
								if ( guiTex.HitTest(screenPos))
								{
									// If the event takes place on the joystick then keep it were it was.
									clickOverJoystick = true;
									clickOverJoystickCoords = new Vector2(e.mousePosition.x, e.mousePosition.y);
								}
								else
								{
									// Set new joystick position
									guiTex.pixelInset = new Rect(touchPos.x - guiTexHalfWidth, touchPos.y - guiTexHalfHeight, guiTexWidth, guiTexHeight);
									// As we are creating new default position we call SetDefaultRect().
									SetGUITexRec();
									guiVisible = true;									
								}
								
								// Make sure the translation vector is null.
								jTrans = Vector2.zero;
								drag = true;
							}
							else
							{
								// If the joystick is not visible at all then place the joystick where the user
								// made the mouse down and start the fade in.
								fadeStartTime = Time.time;
								runFadeIn = true;
								// Place the GUI Texture object
								// Define the pixel inset rectangle
								guiTex.pixelInset = new Rect(touchPos.x - guiTexHalfWidth, touchPos.y - guiTexHalfHeight, guiTexWidth, guiTexHeight);
								// Set fixed joystick texture position
								fixedJoystickTex.pixelInset = new Rect(touchPos.x - fixedJoystickTextHalfWidth, touchPos.y - fixedJoystickTextHalfHeight, fixedJoystickTextWidth, fixedJoystickTextHeight);
								// As we are creating a default position we call SetDefaultRect();
								SetGUITexRec();
								guiVisible = true;
								// Make sure the translation vector is null.
								jTrans = Vector2.zero;
								drag = true;
							}
						}
					}
				}
				
				if (drag && e.type == EventType.MouseUp)
				{
					drag = false;
					// Update GUI Texture position
					guiTex.pixelInset = new Rect(touchPos.x - guiTexHalfWidth, touchPos.y - guiTexHalfHeight, guiTexWidth, guiTexHeight);
					jTrans = Vector2.zero;
					clickOverJoystick = false;
					
					// If the fade out is running do nothing, just let it finish.
					if (!runFadeOut)
					{
						fadeStartTime = Time.time;
						runFadeOut = true;
						// Make sure the translation vector is null.
						jTrans = Vector2.zero;
					}
				}
				
				// We can't use here only the built in drag event as joystick's drag is originated only in the touch rec.
				if (drag && e.type == EventType.MouseDrag)
				{
					Vector2 guiTrans = guiCenter - touchPos;
					
					if (clickOverJoystick)
					{
						// To get the right translation value for the case when the user hit over the existing joystick.
						guiTrans = new Vector2( clickOverJoystickCoords.x - e.mousePosition.x, -(clickOverJoystickCoords.y - e.mousePosition.y));
					}
					
					jTrans = Vector2.ClampMagnitude(guiTrans, jTransClampValue);
					Vector2 guiPos = guiCenter - jTrans;
					// Update GUI Texture position
					guiTex.pixelInset = new Rect(guiPos.x - guiTexHalfWidth, guiPos.y - guiTexHalfHeight, guiTexWidth, guiTexHeight);
					
					// What is expected by a Joystick user is 
					jTrans /= jTransClampValue;
				}
				
			}
			
			if (runFadeIn)
			{
				GUIFadeIn();
			}
			
			if (runFadeOut)
			{
				GUIFadeOut();
			}
		} // End of screen touches emulation
	}// End of OnGUI()
	
	// Self fade-in
	void GUIFadeIn()
	{
		float fadeInProgress = (Time.time - fadeStartTime) / fadeInDuration;
		fadeControlColor.a = Mathf.Lerp(0.0f, 1.0f, fadeInProgress);
		
		if (fadeInProgress >= 1.0)
		{
			runFadeIn = false;
		}
	}
	
	// Self fade-out
	void GUIFadeOut()
	{
		float fadeOutProgress = (Time.time - fadeStartTime) / fadeOutDuration;
		fadeControlColor.a = Mathf.Lerp(1.0f, 0.0f, fadeOutProgress);
		
		if (fadeOutProgress >= 1.0f)
		{
			runFadeOut = false;
			guiVisible = false;
		}
	}
	
	void Update()
	{
		// If we are running in Windows Editor we update the joystick
		// in the OnGUI() function.
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			return;
		}
		
		// If the control is not active then do nothing.
		if (!guiActive)
		{
			return;
		}
		
		// For the case we have set in the device a resolution different from native the new resolution
		// data will not be available untill the window is created. Even it is not enough to call here 
		// SetJoystickTouchRec() only once. Only after a few calls to Update() the new resolution data will be
		// available.
		
		SetJoystickTouchRec();
		
		// Set the alpha for fade-in and fade-out
		// As the joystick textures are semi-transparent (alpha = 0.5) we set only the half of the fade color
		guiTex.color = fadeControlColor/2;
		fixedJoystickTex.color = fadeControlColor/2;
		
		int count = Input.touchCount;
		
		if (count == 0)
		{
			ResetJoystick();
		}
		else
		{
			firstTouchInTouchRec = false;
			for (int i = 0; i < count; i++)
			{
				Touch touch = Input.GetTouch(i);
				Vector2 yInvertedTouchPosition = new Vector2(touch.position.x, Screen.height - touch.position.y);
				
				// Process only the events in the touch rec and only the first touch
				if (touchRec.Contains(yInvertedTouchPosition) && !firstTouchInTouchRec)
				{
					firstTouchInTouchRec = true;
					// Grab the finger Id that triggers the joystick
					fingerId = touch.fingerId;
					
					// Grab the time where the touch began
					if (touch.phase == TouchPhase.Began)
					{
						if (!guiVisible)
						{
							timeBeganTouch = Time.time;
						}
						else
						{
							// The user touches the screen while the joystick is fade out. 
							// The fadeout is interrupted. 
							drag = false;
							timeBeganTouch = Time.time;
							runFadeOut = false;
							fadeControlColor.a = 0.0f;
							guiVisible = false;
							ResetJoystick();
						}
					}
					
					// The joystic fades-in only if the user holds the finger for a time greater than joysticHoldThreshold
					if (touch.phase == TouchPhase.Stationary)
					{
						if (!guiVisible)
						{
							if ((Time.time - timeBeganTouch) > joystickHoldThreshold ) 
							{
								// If the joystick is not visible at all then place the joystick
								// where the user holds the finger and start the fade in.
								fadeStartTime = Time.time;
								runFadeIn = true;
								// Place the GUI Texture object.
								Vector2 guiPos = touch.position;
								// Define the pixel inset rectangle
								guiTex.pixelInset = new Rect(guiPos.x - guiTexHalfWidth, guiPos.y - guiTexHalfHeight, guiTexWidth, guiTexHeight);
								// Set fixed joystick texture position
								fixedJoystickTex.pixelInset = new Rect(guiPos.x - fixedJoystickTextHalfWidth, guiPos.y - fixedJoystickTextHalfHeight, fixedJoystickTextWidth, fixedJoystickTextHeight);
								// Grab current GUI texture position by calling SetDefaultRect().
								SetGUITexRec();
								guiVisible = true;
								// Make sure the translation vector is null.
								jTrans = Vector2.zero;
								
								drag = true;
							}
						}
					}
					
					// Drag event processing here.
					if (guiVisible && touch.phase == TouchPhase.Moved)
					{
						drag = true;
						
					}
					
				} // end of if(touchRec.Contains(yInvertedTouchPosition) ...
				
				// The user releases the finger (in or out the touch rec) and it launches the fade out process.
				// Check if the released finger is the active one.
				if ((fingerId == touch.fingerId) && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) )
				{
					// If the joystick is visible then fade it out 
					if (guiVisible && drag)
					{
						drag = false;
						timeBeganTouch = VERY_HIGH_TIME;
						
						// If the fade out is running do nothing, just let it finish
						if (!runFadeOut)
						{
							fadeStartTime = Time.time;
							runFadeOut = true;
							// Make sure the translation vector is null
							jTrans = Vector2.zero;
						}
					}
					
					ResetJoystick();
				}
				
				// Drag event processing here. No matter we are not longer in the touch rec
				if (guiVisible && drag && (fingerId == touch.fingerId) && touch.phase == TouchPhase.Moved)
				{
					// Update the position of the joystick to match the new touch.
					Vector2 guiTrans = guiCenter - touch.position;
					
					jTrans = Vector2.ClampMagnitude(guiTrans, jTransClampValue);
					Vector2 guiPos = guiCenter - jTrans;
					guiTex.pixelInset = new Rect(guiPos.x - guiTexHalfWidth, guiPos.y - guiTexHalfHeight, guiTexWidth, guiTexHeight);
					
					// What is expected by a Joystick user is 
					jTrans /= jTransClampValue;
				}
			}// End of for
			
		}
		
		if (runFadeIn)
		{
			GUIFadeIn();
		}
		
		if (runFadeOut)
		{
			GUIFadeOut();
		}
	}// End of Update()
	
	// Store rec for the GUI Texture.
	void SetGUITexRec()
	{
		guiTexRect = guiTex.pixelInset;
		// Make sure the fixed texture is always behind the joystick texture
		gameObject.transform.position = new Vector3(0, 0, 1);
		fixedJoystickTex.transform.position = new Vector3(0, 0, 0);
		// Cache the center of the GUI Texture.
		guiCenter.x = guiTexRect.x + guiTexRect.width / 2;
		guiCenter.y = guiTexRect.y + guiTexRect.height / 2;
	}
}

