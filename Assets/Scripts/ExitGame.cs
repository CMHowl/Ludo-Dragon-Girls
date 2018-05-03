using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour {

    public GameObject exitObject;
    public bool visible = false;
    private Vector3 exitdlgPosition;
    public AndroidJavaClass jc;
    public AndroidJavaObject jo;

    public float time1;
    public void setVisible()
    {
        //exitObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
        visible = true;
        time1 = 1;
    }

    public void setInvisible()
    {
        exitObject.GetComponent<RectTransform>().anchoredPosition = exitdlgPosition;
        visible = false;
        time1 = 0;
    }

    public void confirmInDialog()
    {
        setInvisible();
        returnToLobby();
        //退出当前对局返回大厅！
    }

    private void returnToLobby()
    {
        //切换场景
        SceneManager.LoadScene(0);
        //中断游戏线程
        jo.Call("quit_game");

    }

    public void cancelInDialog()
    {
        setInvisible();
    }

    // Use this for initialization
    void Start()
    {
        exitdlgPosition = exitObject.GetComponent<RectTransform>().anchoredPosition;
        //jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }

    // Update is called once per frame
    void Update()
    {
        if (visible)
        {
            if(exitObject.GetComponent<RectTransform>().anchoredPosition.y>-100) exitObject.GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(exitObject.GetComponent<RectTransform>().anchoredPosition, new Vector3(0f, 0f, 0f), 500 * Time.deltaTime);
            else exitObject.GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(exitObject.GetComponent<RectTransform>().anchoredPosition, new Vector3(0f, 0f, 0f),2000 * Time.deltaTime);
        }
        time1 = Time.deltaTime;
    }
}
