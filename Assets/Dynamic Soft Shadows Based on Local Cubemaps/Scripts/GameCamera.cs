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
 
/*
 * Game Camera script.
 * 
 * Class that controls the camera by means of joysticks when running on the device.
 * Right Joystick controls the camera orientation.
 * Left Joystick controls the camera motion.
 * 
 */

public class GameCamera : MonoBehaviour
{
	// Game objects
	public GameObject	leftJoystick;
	public GameObject	rightJoystick;
	 
	public float		moveSpeed;
	public float		rotateSpeed;
	public float		collisionRadius;

	float	rotX	= 0.0f;
	float	rotY	= 0.0f;
 
	bool ignoreAnimMarks;

	void Start()
	{
		// Initialize rotation angles
		Vector3 angles = transform.eulerAngles;
		rotX = angles.y;
		rotY = angles.x;
		
		if (rotX > 360.0f)
		{
			rotX -= 360.0f;
		}
		
		// Azimuth angle is + if the angle is in the range [270, 360]
		// Azimuth angle is - if the angle is in the range [0, 90]
		// In other words, positive in the fourth quadrant and negative in the first one.
		
		if (rotY >= 270.0f && rotY <= 360.0f)
		{
			rotY = 360.0f - rotY;
		}
		else if (rotY > 0.0f && rotY <= 90.0f)
		{
			rotY = - rotY;
		}		
	}

	public void Activate(bool val)
	{
		if (val)
		{
			Vector3 angles = transform.eulerAngles;
			rotX = angles.y;
			rotY= angles.x;

			if (rotX > 360.0f)
			{
				rotX -= 360.0f;
			}

			if (rotY >= 270.0f && rotY <= 360.0f)
			{
				rotY = 360.0f - rotY;
			}
			else if (rotY > 0.0f && rotY <= 90.0f)
			{
				rotY = - rotY;
			}

			gameObject.GetComponent<Camera>().enabled = true;
		}
		else
		{
			gameObject.GetComponent<Camera>().enabled = false;
		}
	}


	void Update()
	{
		// Camera motion with no collision.
		MoveCamera();
	}

	// Apply rotation and translation to the camera according to the joystick input.
	void MoveCamera()
	{
		// Camera orientation: Right Joystick
		Vector2 rightJoystickTrans = rightJoystick.GetComponent<Joystick>().jTrans;
		float rightJoystickSpeed = rightJoystickTrans.magnitude;
		
		rotX -= rightJoystickTrans.x * rightJoystickSpeed * rotateSpeed * Time.deltaTime;
		rotY -= rightJoystickTrans.y * rightJoystickSpeed * rotateSpeed * Time.deltaTime;
		rotY = Mathf.Clamp (rotY, -90.0f, 90.0f);
		
		// Apply rotation around up vector
		transform.localRotation = Quaternion.AngleAxis(rotX, Vector3.up);
		// Apply rotation around left vector
		transform.localRotation *= Quaternion.AngleAxis(rotY, Vector3.left);
		
		// Camera motion: Left Joystick
		Vector2 leftJoystickTrans = leftJoystick.GetComponent<Joystick>().jTrans;
		float leftJoystickSpeed = leftJoystickTrans.magnitude;
		// Move forward-backward
		transform.position -= transform.forward * moveSpeed * leftJoystickSpeed * leftJoystickTrans.y * Time.deltaTime;
		// Move left-right
		transform.position -= transform.right * moveSpeed * leftJoystickSpeed * leftJoystickTrans.x * Time.deltaTime;
	}
}