using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour {

    public GameObject BGMusicManager;

    public void enableBGMusic()//自带两面效果，播放时点则停止，停止时点则播放
    {
        if (BGMusicManager.GetComponent<AudioSource>().enabled)
        {
            BGMusicManager.GetComponent<AudioSource>().enabled = false;
            return;
        }
        if (!BGMusicManager.GetComponent<AudioSource>().enabled)
        {
            BGMusicManager.GetComponent<AudioSource>().enabled = true;
            return;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
