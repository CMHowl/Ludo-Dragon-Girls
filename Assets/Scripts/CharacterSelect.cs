using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {

    //操作对象
    public Camera mainCamera;
    public GameObject background;
    public GameObject red;
    public GameObject yellow;
    public GameObject bule;
    public GameObject green;
    public GameObject circleRed;
    public GameObject circleYellow;
    public GameObject circleBule;
    public GameObject circleGreen;
    public GameObject text1P;
    public GameObject text2P;
    public GameObject text3P;
    public GameObject text4P;
    public GameObject text1COM;
    public GameObject text2COM;
    public GameObject text3COM;

    public GameObject redUI;
    public GameObject yellowUI;
    public GameObject buleUI;
    public GameObject greenUI;

    public InputField nameRed;
    public InputField nameYellow;
    public InputField nameBule;
    public InputField nameGreen;

    public GameObject UIplane;

    //操作对象存放数组
    GameObject[] teams = new GameObject[4];
    GameObject[] circles = new GameObject[4];
    Vector3[] circlePositionsLocal = new Vector3[4];
    Color[] colors = new Color[4];
    int[] playerChose = new int[4];      //每个角色对应的玩家编号
    InputField[] nameList = new InputField[4];

    GameObject[] texts = new GameObject[7];
    bool[] isTextClicked = new bool[7];
    Vector3[] textsLocalPosition = new Vector3[7];

    //拖动用坐标参数
    Vector3 ScreenSpace;
    Vector3 offset;

    //状态判断
    bool isStarting;
    bool isFinsh;

    //动画时间
    float time = 0;


    int playerNum;                       //玩家人数
    int AINum;                           //AI人数

    int playerCode;                      //玩家编号

    ScreenPosition screenPosition = new ScreenPosition();                       //屏幕规格化辅助类

    public AndroidJavaClass jc;
    public AndroidJavaObject jo;

    // Use this for initialization
    void Start () {
        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        isStarting = false;
        isFinsh = false;

        //记录光圈原本位置
        circlePositionsLocal[0] = circleRed.transform.position;
        circlePositionsLocal[1] = circleYellow.transform.position;
        circlePositionsLocal[2] = circleBule.transform.position;
        circlePositionsLocal[3] = circleGreen.transform.position;

        //初始化背景和任务模型位置
        background.transform.position = new Vector3(mainCamera.transform.position.x, 
            mainCamera.transform.position.y - 5, mainCamera.transform.position.z + 5);

        red.transform.position = new Vector3(mainCamera.transform.position.x - 1.5f,
            mainCamera.transform.position.y - 2.2f, mainCamera.transform.position.z + 1.5f);
        yellow.transform.position = new Vector3(mainCamera.transform.position.x - 0.5f,
            mainCamera.transform.position.y - 2.2f, mainCamera.transform.position.z + 1.5f);
        bule.transform.position = new Vector3(mainCamera.transform.position.x + 0.5f,
            mainCamera.transform.position.y - 2.2f, mainCamera.transform.position.z + 1.5f);
        green.transform.position = new Vector3(mainCamera.transform.position.x + 1.5f,
            mainCamera.transform.position.y - 2.2f, mainCamera.transform.position.z + 1.5f);

        text1P.transform.position = new Vector3(mainCamera.transform.position.x - 1.2f,
            mainCamera.transform.position.y - 1, mainCamera.transform.position.z + 2f);
        text2P.transform.position = new Vector3(mainCamera.transform.position.x - 0.6f,
            mainCamera.transform.position.y - 1, mainCamera.transform.position.z + 2f);
        text1COM.transform.position = new Vector3(mainCamera.transform.position.x,
            mainCamera.transform.position.y - 1, mainCamera.transform.position.z + 2f);
        text2COM.transform.position = new Vector3(mainCamera.transform.position.x,
            mainCamera.transform.position.y - 1, mainCamera.transform.position.z + 2f);
        text3COM.transform.position = new Vector3(mainCamera.transform.position.x,
            mainCamera.transform.position.y - 1, mainCamera.transform.position.z + 2f);
        text3P.transform.position = new Vector3(mainCamera.transform.position.x + 0.6f,
            mainCamera.transform.position.y - 1, mainCamera.transform.position.z + 2f);
        text4P.transform.position = new Vector3(mainCamera.transform.position.x + 1.2f,
            mainCamera.transform.position.y - 1, mainCamera.transform.position.z + 2f);

        redUI.transform.position = Camera.main.WorldToScreenPoint(red.transform.position);
        yellowUI.transform.position = Camera.main.WorldToScreenPoint(yellow.transform.position);
        buleUI.transform.position = Camera.main.WorldToScreenPoint(bule.transform.position);
        greenUI.transform.position = Camera.main.WorldToScreenPoint(green.transform.position);


        //游戏对象放入数组
        teams[0] = red;
        teams[1] = yellow;
        teams[2] = bule;
        teams[3] = green;

        texts[0] = text1P;
        texts[1] = text2P;
        texts[2] = text3P;
        texts[3] = text4P;
        texts[4] = text1COM;
        texts[5] = text2COM;
        texts[6] = text3COM;

        circles[0] = circleRed;
        circles[1] = circleYellow;
        circles[2] = circleBule;
        circles[3] = circleGreen;

        colors[0] = Color.red;
        colors[1] = Color.yellow;
        colors[2] = new Color(0.3f, 0.9f, 1f);
        colors[3] = Color.green;

        nameList[0] = nameRed;
        nameList[1] = nameYellow;
        nameList[2] = nameBule;
        nameList[3] = nameGreen;

        for (int i = 0; i < 4; i++)
        {
            nameList[i].enabled = false;
        }

        //给Android发送场景已经加载完成的信息
        jo.Call("start_choose_room");

        playerCode = 2;

        //初始化标签位置数组
        textsLocalPosition[0] = text1P.transform.position;
        textsLocalPosition[1] = text2P.transform.position;
        textsLocalPosition[2] = text3P.transform.position;
        textsLocalPosition[3] = text4P.transform.position;
        textsLocalPosition[4] = text1COM.transform.position;
        textsLocalPosition[5] = text2COM.transform.position;
        textsLocalPosition[6] = text3COM.transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //按下响应
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Text")))
            {
                if (hit.collider.gameObject.name == "1P")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        //清空1P所选角色标志
                        if (playerChose[i] == 1)
                        {
                            playerChose[i] = 0;
                        }
                    }

                    //获取屏幕鼠标坐标
                    ScreenSpace = Camera.main.WorldToScreenPoint(texts[0].transform.position);
                    offset = texts[0].transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y, ScreenSpace.z));

                    //赋拖动标志
                    isTextClicked[0] = true;
                }

                if (hit.collider.gameObject.name == "2P")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (playerChose[i] == 2)
                        {
                            playerChose[i] = 0;
                        }
                    }

                    ScreenSpace = Camera.main.WorldToScreenPoint(texts[1].transform.position);
                    offset = texts[1].transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y, ScreenSpace.z));
                    isTextClicked[1] = true;
                }

                if (hit.collider.gameObject.name == "3P")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (playerChose[i] == 3)
                        {
                            playerChose[i] = 0;
                        }
                    }

                    ScreenSpace = Camera.main.WorldToScreenPoint(texts[2].transform.position);
                    offset = texts[2].transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y, ScreenSpace.z));
                    isTextClicked[2] = true;
                }

                if (hit.collider.gameObject.name == "4P")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (playerChose[i] == 4)
                        {
                            playerChose[i] = 0;
                        }
                    }

                    ScreenSpace = Camera.main.WorldToScreenPoint(texts[3].transform.position);
                    offset = texts[3].transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y, ScreenSpace.z));
                    isTextClicked[3] = true;
                }

                if (hit.collider.gameObject.name == "1COM")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (playerChose[i] == 5)
                        {
                            playerChose[i] = 0;
                        }
                    }

                    ScreenSpace = Camera.main.WorldToScreenPoint(texts[4].transform.position);
                    offset = texts[4].transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y, ScreenSpace.z));
                    isTextClicked[4] = true;
                }

                if (hit.collider.gameObject.name == "2COM")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (playerChose[i] == 6)
                        {
                            playerChose[i] = 0;
                        }
                    }

                    ScreenSpace = Camera.main.WorldToScreenPoint(texts[5].transform.position);
                    offset = texts[5].transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y, ScreenSpace.z));
                    isTextClicked[5] = true;
                }

                if (hit.collider.gameObject.name == "3COM")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (playerChose[i] == 7)
                        {
                            playerChose[i] = 0;
                        }
                    }

                    ScreenSpace = Camera.main.WorldToScreenPoint(texts[6].transform.position);
                    offset = texts[6].transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y, ScreenSpace.z));
                    isTextClicked[6] = true;
                }
            }
        }

        //拖动响应
        if (Input.GetMouseButton(0))
        {
            //获取鼠标移动距离
            Vector3 CurScreenPosition = new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, ScreenSpace.z);
            Vector3 CurPosition = Camera.main.ScreenToWorldPoint(CurScreenPosition) + offset;

            for(int i = 0; i < 7; i++)
            {
                if (isTextClicked[i])
                {
                    //标签跟随鼠标
                    texts[i].transform.position = CurPosition;

                    //松开响应
                    if (Input.GetMouseButtonUp(0))
                    {
                        //清空拖动标志，标签归位
                        isTextClicked[i] = false;
                        texts[i].transform.position = textsLocalPosition[i];
                        texts[i].GetComponent<Renderer>().material.color = new Color(0.6f, 0.6f, 0.6f, 1f);

                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("TeamSelect")))
                        {
                            //赋选择标志
                            if (hit.collider.gameObject.name == "RedSelect")
                            {
                                if (playerChose[0] == 0)
                                {
                                    playerChose[0] = i + 1;
                                    for (int j = 0; j < 4; j++)
                                    {
                                        if (j != 0 && playerChose[j] == i + 1)
                                        {
                                            playerChose[j] = 0;
                                        }
                                    }
                                }
                            }
                            if (hit.collider.gameObject.name == "YellowSelect")
                            {
                                if (playerChose[1] == 0)
                                {
                                    playerChose[1] = i + 1;
                                    for (int j = 0; j < 4; j++)
                                    {
                                        if (j != 1 && playerChose[j] == i + 1)
                                        {
                                            playerChose[j] = 0;
                                        }
                                    }
                                }
                            }
                            if (hit.collider.gameObject.name == "BuleSelect")
                            {
                                if (playerChose[2] == 0)
                                {
                                    playerChose[2] = i + 1;
                                    for (int j = 0; j < 4; j++)
                                    {
                                        if (j != 2 && playerChose[j] == i + 1)
                                        {
                                            playerChose[j] = 0;
                                        }
                                    }
                                }

                            }
                            if (hit.collider.gameObject.name == "GreenSelect")
                            {
                                if (playerChose[3] == 0)
                                {
                                    playerChose[3] = i + 1;
                                    for (int j = 0; j < 4; j++)
                                    {
                                        if (j != 3 && playerChose[j] == i + 1)
                                        {
                                            playerChose[j] = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < 7; i++)
            {
                if (isTextClicked[i])
                {
                    isTextClicked[i] = false;
                    texts[i].transform.position = textsLocalPosition[i];
                    texts[i].GetComponent<Renderer>().material.color = new Color(0.6f, 0.6f, 0.6f, 1f);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("TeamSelect")))
                    {
                        if (hit.collider.gameObject.name == "RedSelect")
                        {
                            if (playerChose[0] == 0)
                            {
                                playerChose[0] = i + 1;
                                for (int j = 0; j < 4; j++)
                                {
                                    if (j != 0 && playerChose[j] == i + 1)
                                    {
                                        playerChose[j] = 0;
                                    }
                                }
                            }
                        }
                        if (hit.collider.gameObject.name == "YellowSelect")
                        {
                            if (playerChose[1] == 0)
                            {
                                playerChose[1] = i + 1;
                                for (int j = 0; j < 4; j++)
                                {
                                    if (j != 1 && playerChose[j] == i + 1)
                                    {
                                        playerChose[j] = 0;
                                    }
                                }
                            }
                        }
                        if (hit.collider.gameObject.name == "BuleSelect")
                        {
                            if (playerChose[2] == 0)
                            {
                                playerChose[2] = i + 1;
                                for (int j = 0; j < 4; j++)
                                {
                                    if (j != 2 && playerChose[j] == i + 1)
                                    {
                                        playerChose[j] = 0;
                                    }
                                }
                            }

                        }
                        if (hit.collider.gameObject.name == "GreenSelect")
                        {
                            if (playerChose[3] == 0)
                            {
                                playerChose[3] = i + 1;
                                for (int j = 0; j < 4; j++)
                                {
                                    if (j != 3 && playerChose[j] == i + 1)
                                    {
                                        playerChose[j] = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        for (int i = 0; i < 4; i++)
        {
            //刷新光圈和玩家选择
            if (!isFinsh)
            {
                circles[i].transform.position = circlePositionsLocal[i];

                if (playerChose[i] > 0)
                {
                    circles[i].transform.position = teams[i].transform.position;

                    //两边标签位置偏低，中间偏高
                    if (i == 0 || i == 3)
                        texts[playerChose[i] - 1].transform.position = teams[i].transform.position + new Vector3(0, 1.1f, 0);
                    else
                        texts[playerChose[i] - 1].transform.position = teams[i].transform.position + new Vector3(0, 1.2f, 0);

                    texts[playerChose[i] - 1].GetComponent<Renderer>().material.color = colors[i];

                    if (playerChose[i] > 4)
                    {
                        nameList[i].text = "COM";
                    }

                    nameList[i].enabled = true;
                }

                else
                {
                    nameList[i].enabled = false;
                }
            }
        }



        if (isStarting)
        {
            //返回创建房间信息
            //1,2,3,4是玩家；5,6,7是人机；0是未选中
            print(" " + playerChose[0] + " " + playerChose[1] + " " + playerChose[2] + " " + playerChose[3]);
            //从左到右的玩家名
            print(" " + nameList[0].text + " " + nameList[1].text + " " + nameList[2].text + " " + nameList[3].text);
            //使用string数组保存每个玩家的名字
            string[] nameList_string = new string[4];
            for(int i = 0; i < 4; i++)
            {
                nameList_string[i] = nameList[i].text;
            }
            //传输玩家类型和玩家名字
            jo.Call("get_init_data", playerChose, nameList_string);

            time += Time.fixedDeltaTime;

            //销毁文字光圈
            for (int i = 0; i < 4; i++)
            {
                Destroy(circles[i]);
            }

            for (int i = 0; i < 7; i++)
            {
                Destroy(texts[i]);
            }

            Destroy(UIplane);

            if (time > 0 && time < 0.1f)
            {
                red.transform.localScale -= new Vector3(0, 0.04f, 0);
                yellow.transform.localScale -= new Vector3(0, 0.04f, 0);
                bule.transform.localScale -= new Vector3(0, 0.04f, 0);
                green.transform.localScale -= new Vector3(0, 0.04f, 0);
            }

            if(time > 0.1f && time < 0.2f)
            {
                red.transform.localScale += new Vector3(0, 0.04f, 0);
                yellow.transform.localScale += new Vector3(0, 0.04f, 0);
                bule.transform.localScale += new Vector3(0, 0.04f, 0);
                green.transform.localScale += new Vector3(0, 0.04f, 0);
            }

            if (time > 0.2f && time < 2f)
            {
                red.transform.Translate(Vector3.up * 0.5f);
                yellow.transform.Translate(Vector3.up * 0.5f);
                bule.transform.Translate(Vector3.up * 0.5f);
                green.transform.Translate(Vector3.up * 0.5f);

                background.transform.localScale = background.transform.localScale - new Vector3(3f, 0, 3f);

                //背景消失后，销毁人物背景
                if (background.transform.localScale.x < 2)
                {
                    Destroy(background);
                    Destroy(red);
                    Destroy(yellow);
                    Destroy(bule);
                    Destroy(green);
                    background.transform.localScale = new Vector3(0, 0, 0);
                    isStarting = false;

                    SceneManager.LoadScene(2);
                }
            }
        }
    }

    //响应开始游戏
    public void GameStart()
    {
        isStarting = true;
        isFinsh = true;
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    //设置玩家AI人数
    //刷新界面
    public void Set_Num(string num_string)
    {
        string[] temp = num_string.Split(' ');
        playerNum = int.Parse(temp[0]);
        AINum = int.Parse(temp[1]);

        for (int i = playerNum; i < 4; i++)
        {
            texts[i].SetActive(false);
        }

        for (int i = 4 + AINum; i < 7; i++)
        {
            texts[i].SetActive(false);
        }
    }
}
