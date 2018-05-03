using UnityEngine;
using System.Collections;

public class FaceToCamera : MonoBehaviour {

	public int rotY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		this.transform.rotation = Quaternion.Euler(0, rotY, 0);

	}
}
