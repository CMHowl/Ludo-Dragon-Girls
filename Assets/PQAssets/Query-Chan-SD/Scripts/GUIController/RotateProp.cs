using UnityEngine;
using System.Collections;

public class RotateProp : MonoBehaviour {
	
	[SerializeField]
	private float rotateAng;
	private float timeNextRotation;

	// Use this for initialization
	void Start () {
		timeNextRotation = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeNextRotation < Time.time)
		{
			this.transform.Rotate(Vector3.down * rotateAng, Space.World);
			timeNextRotation = Time.time + Time.deltaTime;
		}
	}

}
