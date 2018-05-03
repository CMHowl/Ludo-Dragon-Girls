using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour {

    public bool isClicked;
    public Vector3 storyPosition;
    public GameObject storyDialog;

    public void enableStory()//控制按钮视觉效果
    {
        if (!isClicked)
        {
            storyDialog.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
            isClicked = true;
            return;
        }

        if (isClicked)
        {
            storyDialog.GetComponent<RectTransform>().anchoredPosition = storyPosition;
            gameObject.GetComponent<IconVisualEffect>().visual();
            isClicked = false;
            return;
        }
    }

    // Use this for initialization
    void Start()
    {
        isClicked = false;
        storyPosition = storyDialog.GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
