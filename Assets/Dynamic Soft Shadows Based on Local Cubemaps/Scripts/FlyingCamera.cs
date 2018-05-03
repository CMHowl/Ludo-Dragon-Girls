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
 * Flying Camera script.
 * 
 * This class controls the main camera when the application is running
 * in the Editor mode. Camera motion is controlled with the keyboard arrow
 * keys and Q,W,A,S,Z,D keys.
 * 
 */

public class FlyingCamera : MonoBehaviour
{
	public GameObject moveChessPiece;

	public float 	moveSpeed 		= 12.0f;
	public float 	sensitivity 	= 80.0f;
	public float 	up_downSpeed 	= 5.0f;
	public float 	slowFactor 		= 0.50f;
	public float 	fastFactor 		= 2.5f;

	private float 	rotX	= 0.0f;
	private float 	rotY 	= 0.0f;
	
	void Start()
	{
		// Initialize rotation angles
		Vector3 angles = transform.eulerAngles;
		rotX = angles.y;
		rotY= angles.x;
				
		if (rotY >= 270.0f)
		{
			rotY = 360.0f - rotY;
		}
		else if (rotY > 0.0f && rotY <= 90.0f)
		{
			rotY = - rotY;
		}
	}
	
	void Update()
	{
		if (moveChessPiece.GetComponent<MoveChessPiece>().GetHitGO() == null)
		{
			// Camera orientation
			if (Input.GetMouseButton (0))
			{
				rotX += Input.GetAxis ("Mouse X") * sensitivity * Time.deltaTime;
				rotY += Input.GetAxis ("Mouse Y") * sensitivity * Time.deltaTime;
				rotY = Mathf.Clamp (rotY, -90, 90);
				
				transform.localRotation = Quaternion.AngleAxis (rotX, Vector3.up);
				transform.localRotation *= Quaternion.AngleAxis (rotY, Vector3.left);
			}
			
			// Move forward-backward and right-left
			if (Input.GetMouseButton (0) && Input.GetKey (KeyCode.LeftShift))
			{
				transform.position += transform.forward * moveSpeed * fastFactor * Input.GetAxis ("Vertical") * Time.deltaTime;
				transform.position += transform.right * moveSpeed * fastFactor * Input.GetAxis ("Horizontal") * Time.deltaTime;
			}
			else if (Input.GetMouseButton (0) && Input.GetKey (KeyCode.LeftControl))
			{
				transform.position += transform.forward * moveSpeed * slowFactor * Input.GetAxis ("Vertical") * Time.deltaTime;
				transform.position += transform.right * moveSpeed * slowFactor * Input.GetAxis ("Horizontal") * Time.deltaTime;
			}
			else
			{
				if (Input.GetMouseButton (0))
				{
					transform.position += transform.forward * moveSpeed * Input.GetAxis ("Vertical") * Time.deltaTime;
					transform.position += transform.right * moveSpeed * Input.GetAxis ("Horizontal") * Time.deltaTime;
				}
			}
			
			// Camera up-down controls
			if (Input.GetMouseButton (0) && Input.GetKey (KeyCode.Q))
			{
				transform.position += transform.up * up_downSpeed * Time.deltaTime;
			}
			
			if (Input.GetMouseButton (0) && Input.GetKey (KeyCode.Z))
			{
				transform.position -= transform.up * up_downSpeed * Time.deltaTime;
			}
		}
	}
}
