using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoScene : MonoBehaviour {

    public AudioListener listener;
    public Camera[] otherCamera;
    public AudioSource source;

    public void switchIntoMainScene()
    {
        gameObject.SetActive(false);
        listener.enabled = true;
        source.Play();
    }

    public void slowLoadCamera()
    {
        otherCamera[0].enabled = true;
        otherCamera[1].enabled = true;
    }

    // Use this for initialization
    void Start () {
        otherCamera[0].enabled = false;
        otherCamera[1].enabled = false;
        listener.enabled = false;
        Invoke("slowLoadCamera", 1f);
        Invoke("switchIntoMainScene", 3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
