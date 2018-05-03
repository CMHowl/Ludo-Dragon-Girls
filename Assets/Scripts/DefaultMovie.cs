using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMovie : MonoBehaviour {

    public void displayMovie()
    {
        Handheld.PlayFullScreenMovie("VideoHigh 2.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        print("已调用");
    }

	// Use this for initialization
	void Start () {
        displayMovie();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
