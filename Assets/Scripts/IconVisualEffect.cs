using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconVisualEffect : MonoBehaviour {

    public bool isClicked;

    public void visual()//控制按钮视觉效果
    {
        if (!isClicked)
        {
            gameObject.GetComponent<RawImage>().CrossFadeColor(Color.gray, 0, false, true);
            isClicked = true;
            return;
        }

        if (isClicked)
        {
            gameObject.GetComponent<RawImage>().CrossFadeColor(Color.white, 0, false, true);
            isClicked = false;
            return;
        }
    }

	// Use this for initialization
	void Start () {
        isClicked = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
