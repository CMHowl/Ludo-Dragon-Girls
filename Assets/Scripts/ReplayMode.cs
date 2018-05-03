using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayMode : MonoBehaviour {

    public Slider slider;
    public Text sliderText;
    private float srcTimeScale;
    private bool timeSwitch;
    public void pauseScene()
    {
        Time.timeScale = 0;
        print("暂停！");
    }

    public void continueScene()
    {
        Time.timeScale = srcTimeScale;
    }

    public void enablePause()//双向开关
    {
        print("场景开关");
        if(timeSwitch == false)
        {
            timeSwitch = true;
            pauseScene();
            return;
        }
        else
        {
            timeSwitch = false;
            continueScene();
        }
    }

    public float getProgressPercentage()//获得进度条百分比
    {
        return slider.value;
    }

    public void updatePercentage()//更新文本组件
    {
        float percentage = getProgressPercentage();
        percentage *= 100;
        percentage = Mathf.Round(percentage);
        sliderText.text = (percentage).ToString() + "%";
    }

	// Use this for initialization
	void Start () {
        srcTimeScale = Time.timeScale;
        timeSwitch = false;
	}
	
	// Update is called once per frame
	void Update () {
        updatePercentage();
	}
}
