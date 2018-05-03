using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMode : MonoBehaviour {

    public GameObject autoObject;
    public GameObject autoText;
    public GameObject blocker;
    public bool visible=false;
    private bool blocked = false;
    private Vector3 autodlgPosition;
    public AndroidJavaClass jc;
    public AndroidJavaObject jo;
    public void setVisible()
    {
        if (blocked)//当前处于托管状态，则单点取消托管
        {
            disableAutoMode();
        }
        else
        {
            autoObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
            visible = true;
        }
    }

    public void setInvisible()
    {
        autoObject.GetComponent<RectTransform>().anchoredPosition = autodlgPosition;
        visible = false;
    }

    public void confirmInDialog()
    {
        setInvisible();
        enableAutoMode();
    }

    public void enableAutoMode()//开启托管
    {
        autoText.SetActive(true);
        blocker.SetActive(true);
        blocked = true;
        jo.Call("Player_switchToAI");
        
    }

    public void disableAutoMode()//取消托管
    {
       
        autoText.SetActive(false);
        blocker.SetActive(false);
        blocked = false;
        jo.Call("Player_switchToAI");
    }

    public void cancelInDialog()
    {
        setInvisible();
    }

    // Use this for initialization
    void Start () {
        autodlgPosition = autoObject.GetComponent<RectTransform>().anchoredPosition;
        autoText.SetActive(false);
        blocker.SetActive(false);
        //jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }
	
	// Update is called once per frame
	void Update () {
    }
}
