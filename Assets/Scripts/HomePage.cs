using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePage : MonoBehaviour {

    //操作对象
    public GameObject background;
    public GameObject model;
    public GameObject red;
    public GameObject yellow;
    public GameObject bule;
    public GameObject green;
    public GameObject title;
    public GameObject singleModePanel;
    public GameObject createRoomPlane;
    public GameObject roomListPlane;
    public GameObject getPlayerNamePlane;
    public GameObject playerNumGet;
    public GameObject AINumGet;

    //重力曲线
    public AnimationCurve curveUp;
    public AnimationCurve curveDown;

    //按钮对象
    public Button startButton;
    public Button singleButton;
    public Button netButton;
    public Button goHomeButton;
    public Button createRoomButton;
    public Button joinRoomButton;
    public Button backButton;
    public Button onePlayersButton;
    public Button twoPlayersButton;
    public Button threePlayersButton;
    public Button fourPlayersButton;
    public Button oneAIButton;
    public Button twoAIButton;
    public Button threeAIButton;
    public Button fourAIButton;
    public Button createButton;

    //房间名,玩家名
    public InputField roomNameField;
    public InputField playerNameField;

    string roomName;
    string playerName;

    float time;

    //透明度
    float[] alphaList = new float[2];

    int state;
    //state=-2:获取玩家名   state=-1:获得玩家名   state=0:首次进入游戏状态
    //state=1:主界面   state=2:主界面->游戏模式   
    //state=3:游戏模式->单人模式       state=4:单人模式->游戏模式
    //state=5:游戏模式->多人模式       state=6:多人模式->创建房间 
    //state=7:创建房间->人数设置       state=8:人数设置->创建房间 
    //state=9:创建房间->多人模式       state=10:多人模式->加入房间       
    //state=11:加入房间->多人模式       state=12:多人模式->游戏模式       state=13:游戏模式->主界面

    //玩家人数
    int playerNum;
    int AINum;

    bool isNetMode;

    ScreenPosition screenPosition = new ScreenPosition();                       //屏幕规格化辅助类

    public AndroidJavaClass jc;
    public AndroidJavaObject jo;

    // Use this for initialization
    void Start () {
        getPlayerNamePlane.SetActive(false);

        //预保存存储并获取界面状态和玩家名
        state = PlayerPrefs.GetInt("State");
        if (PlayerPrefs.GetString("PlayerName") == "")
        {
            state = 0;
        }

        if (state == 0)
        {
            state = -2;
            PlayerPrefs.SetInt("State", 1);
        }

        playerName = PlayerPrefs.GetString("PlayerName");

        //初始化模型标题位置
        red.transform.localScale = new Vector3(0, 0, 0);
        yellow.transform.localScale = new Vector3(0, 0, 0);
        bule.transform.localScale = new Vector3(0, 0, 0);
        green.transform.localScale = new Vector3(0, 0, 0);
        title.transform.position = new Vector3(0, 15, -9);

        //设置按钮透明
        for (int i = 0; i < 2; i++)
        {
            alphaList[i] = 0;
        }

        startButton.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, alphaList[0]);
        singleButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f, alphaList[1]);
        netButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f, alphaList[1]);
        goHomeButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f, alphaList[1]);

        //隐藏按钮
        singleButton.gameObject.SetActive(false);
        netButton.gameObject.SetActive(false);
        goHomeButton.gameObject.SetActive(false);

        createRoomButton.gameObject.SetActive(false);
        joinRoomButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);

        //默认人数为4
        playerNum = 4;
        AINum = 0;

        AINumGet.SetActive(false);
        oneAIButton.gameObject.SetActive(false);
        twoAIButton.gameObject.SetActive(false);
        threeAIButton.gameObject.SetActive(false);
        fourAIButton.gameObject.SetActive(false);

        isNetMode = false;

        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private void FixedUpdate()
    {
        //弹出用户名输入框
        if (state == -2)
        {
            time += Time.fixedDeltaTime;
            if (time > 0.5 && time < 1.0f)
            {
                getPlayerNamePlane.SetActive(true);
                getPlayerNamePlane.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), curveUp.Evaluate((time - 0.5f) / 0.5f));
            }
        }

        //收回用户名输入框
        if (state == -1)
        {
            time += Time.fixedDeltaTime;
            if (time > 0 && time < 0.3f)
            {
                getPlayerNamePlane.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 0, 0), curveUp.Evaluate(time / 0.3f));
            }

            if (time > 0.3f && time < 0.4f)
            {
                getPlayerNamePlane.SetActive(false);
                state = 1;
                time = 0;
            }
        }

        if (state == 1)
        {
            time += Time.fixedDeltaTime;

            //标题动画
            if (time > 0 && time < 0.8f)
                title.transform.position = Vector3.Lerp(new Vector3(0, 10, -9), new Vector3(0, -0.1f, -9), curveDown.Evaluate(time / 0.8f));

            if (time > 0.8f && time < 0.9f)
                title.transform.position = Vector3.Lerp(new Vector3(0, -0.1f, -9), new Vector3(0, 1f, -9), curveUp.Evaluate((time - 0.8f) / 0.1f));

            if (time > 0.9f && time < 1.0f)
                title.transform.position = Vector3.Lerp(new Vector3(0, 1f, -9), new Vector3(0, -0.1f, -9), curveDown.Evaluate((time - 0.9f) / 0.1f));

            //人物放大
            if (time > 0.8f && time < 1.3f)
                yellow.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);

            if (time > 0.9f && time < 1.4f)
                green.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);

            if (time > 1.0f && time < 1.5f)
                bule.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);

            if (time > 1.1f && time < 1.6f)
                red.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);

            //人物缩小
            if (time > 1.3f && time < 1.4f)
                yellow.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);

            if (time > 1.4f && time < 1.5f)
                green.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);

            if (time > 1.5f && time < 1.6f)
                bule.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);

            if (time > 1.6f && time < 1.7f)
                red.transform.localScale -= new Vector3(0.4f, 0.4f, 0.4f);

            //人物放大
            if (time > 1.4 && time < 1.5f)
                yellow.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);

            if (time > 1.5f && time < 1.6f)
                green.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);

            if (time > 1.6f && time < 1.7f)
                bule.transform.localScale += new Vector3(0.15f, 0.15f, 0.15f);

            if (time > 1.7f && time < 1.8f)
                red.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);

            //开始按钮透明度变化
            if (time > 1.7f && time < 2.5f)
            {
                alphaList[0] += 0.02f;
                startButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f, alphaList[0]);
            }
        }

        if (state == 2)
        {
            time += Time.fixedDeltaTime;

            //图像移位
            if (time > 0f && time < 0.5f)
            {
                model.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(0, 0, 0)),
                    screenPosition.GetScreenPosition(new Vector3(-3, -1, 0)), time / 0.5f);
                model.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0.7f, 0.7f, 0.7f), time / 0.5f);
            }

            //按钮动画
            if (time > 0.5f && time < 1.5f)
            {
                singleButton.gameObject.SetActive(true);
                netButton.gameObject.SetActive(true);
                goHomeButton.gameObject.SetActive(true);

                alphaList[1] += 0.02f;
                singleButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f, alphaList[1]);
                netButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f, alphaList[1]);
                goHomeButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f, alphaList[1]);
            }
        }

        if (state == 3)
        {
            time += Time.fixedDeltaTime;

            //面板出现
            if (time > 0f && time < 0.5f)
            {
                singleModePanel.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(1500, 500, 0)),
                    screenPosition.GetScreenPosition(new Vector3(700, 500, 0)), curveUp.Evaluate(time / 0.5f));
            }

            if (time > 0.5f && time < 0.6f)
            {
                singleModePanel.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(700, 500, 0)),
                    screenPosition.GetScreenPosition(new Vector3(750, 500, 0)), curveDown.Evaluate((time - 0.5f) / 0.1f));
            }

            //按钮消失
            if (time > 0.1f && time < 0.3f)
            {
                singleButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 1, 1), (time - 0.1f) / 0.2f);
                netButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 1, 1), (time - 0.1f) / 0.2f);
                goHomeButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 1, 1), (time - 0.1f) / 0.2f);
            }

            if (time > 0.4f && time < 0.5f)
            {
                singleButton.gameObject.SetActive(false);
                netButton.gameObject.SetActive(false);
                goHomeButton.gameObject.SetActive(false);
            }
        }

        if (state == 4)
        {
            time += Time.fixedDeltaTime;

            //面板离开
            if (time > 0f && time < 0.5f)
            {
                singleModePanel.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(700, 500, 0)),
                    screenPosition.GetScreenPosition(new Vector3(1500, 500, 0)), curveUp.Evaluate(time / 0.5f));
            }

            //按钮出现
            if (time > 0.0f && time < 0.2f)
            {
                singleButton.transform.localScale = Vector3.Lerp(new Vector3(0, 1, 1), new Vector3(1, 1, 1), time / 0.2f);
                netButton.transform.localScale = Vector3.Lerp(new Vector3(0, 1, 1), new Vector3(1, 1, 1), time / 0.2f);
                goHomeButton.transform.localScale = Vector3.Lerp(new Vector3(0, 1, 1), new Vector3(1, 1, 1), time / 0.2f);
            }
        }

        if (state == 5)
        {
            time += Time.fixedDeltaTime;

            //按钮消失
            if (time > 0f && time < 0.2f)
            {
                singleButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1, 0, 1), time / 0.2f);
                netButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1, 0, 1), time / 0.2f);
                goHomeButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1, 0, 1), time / 0.2f);
            }

            //按钮出现
            if (time > 0.2f && time < 0.4f)
            {
                singleButton.gameObject.SetActive(false);
                netButton.gameObject.SetActive(false);
                goHomeButton.gameObject.SetActive(false);

                createRoomButton.gameObject.SetActive(true);
                joinRoomButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(true);

                createRoomButton.transform.localScale = Vector3.Lerp(new Vector3(1, 0, 1), new Vector3(1, 1, 1), (time - 0.2f) / 0.2f);
                joinRoomButton.transform.localScale = Vector3.Lerp(new Vector3(1, 0, 1), new Vector3(1, 1, 1), (time - 0.2f) / 0.2f);
                backButton.transform.localScale = Vector3.Lerp(new Vector3(1, 0, 1), new Vector3(1, 1, 1), (time - 0.2f) / 0.2f);
            }
        }

        if (state == 6 || state == 10)
        {
            time += Time.fixedDeltaTime;

            //面板出现
            if (time > 0f && time < 0.5f)
            {
                if (state == 6)
                    createRoomPlane.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(1500, 500, 0)),
                    screenPosition.GetScreenPosition(new Vector3(700, 500, 0)), curveUp.Evaluate(time / 0.5f));
                if (state == 10)
                    roomListPlane.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(1500, 500, 0)),
                    screenPosition.GetScreenPosition(new Vector3(700, 500, 0)), curveUp.Evaluate(time / 0.5f));
            }

            if (time > 0.5f && time < 0.6f)
            {
                if (state == 6)
                    createRoomPlane.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(700, 500, 0)),
                    screenPosition.GetScreenPosition(new Vector3(750, 500, 0)), curveDown.Evaluate((time - 0.5f) / 0.1f));
                if (state == 10)
                    roomListPlane.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(700, 500, 0)),
                    screenPosition.GetScreenPosition(new Vector3(750, 500, 0)), curveDown.Evaluate((time - 0.5f) / 0.1f));
            }

            //按钮消失
            if (time > 0.1f && time < 0.3f)
            {
                createRoomButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 1, 1), (time - 0.1f) / 0.2f);
                joinRoomButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 1, 1), (time - 0.1f) / 0.2f);
                backButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0, 1, 1), (time - 0.1f) / 0.2f);
            }

            if (time > 0.4f && time < 0.5f)
            {
                createRoomButton.gameObject.SetActive(false);
                joinRoomButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);
            }
        }

        if (state == 7)
        {
            time += Time.fixedDeltaTime;

            if (time > 0f && time < 0.2f)
            {
                singleModePanel.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(750, 500, 0)),
                    screenPosition.GetScreenPosition(new Vector3(800, 450, 0)), curveUp.Evaluate(time / 0.2f));
            }
        }

        if (state == 8)
        {
            time += Time.fixedDeltaTime;

            if (time > 0f && time < 0.2f)
            {
                singleModePanel.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(800, 450, 0)),
                    screenPosition.GetScreenPosition(new Vector3(750, 500, 0)), curveUp.Evaluate(time / 0.2f));
            }

            if (time > 0.15f && time < 0.2f)
            {
                singleModePanel.transform.position = screenPosition.GetScreenPosition(new Vector3(1500, 500, 0));
            }
        }

        if (state == 9 || state == 11)
        {
            time += Time.fixedDeltaTime;

            //面板离开
            if (time > 0f && time < 0.5f)
            {
                if (state == 9)
                    createRoomPlane.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(750, 500, 0)),
                        screenPosition.GetScreenPosition(new Vector3(1500, 500, 0)), curveUp.Evaluate(time / 0.5f));
                if (state == 11)
                    roomListPlane.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(750, 500, 0)),
                        screenPosition.GetScreenPosition(new Vector3(1500, 500, 0)), curveUp.Evaluate(time / 0.5f));
            }

            //按钮出现
            if (time > 0.0f && time < 0.2f)
            {
                createRoomButton.transform.localScale = Vector3.Lerp(new Vector3(0, 1, 1), new Vector3(1, 1, 1), time / 0.2f);
                joinRoomButton.transform.localScale = Vector3.Lerp(new Vector3(0, 1, 1), new Vector3(1, 1, 1), time / 0.2f);
                backButton.transform.localScale = Vector3.Lerp(new Vector3(0, 1, 1), new Vector3(1, 1, 1), time / 0.2f);
            }
        }

        if (state == 12)
        {
            time += Time.fixedDeltaTime;

            //按钮消失
            if (time > 0f && time < 0.2f)
            {
                createRoomButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1, 0, 1), time / 0.2f);
                joinRoomButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1, 0, 1), time / 0.2f);
                backButton.transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1, 0, 1), time / 0.2f);
            }

            //按钮出现
            if (time > 0.2f && time < 0.4f)
            {
                createRoomButton.gameObject.SetActive(false);
                joinRoomButton.gameObject.SetActive(false);
                backButton.gameObject.SetActive(false);

                singleButton.gameObject.SetActive(true);
                netButton.gameObject.SetActive(true);
                goHomeButton.gameObject.SetActive(true);

                singleButton.transform.localScale = Vector3.Lerp(new Vector3(1, 0, 1), new Vector3(1, 1, 1), (time - 0.2f) / 0.2f);
                netButton.transform.localScale = Vector3.Lerp(new Vector3(1, 0, 1), new Vector3(1, 1, 1), (time - 0.2f) / 0.2f);
                goHomeButton.transform.localScale = Vector3.Lerp(new Vector3(1, 0, 1), new Vector3(1, 1, 1), (time - 0.2f) / 0.2f);
            }
        }

        if (state == 13)
        {
            time += Time.fixedDeltaTime;

            //图像归位
            if (time > 0f && time < 0.5f)
            {
                model.transform.position = Vector3.Lerp(screenPosition.GetScreenPosition(new Vector3(-3, -1, 0)),
                    screenPosition.GetScreenPosition(new Vector3(0, 0, 0)), time / 0.5f);
                model.transform.localScale = Vector3.Lerp(new Vector3(0.7f, 0.7f, 0.7f), new Vector3(1, 1, 1), time / 0.5f);
            }

            //按钮动画
            if (time > 0.5f && time < 2.0f)
            {
                startButton.gameObject.SetActive(true);
                alphaList[0] += 0.02f;
                startButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f, alphaList[0]);
            }

            if (time < 1.0f)
            {
                //动画暂停
                yellow.GetComponent<Animator>().speed = 0;
                bule.GetComponent<Animator>().speed = 0;
                green.GetComponent<Animator>().speed = 0;
            }
        }


        if (state != 1 && state != 13)
        {
            //动画暂停
            yellow.GetComponent<Animator>().speed = 0;
            bule.GetComponent<Animator>().speed = 0;
            green.GetComponent<Animator>().speed = 0;
        }
    }

    //state=-1:获得玩家名
    public void GetPlayerName()
    {
        state = -1;
        time = 0;
        playerName = playerNameField.text;
        PlayerPrefs.SetString("PlayerName", playerName);
    }

    //state=2:主界面->游戏模式    
    public void GameModeSelect()
    {
        state = 2;
        time = 0;
        alphaList[0] = 0;
        startButton.gameObject.SetActive(false);
    }

    //state=3:游戏模式->单人模式
    public void SingleMode()
    {
        state = 3;
        time = 0;
    }

    //state=4:单人模式->游戏模式
    //state=8:人数选择->创建房间
    public void BackToGameModeSelectFromSingle()
    {
        if (!isNetMode)
        {
            state = 4;
            time = 0;
            singleButton.gameObject.SetActive(true);
            netButton.gameObject.SetActive(true);
            goHomeButton.gameObject.SetActive(true);
        }
        else
        {
            state = 8;
            time = 0;
        }
    }

    //state=5:游戏模式->多人模式
    public void RoomSelect()
    {
        state = 5;
        time = 0;
        isNetMode = true;
    }

    //state=6:多人模式->创建房间 
    public void CreateRoom()
    {
        state = 6;
        time = 0;
    }

    //state=7:创建房间->人数选择
    public void NumSelect()
    {
        state = 7;
        time = 0;
    }

    //state=9:创建房间->多人模式
    public void BackToRoomSelectFromCreate()
    {
        state = 9;
        time = 0;
        createRoomButton.gameObject.SetActive(true);
        joinRoomButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    //state=10:多人模式->加入房间
    public void JoinRoom()
    {
        state = 10;
        time = 0;
    }

    //state=11:加入房间->多人模式
    public void BackToRoomSelectFromJoin()
    {
        state = 11;
        time = 0;
        createRoomButton.gameObject.SetActive(true);
        joinRoomButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    //state=12:多人模式->游戏模式
    public void BackToGameModeSelectFromNet()
    {
        state = 12;
        time = 0;
        isNetMode = false;
    }

    //state=13:游戏模式->主界面
    public void GoHome()
    {
        state = 13;
        time = 0;
        alphaList[1] = 0;
        singleButton.gameObject.SetActive(false);
        netButton.gameObject.SetActive(false);
        goHomeButton.gameObject.SetActive(false);
    }

    //1个玩家
    public void setPlayerNum(int num)
    {
        playerNum = num;
        AINum = 0;

        switch (num)
        {
            case 1:
                playerNumGet.transform.position = onePlayersButton.transform.position;
                oneAIButton.gameObject.SetActive(true);
                twoAIButton.gameObject.SetActive(true);
                threeAIButton.gameObject.SetActive(true);
                fourAIButton.gameObject.SetActive(false);
                break;
            case 2:
                playerNumGet.transform.position = twoPlayersButton.transform.position;
                oneAIButton.gameObject.SetActive(true);
                twoAIButton.gameObject.SetActive(true);
                threeAIButton.gameObject.SetActive(false);
                fourAIButton.gameObject.SetActive(false);
                break;
            case 3:
                playerNumGet.transform.position = threePlayersButton.transform.position;
                oneAIButton.gameObject.SetActive(true);
                twoAIButton.gameObject.SetActive(false);
                threeAIButton.gameObject.SetActive(false);
                fourAIButton.gameObject.SetActive(false);
                break;
            case 4:
                playerNumGet.transform.position = fourPlayersButton.transform.position;
                oneAIButton.gameObject.SetActive(false);
                twoAIButton.gameObject.SetActive(false);
                threeAIButton.gameObject.SetActive(false);
                fourAIButton.gameObject.SetActive(false);
                break;
        }
        AINumGet.SetActive(false);
    }

    //选择AI人数
    public void setAINum(int num)
    {
        AINum = num;
        AINumGet.SetActive(true);
        switch (num)
        {
            case 1:
                AINumGet.transform.position = oneAIButton.transform.position;
                break;
            case 2:
                AINumGet.transform.position = twoAIButton.transform.position;
                break;
            case 3:
                AINumGet.transform.position = threeAIButton.transform.position;
                break;
            case 4:
                AINumGet.transform.position = fourAIButton.transform.position;
                break;
        }
    }

    //单人模式
    public void SingleModeStart()
    {
        print("玩家数：" + playerNum + "AI数：" + AINum);
        //向Android发送玩家数和AI数
        jo.Call("set_Player_AI_num", playerNum, AINum);
        SceneManager.LoadScene(1);
    }

    //创建房间
    public void CreatedRoom()
    {
        print("玩家数：" + playerNum + "AI数：" + AINum);
        SceneManager.LoadScene(1);
    }

    //加入房间
    public void JoinedRoom()
    {
        print("你加入了一个房间，房间名为：" + roomNameField.text + ",房间人数为：" + playerNum);
        SceneManager.LoadScene(1);
    }
}
