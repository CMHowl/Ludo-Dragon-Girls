using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAnime : MonoBehaviour {

    void selfRotate()//自转
    {
        gameObject.transform.Rotate(new Vector3(0, 2f, 0));
    }

    public void becameVisible ()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
    }
    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Renderer>().enabled=false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (gameObject.GetComponent<Renderer>().enabled == true) selfRotate();
	}
}
