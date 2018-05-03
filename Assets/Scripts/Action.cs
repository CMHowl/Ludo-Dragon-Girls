using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Action : MonoBehaviour
{

    public CharacterControl characterControl;
    public DiceGirlMove diceGirl;
    public FastAnimate fastDice;
    public GameObject myStar;
    public GameObject myChess;
    public static Queue<GameObject> fairyQueue;
    public static Queue<Command> commandQueue;
    public GameObject UIManager;
    public List<Vector3> circularInColor;
    public AndroidJavaClass jc;
    public AndroidJavaObject jo;
    public Vector3 diceButtonUIPosition;
    public GameObject[] canvasSeries;


    //结束标志
    bool isEnd = false;

    //启用与禁用动画标志
    bool enableAnimation = true;

    public class Command
    {
        public int playerid;
        public int chessid;
        public int action;
        public int step;
        public Command(int playerid, int chessid, int action, int step)
        {
            this.playerid = playerid;
            this.chessid = chessid;
            this.action = action;
            this.step = step;
        }
        public void setCommand(int playerid, int chessid, int action, int step)
        {
            this.playerid = playerid;
            this.chessid = chessid;
            this.action = action;
            this.step = step;
        }
    }

    public static Command preCommand;//上一条指令
    public int timetmp;
    internal static string getChessName(int playerid, int chessid)
    {
        //red 0;yellow 1;blue 2;green 3.
        int posCode = playerid * 4 + chessid;//人物号码！
        object tmp = posCode;
        string chessName = "Chess" + tmp.ToString();
        return chessName;
    }

    public void moveToStartLine(int playerid, int chessid)//起飞
    {
        characterControl.gotoTakeoffpos(playerid, chessid);//停机坪！
    }

    public void fallen(int playerid, int chessid)//打回基地
    {
        UnityEngine.Object tmpSong = Resources.Load<AudioClip>("Audios/Tornado");
        //UnityEngine.Object tmpSong = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Audios/Tornado.mp3", typeof(AudioClip));
        AudioSource music = GameObject.Find("SceneAudio").GetComponent<AudioSource>();
        music.clip = (AudioClip)Instantiate(tmpSong);
        music.Play();
        characterControl.gotoStartpos(GameObject.Find(getChessName(playerid, chessid)), playerid * 4 + chessid);
        myChess = GameObject.Find(getChessName(playerid, chessid));
        myChess.GetComponent<UnityChanControlScriptWithRgidBody>().startLine = true;//一定要还原startline属性！让棋子出门左转
    }

    public void hide(int playerid, int chessid)//消失
    {
        characterControl.invisibleChess(GameObject.Find(getChessName(playerid, chessid)));
    }

    public void finished(int playerid, int chessid)//完成
    {

        characterControl.gotoStartposSimple(GameObject.Find(getChessName(playerid, chessid)), playerid * 4 + chessid);
        myStar = GameObject.Find(getChessName(playerid, chessid));//先取棋子对象
        myStar.GetComponentInChildren<StarAnime>().becameVisible();//再对其子游戏对象提取StarAnime脚本
        characterControl.victoryPosVoice(playerid);
    }

    public void normalMove(int playerid, int chessid, int step)//基础移动
    {
        myChess = GameObject.Find(getChessName(playerid, chessid));
        myChess.GetComponent<UnityChanControlScriptWithRgidBody>().transCharacterLoop(step);
    }

    public void quickMove(int playerid, int chessid, int step)
    {
        myChess = GameObject.Find(getChessName(playerid, chessid));
        myChess.GetComponent<UnityChanControlScriptWithRgidBody>().QuickTransCharacterLoop(step);
    }

    public void reverse(int playerid, int chessid)
    {
        myChess = GameObject.Find(getChessName(playerid, chessid));
        rotate180();//完成旋转180
    }

    public void turnRight(int playerid, int chessid)
    {
        myChess = GameObject.Find(getChessName(playerid, chessid));
        myChess.GetComponent<UnityChanControlScriptWithRgidBody>().rotateCharacter_Right();
    }

    IEnumerator rotateRest90()
    {

        myChess.GetComponent<UnityChanControlScriptWithRgidBody>().rotateCharacter_Right();//先转90度
        yield return new WaitForSeconds(Time.fixedDeltaTime * 47);//等待一个旋转周期
        myChess.GetComponent<UnityChanControlScriptWithRgidBody>().rotateCharacter_Right();//再转剩余90度
    }

    void rotate180()
    {
        StartCoroutine(rotateRest90());//启动协程
    }

    public void executeAction(int playerid, int chessid, int action, int step)//执行动作
    {
        switch (action)
        {
            case 1:
                moveToStartLine(playerid, chessid);
                break;
            case 2:
                normalMove(playerid, chessid, step);
                break;
            case 3:
                hide(playerid, chessid);
                break;
            case 4:
                fallen(playerid, chessid);
                break;
            case 5:
                finished(playerid, chessid);
                break;
            case 6:
                reverse(playerid, chessid);
                break;
            case 7:
                turnRight(playerid, chessid);
                break;
            case 8:
                quickMove(playerid, chessid, step);
                break;
            case 9:
                activate(playerid, chessid);
                break;
            case 10:
                click(playerid, chessid);
                break;
            case 11:
                throwDiceByForce();
                break;
            case 12:
                turnLeft(playerid, chessid);
                break;
            case 13:
                roundStart(playerid);
                break;
            case 14:
                throwDiceByNum(step);
                break;
            case 15:
                showDiceButtonUI();
                break;
            case 16:
                roundEnd();
                break;
            case 17:
                hidechessUI(playerid);
                break;
            case 18:
                gameEnd();
                break;
            case 19:
                enableAllInteraction();
                break;
            case 20:
                hideDiceButtonUI();
                break;
            case 21:
                throwDiceByNum(step);
                break;
            case 22:
                fallenMomentary(playerid, chessid);
                break;
            case 23:
                moveToStartLineMomentary(playerid, chessid);
                break;
            case 24:
                teleport(playerid, chessid, step);
                break;
            case 25:
                configAnimation();
                break;
            case 26:
                ChangeUISeries(step);
                break;
            default:
                Debug.Log("无效命令！");
                break;
        }

    }

    private void ChangeUISeries(int step)//改变UI系列——1为主游戏UI，2为回放场景UI 
    {
        if (step == 1) {
            canvasSeries[0].SetActive(true);
            canvasSeries[1].SetActive(false);
            return;
        }
        if (step == 2)
        {
            canvasSeries[1].SetActive(true);
            canvasSeries[0].SetActive(false);
            return;
        }
    }

    public void configAnimation()//设置动画特效开关，双向
    {
        if (enableAnimation) { enableAnimation = false; return; }
        else enableAnimation = true;
    }

    private void teleport(int playerid, int chessid, int step)
    {
        myChess = GameObject.Find(getChessName(playerid, chessid));
        characterControl.teleportChess(myChess, step);
    }
    private void moveToStartLineMomentary(int playerid, int chessid)
    {
        characterControl.gotoTakeoffposSimple(playerid, chessid);
    }

    private void fallenMomentary(int playerid, int chessid)
    {
        characterControl.gotoStartposSimple(GameObject.Find(getChessName(playerid, chessid)), playerid * 4 + chessid);
    }

    private void enableAllInteraction()//单双混合，暂时不要用
    {
        characterControl.enableAllPhysics();//开启或关闭所有物理交互
        showDiceButtonUI();//开启或关闭骰子UI
    }

    private void gameEnd()//游戏结束
    {
        isEnd = true;

        RankList rankList = GameObject.Find("RankListDialog").GetComponent<RankList>();
        string str = jo.Call<String>("Game_End_Info");
        jo.Call("recyclerview_addItem", str);
        string[] data = str.Split(' ');
        for (int i = 0; i < 4; i++)
        {
            //rank,color,playerName,score,chessNum,totalTime
            rankList.inputRankItem(int.Parse(data[i * 6 + 0]), int.Parse(data[i * 6 + 1]), data[i * 6 + 2],
                int.Parse(data[i * 6 + 3]), int.Parse(data[i * 6 + 4]), data[i * 6 + 5]);
        }
        //rankList.testRankList();//插入数据，测试用
        rankList.enableRankListDialog();//显示
        UnityEngine.Object tmpSong = Resources.Load<AudioClip>("Audios/Victory");
        //UnityEngine.Object tmpVictorySong = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Audios/Victory.mp3", typeof(AudioClip));
        AudioSource music = GameObject.Find("BGMusic").GetComponent<AudioSource>();
        music.clip = (AudioClip)Instantiate(tmpSong);//切歌
        music.Play();//放歌
    }

    private void hidechessUI(int playerid)//显示——1；隐藏——2
    {
        string tmp = playerid.ToString();
        List<int> colorUI = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            colorUI.Add(int.Parse(tmp.Substring(i, 1)));
        }
        if (colorUI[0] == 2)
        {
            RawImage[] raws = GameObject.Find("RedUI").GetComponentsInChildren<RawImage>();
            foreach (RawImage rawimage in raws)
                rawimage.CrossFadeColor(Color.gray, 0, false, true);
        }
        if (colorUI[1] == 2)
        {
            RawImage[] raws = GameObject.Find("YellowUI").GetComponentsInChildren<RawImage>();
            foreach (RawImage rawimage in raws)
                rawimage.CrossFadeColor(Color.gray, 0, false, true);
        }
        if (colorUI[2] == 2)
        {
            RawImage[] raws = GameObject.Find("BlueUI").GetComponentsInChildren<RawImage>();
            foreach (RawImage rawimage in raws)
                rawimage.CrossFadeColor(Color.gray, 0, false, true);
        }
        if (colorUI[3] == 2)
        {
            RawImage[] raws = GameObject.Find("GreenUI").GetComponentsInChildren<RawImage>();
            foreach (RawImage rawimage in raws)
                rawimage.CrossFadeColor(Color.gray, 0, false, true);
        }
    }

    private void roundEnd()//界面显示回合结束消息
    {
        //调用Android端回合结束函数
        jo.Call("End_Turn");
    }

    public void showDiceButtonUI()//显示骰子按钮
    {
        GameObject diceButton = GameObject.Find("DiceButton");
        diceButton.GetComponent<Button>().enabled = true;
        diceButton.GetComponent<RawImage>().CrossFadeColor(Color.white, 0, false, true);
        GameObject.Find("CFX4 Aura Bubble C").transform.position = diceButtonUIPosition;
        return;
    }

    public void hideDiceButtonUI()//隐藏骰子按钮
    {
        GameObject diceButton = GameObject.Find("DiceButton");
        diceButton.GetComponent<Button>().enabled = false;
        diceButton.GetComponent<RawImage>().CrossFadeColor(Color.gray, 0, false, true);
        GameObject.Find("CFX4 Aura Bubble C").transform.position = new Vector3(100f, 100f, 100f);
        return;
    }


    private void roundStart(int playerid)
    {
        GameObject.Find("CFX3_Fire_Shield").transform.localPosition = circularInColor[playerid];

    }

    private void turnLeft(int playerid, int chessid)
    {
        myChess = GameObject.Find(getChessName(playerid, chessid));
        myChess.GetComponent<UnityChanControlScriptWithRgidBody>().rotateCharacter_Left();
    }

    public float getForceVarfromAndroid()//安卓给随机力大小的函数写在这里！！
    {
        //切记范围 0.5f~1.0f
        return 0.63f;//这里改写成获取的流程
    }

    private void throwDiceByNum(string step_String)//固定值骰子
    {
        int step = int.Parse(step_String);
        throwDiceByNum(step);
    }

    private void throwDiceByNum(int step)//固定值骰子
    {
        if (enableAnimation)
        {
            float mForceVar;
            switch (step)
            {
                case 1:
                    mForceVar = 0.80f;
                    break;
                case 2:
                    mForceVar = 0.85f;
                    break;
                case 3:
                    mForceVar = 0.63f;
                    break;
                case 4:
                    mForceVar = 0.75f;
                    break;
                case 5:
                    mForceVar = 0.90f;
                    break;
                case 6:
                    mForceVar = 0.70f;
                    break;
                default://可恶啊！不能有default，想作弊走十几步的话要在这里加吖！
                    mForceVar = 0f;
                    break;
            }
            diceGirl.setForceVar(mForceVar);
            diceGirl.throwDice();
            Invoke("getDiceResult", 10.0f);
        }
        else
        {
            System.Random rd = new System.Random();
            fastDice.throwDicebyNum(rd.Next(1, 7));//左闭右开，1~6
            getDiceResult();
        }
        
    }

    public void throwDiceByForce()//随机值骰子
    {
        //float force = jo.Call<float>("randomForce");//这里保留，可以阻塞用户投掷骰子的操作
        float force = 0;//测试用
        if (enableAnimation)
        {
            diceGirl.setForceVar(force);
            //diceGirl.setForceVar(getForceVarfromAndroid());//测试用，实际由安卓提供随机力因子
            diceGirl.throwDice();

            //这里也可以修改变量返回类型，使之传给Android控制端
            Invoke("getDiceResult", 10.0f);
        }
        else
        {
            System.Random rd = new System.Random();
            fastDice.throwDicebyNum(rd.Next(1, 7));//左闭右开，1~6
            getDiceResult();
        }
    }

    private void getDiceResult()
    {
        if (enableAnimation)
        {
            Debug.Log("Action中骰子结果：" + diceGirl.result);
            //调用Android setDice(result)函数
            jo.Call("Player_setDice", diceGirl.result);
        }
        else {
            Debug.Log("Action中骰子结果(瞬间骰子版)：" + fastDice.getDiceNum());
            //调用Android setDice(result)函数
            jo.Call("Player_setDice", fastDice.getDiceNum());
        }
    }

    private void click(int playerid, int chessid)
    {
        throw new NotImplementedException();
    }

    private void activate(int playerid, int chessid)//激活底部光环，一定要与checkListener配合使用！
    {
        myChess = GameObject.Find(getChessName(playerid, chessid));
        myChess.GetComponentInChildren<FairyAnime>().becameVisible();//光环可视化
        fairyQueue.Enqueue(myChess);//入队列
    }

    public void execActionInUI()
    {
        int playerid, chessid, action, step;
        object a;
        a = GameObject.Find("InputField0").GetComponents<InputField>()[0].text;
        playerid = (int)System.Convert.ToUInt32(a);
        a = GameObject.Find("InputField1").GetComponents<InputField>()[0].text;
        chessid = (int)System.Convert.ToUInt32(a);
        a = GameObject.Find("InputField2").GetComponents<InputField>()[0].text;
        action = (int)System.Convert.ToUInt32(a);
        a = GameObject.Find("InputField3").GetComponents<InputField>()[0].text;
        step = (int)System.Convert.ToUInt32(a);
        Debug.Log(playerid + " " + chessid + " " + action + " " + step);
        executeAction(playerid, chessid, action, step);
    }

    public void execActionInAndroidLoop()//测试用，连续指令
    {
        string[] cmd = new string[23];
        cmd[0] = "2 2 1 1";
        cmd[0] = "1 1 13 1";
        cmd[1] = "1 1 1 1";
        cmd[2] = "1 2 1 1";
        cmd[3] = "1 2 2 6";
        cmd[4] = "1 1 2 2";
        cmd[5] = "1 1 8 4";
        cmd[6] = "1 2 3 0";
        cmd[7] = "2 2 2 1";
        cmd[8] = "1 1 2 4";
        cmd[9] = "1 1 8 4";
        cmd[10] = "2 2 4 4";
        cmd[11] = "1 1 2 4";
        cmd[12] = "1 1 7 4";
        cmd[13] = "1 1 8 6";
        cmd[14] = "1 1 7 6";
        cmd[15] = "1 1 8 4";
        cmd[16] = "1 1 2 6";
        cmd[17] = "1 1 2 6";
        cmd[18] = "1 1 2 4";//快步有错？
        cmd[19] = "1 1 7 4";
        cmd[20] = "1 1 2 6";
        cmd[21] = "1 1 5 4";
        cmd[22] = "1 2 5 4";
        for (int i = 0; i < 23; i++)
            execActionInAndroid(cmd[i]);
    }

    public void execActionInAndroid(string command)//安卓调用时的接口，带并发控制
    {
        string[] cmdCode = command.Split(' ');
        int playerid = int.Parse(cmdCode[0]);
        int chessid = int.Parse(cmdCode[1]);
        int action = int.Parse(cmdCode[2]);
        int step = int.Parse(cmdCode[3]);
        Debug.Log("安卓端信息:" + playerid + " " + chessid + " " + action + " " + step);
        Command com = new Command(playerid, chessid, action, step);
        commandQueue.Enqueue(com);//入命令队列
    }

    public void submitCommand()//并发时，每完成一次，则提交新的命令
    {
        if (commandQueue.Count == 0) { preCommand.setCommand(0, 0, 0, 0); return; }//队列为空，直接返回
        Command com = commandQueue.Dequeue();//队列不为空，且上一条指令执行完毕了，下一条指令出栈
        executeAction(com.playerid, com.chessid, com.action, com.step);//执行！
        preCommand = com;//将当前指令置为上一条指令
    }

    public void ChangeNameInUI(string data)
    {
        string[] temp = data.Split(' ');
        int pos = int.Parse(temp[0]);
        string mName = temp[1];
        string mColor = "";
        if (pos == 0) mColor = "RedUI";
        if (pos == 1) mColor = "YellowUI";
        if (pos == 2) mColor = "BlueUI";
        if (pos == 3) mColor = "GreenUI";
        GameObject.Find("Canvas/" + mColor).GetComponentInChildren<Text>().text = mName;
    }

    // Use this for initialization
    void Start()
    {

        fairyQueue = new Queue<GameObject>();
        commandQueue = new Queue<Command>();
        preCommand = new Command(0, 0, 0, 0);
        circularInColor = new List<Vector3>();
        diceButtonUIPosition = GameObject.Find("CFX4 Aura Bubble C").transform.position;
        for (int i = 0; i < 4; i++)
        {
            circularInColor.Add(new Vector3(-830f, 440f - i * 130f, 10f));//圆环距离计算公式
        }
        //jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //jo.Call("start_game");
    }

    // Update is called once per frame
    void FixedUpdate()
    {



        if (preCommand.action == 2 || preCommand.action == 6 || preCommand.action == 7 || preCommand.action == 8 || preCommand.action == 12
            || preCommand.action == 15)
        {
            myChess = GameObject.Find(getChessName(preCommand.playerid, preCommand.chessid));
            if (myChess.GetComponent<UnityChanControlScriptWithRgidBody>().maintainStand == 5) submitCommand();//检验速度是否为0
        }

        if (preCommand.action == 1)
        {
            if (gameObject.GetComponent<CharacterControl>().isGoing == false) submitCommand();//检验是否完成起飞动作
        }

        if (preCommand.action == 4)
        {
            if (gameObject.GetComponent<CharacterControl>().isFalling == false) submitCommand();//检验是否完成坠机动作
        }

        if (preCommand.action == 11)
        {
            if (diceGirl.waitCount == 0) submitCommand();
        }

        if (preCommand.action == 0 || preCommand.action == 3 || preCommand.action == 5 || preCommand.action == 9 || preCommand.action == 13
            || preCommand.action == 14 || preCommand.action == 16 || preCommand.action == 17 || preCommand.action == 18 || preCommand.action == 19
            || preCommand.action == 20)
            submitCommand();

        //if (Input.GetKeyDown(KeyCode.M)) execActionInAndroidLoop();

        if (Input.GetMouseButton(0) && isEnd)
        {
            //切换场景
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Y)) execActionInAndroid("0 0 25 0");
    }

}