using UnityEngine;
using System.Collections;

public class PanelPrefCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider otherObj) {

		int panelNumber = otherObj.gameObject.GetComponent<DetectPanel>().panelNumber;
		//Debug.Log ("panel number = " + panelNumber);
		GameObject.Find("Query-Chan-SD_Controller").GetComponent<ShowTimeController>().ChangePanelPref(panelNumber);
	}

}
