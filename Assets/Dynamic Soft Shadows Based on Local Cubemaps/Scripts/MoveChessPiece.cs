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
 * Move Chess Piece script
 * 
 * This class implements moving the chess piece based on a touch event from the user.
 * It is implemented by mouse emulation in Editor Mode.
 * This script is attached to an empty MoveChessPiece game object.
 * 
 */
public class MoveChessPiece : MonoBehaviour
{
	int 		layerMask;
	GameObject	hitGO 		= null;
	float 		goPosY;

	Vector3		offset3D;
	Vector3		screenPoint3D;
	int			fingerId;

	// Use this for initialization
	void Start()
	{
		layerMask = 1 << LayerMask.NameToLayer("DynObjects");
	}

	// Mouse emulation of touch event when we are runing in Editor Mode
	void OnGUI()
	{
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			// The mouse event
			Event e = Event.current;
			
			if (e.isMouse)
			{
				Vector2 touchPos = new Vector2(e.mousePosition.x, Screen.height - e.mousePosition.y);

				if (e.type == EventType.MouseDown )
				{
					RaycastHit hit; 
					Vector3 touchPos3D = new Vector3(touchPos.x, touchPos.y, 0.0f); 
					Ray ray = Camera.main.ScreenPointToRay(touchPos3D);

					if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
					{ 
						// A GO in the layerMask has been hit
						hitGO = hit.collider.gameObject;

						screenPoint3D = Camera.main.WorldToScreenPoint(hitGO.transform.position);
						offset3D = hitGO.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, screenPoint3D.z)); 

						// Store GO Y axis coordinate
						goPosY = hitGO.transform.position.y;
					}
				}

				if (e.type == EventType.MouseUp )
				{
					// Reset hit GO
					hitGO = null;
				}
				
				if (e.type == EventType.MouseDrag && hitGO != null)
				{


					Vector3 curScreenPoint3D = new Vector3(touchPos.x, touchPos.y, screenPoint3D.z);
					Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint3D) + offset3D;
					// We don't want the object moving along Y axis
					curPosition.y = goPosY;

					KeepObjectInsideTheRoom(ref curPosition);

					// Update hit GO 3D position
					hitGO.transform.position = curPosition; 
				}
			}
		}
	}

	public GameObject GetHitGO()
	{
		return hitGO;
	}

	// In addition to collision detection, as we are not using physics
	// to move the object we avoid it passing through walls.
	void KeepObjectInsideTheRoom(ref Vector3 pos)
	{
		if(pos.x > 45)
			pos.x = 45;
		if(pos.x < -45)
			pos.x = -45;
		if(pos.z > 48)
			pos.z = 48;
		if(pos.z < -48)
			pos.z = -48;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{

		}
		else
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				Vector2 touchPos = touch.position;

				// Grab the beginning of the touch event
				if (touch.phase == TouchPhase.Began)
				{
					// The user can interact with the object only when it is in the central third of the screen width !!!
					if (touchPos.x < Screen.width/3 || touchPos.x > (Screen.width - Screen.width/3))
					{
						hitGO = null;
						return;
					}

					RaycastHit hit;
					fingerId = touch.fingerId;
					
					Vector3 touchPos3D = new Vector3(touchPos.x, touchPos.y, 0.0f); 
					Ray ray = Camera.main.ScreenPointToRay(touchPos3D);
					if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
					{ 
						// A GO in the layerMask has been hit						
						hitGO = hit.collider.gameObject;

						screenPoint3D = Camera.main.WorldToScreenPoint(hitGO.transform.position);
						offset3D = hitGO.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, screenPoint3D.z)); 
						goPosY = hitGO.transform.position.y;
					}
				}

				// The user releases the finger. 
				// Check if the released finger is the active one.
				if ((fingerId == touch.fingerId) && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
				{
					// Reset hit GO
					hitGO = null;
				}
				
				// If the user drags the finger or keep it touching in the same place
				if (hitGO != null && (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
				{ 		
					Vector3 curScreenPoint3D = new Vector3(touchPos.x, touchPos.y, screenPoint3D.z);
					Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint3D) + offset3D;
					// We don't want the object moving along Y axis.
					curPosition.y = goPosY;
					// Additionally to collision detection avoid obejct passing through walls as we are not using physics to move the obejct

					KeepObjectInsideTheRoom(ref curPosition);
					// Update hit GO 3D position.
					hitGO.transform.position = curPosition; 		
				}
			}
		}
	}
}
