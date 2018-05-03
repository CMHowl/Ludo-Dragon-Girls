using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterControl : MonoBehaviour {

    public List<Vector3> startpos;
    public List<Vector3> takeoffpos;
    public List<Quaternion> startrot;//初始方向

    /*
     * moveToStartLine相关
     */
    public GameObject fire;
    public bool isGoing;
    int goTimeCount;
    GameObject currentChess;
    int currentPlayerid;
    int currentChessid;
    float deep;
    bool fireStart;

    public bool isFalling;
    public GameObject tornado;

    public bool isInteract;//是否需要交互

    public void initCharacter()//初始化，把所有棋子的初始位置记录下来
    {
        GameObject[] chesses=new GameObject[16];
        for (int i = 0; i < 16; i++)
        {
            chesses[i] = GameObject.Find("Chess" + i);
        }
        //需要自己 先添加一个chess的Tag分别添加给各个棋子
        
        foreach (GameObject chess in chesses)
        {
            startpos.Add(chess.transform.position);
            //遍历，把所有棋子初始位置记录下来  
            startrot.Add(chess.transform.localRotation);
        }
    }

    public void initStartLine()//初始化起飞点
    {
        //GameObject[] takeoffs = GameObject.FindGameObjectsWithTag("startline").OrderBy(g => g.transform.GetSiblingIndex()).ToArray();
        //需要自己 先添加一个startline的Tag分别添加给各个起飞点
        GameObject[]takeoffs = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            takeoffs[i] = GameObject.Find("StartLine" + i);
        }

        foreach (GameObject takeoff in takeoffs)
        {
            takeoffpos.Add(takeoff.transform.position);
            //把所有起飞点位置记录下来  
        }
    }

    public void gotoStartpos(GameObject chess, int posCode)//闪现到基地
    {
        //提示：posCode=playerid*4+chessid

        currentPlayerid = posCode / 4;
        currentChessid = posCode % 4;
        isFalling = true;
        fallenPosVoice();
    }

    public void gotoStartposSimple(GameObject chess, int posCode)//闪现到基地瞬移版，用于完成动画
    {
        //提示：posCode=playerid*4+chessid

        chess.transform.position = startpos[posCode];
        chess.transform.localRotation = startrot[posCode];

    }

    public void gotoTakeoffposSimple(int playerid, int chessid)//闪现到起飞点
    {
        //提示：posCode=playerid*4+chessid
        currentChess = GameObject.Find(Action.getChessName(playerid, chessid));
        currentChess.transform.position = takeoffpos[playerid];
    }

    public void gotoTakeoffpos(int playerid, int chessid)//缓慢到起飞点
    {
        //提示：posCode=playerid*4+chessid
        //chess.transform.position = takeoffpos[posCode];

        currentPlayerid = playerid;
        currentChessid = chessid;
        isGoing = true;
        Invoke("takeoffPosVoice", 3.5f);
    }

    public void teleportChess(GameObject chess, int pos)//闪现至棋盘任意位置
    {
        string readStr = "";

        TextAsset ta = Resources.Load("values/RelativeTransform") as TextAsset;
        byte[] array = Encoding.ASCII.GetBytes(ta.ToString());
        MemoryStream stream = new MemoryStream(array);             //convert stream 2 string      
        StreamReader reader = new StreamReader(stream);
        while (reader.Peek() >= 0)//流还有东西
        {
            readStr = reader.ReadLine();//从流中读取一行
            string[] tmparr = readStr.Split(' ');
            if (int.Parse(tmparr[0]) == pos) //位置与索引号匹配，则取出内容
            {
                Vector3 tPos = new Vector3(float.Parse(tmparr[1]), float.Parse(tmparr[2]), float.Parse(tmparr[3]));//获取位置
                Quaternion tLocalRot = new Quaternion(float.Parse(tmparr[5]), float.Parse(tmparr[6]), float.Parse(tmparr[7]), float.Parse(tmparr[4]));//获取方向
                //没有写错，因为读取的时候w轴与x轴读反了，所以这里颠倒回来赋值
                chess.transform.position = tPos;
                chess.transform.localRotation = tLocalRot;
            }
        }
        reader.Close();//关闭流
    }

    public void invisibleChess(GameObject chess)//隐身
    {
        chess.transform.position = new Vector3(99f, 99f, 99f);//真让物体消失太麻烦了。。移到摄像机外就好啦~
    }

    public void enableAllPhysics()//禁用所有物理交互
    {
        if (isInteract)//当前为可选
        {
            GameObject.Find("MainCamera").GetComponent<PhysicsRaycaster>().enabled = false;
            //print("已禁用发射器");
            isInteract = false;
            return;
        }

        if (!isInteract)//当前为可选
        {
            GameObject.Find("MainCamera").GetComponent<PhysicsRaycaster>().enabled = true;
            //print("已启用发射器");
            isInteract = true;
            return;
        }
    }

    public void takeoffPosVoice()//起飞人物音效
    {
        UnityEngine.Object tmpSong = Resources.Load<AudioClip>("Audios/Player" + currentPlayerid.ToString() + "Start");
        //UnityEngine.Object tmpSong = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Audios/Player" + currentPlayerid.ToString() + "Start.mp3", typeof(AudioClip));
        AudioSource music = GameObject.Find("CharacterAudio").GetComponent<AudioSource>();
        music.clip = (AudioClip)Instantiate(tmpSong);
        music.Play();
    }

    public void fallenPosVoice()//坠机人物音效
    {
        UnityEngine.Object tmpSong = Resources.Load<AudioClip>("Audios/Player" + currentPlayerid.ToString() + "Fallen");
        //UnityEngine.Object tmpSong = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Audios/Player" + currentPlayerid.ToString() + "Fallen.mp3", typeof(AudioClip));
        AudioSource music = GameObject.Find("CharacterAudio").GetComponent<AudioSource>();
        music.clip = (AudioClip)Instantiate(tmpSong);
        music.Play();
    }

    public void victoryPosVoice(int playerid)//胜利人物音效
    {
        UnityEngine.Object tmpSong = Resources.Load<AudioClip>("Audios/Player" + playerid.ToString() + "Victory");
        //UnityEngine.Object tmpSong = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Audios/Player" + playerid.ToString() + "Victory.mp3", typeof(AudioClip));
        AudioSource music = GameObject.Find("CharacterAudio").GetComponent<AudioSource>();
        music.clip = (AudioClip)Instantiate(tmpSong);
        music.Play();
    }


    // Use this for initialization
    void Start () {
        startpos = new List<Vector3>();
        takeoffpos = new List<Vector3>();
        initCharacter();
        initStartLine();
        Debug.Log(startpos[7] + " and " + startpos[8]);
        isInteract = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (isGoing)
        {
            currentChess = GameObject.Find(Action.getChessName(currentPlayerid, currentChessid));


            //人物动画
            //下潜
            if (goTimeCount > 0 && goTimeCount < 100)
            {
                currentChess.transform.Rotate(Vector3.up, 30f);
                if (goTimeCount > 30 && goTimeCount < 100)
                {
                    currentChess.transform.Translate(Vector3.down * 0.2f);
                    deep = currentChess.transform.position.y;
                }
            }

            //瞬移
            if (goTimeCount > 100 && goTimeCount < 110)
            {
                //moveToStartLine(currentPlayerid, currentChessid);

                currentChess.transform.position = takeoffpos[currentPlayerid];

                fire.transform.position = currentChess.transform.position;

                currentChess.transform.position = new Vector3(currentChess.transform.position.x, deep, currentChess.transform.position.z);

                fireStart = false;
                fire.SetActive(false);
            }

            //上浮
            if (goTimeCount > 110 && goTimeCount < 210)
            {
                currentChess.transform.Rotate(Vector3.down, 30f);
                if (goTimeCount > 110 && goTimeCount < 180)
                {
                    currentChess.transform.Translate(Vector3.up * 0.2f);
                }
            }

            //火焰动画
            if (goTimeCount > 110 && goTimeCount < 220)
            {
                fire.transform.Rotate(Vector3.back, 2f);
                if (!fireStart)
                {
                    fireStart = true;
                    fire.SetActive(true);
                }
            }

            if (goTimeCount++ > 220)
            {
                isGoing = false;

                fireStart = false;
                //fire.SetActive(false);

                fire.transform.position = new Vector3(99f, 99f, 99f);

                goTimeCount = 0;
            }
        }

        if (isFalling)//回城时台风特效
        {
            currentChess = GameObject.Find(Action.getChessName(currentPlayerid, currentChessid));

            //台风动画
            if (goTimeCount > 0 && goTimeCount < 10)
            {
                tornado.transform.position = currentChess.transform.position;
                tornado.SetActive(true);
            }

            //人物动画
            //飞到天上
            if (goTimeCount > 10 && goTimeCount < 100)
            {
                currentChess.transform.Rotate(Vector3.up, 30f);
                if (goTimeCount > 30 && goTimeCount < 100)
                {
                    currentChess.transform.Translate(Vector3.up * 0.5f);
                    deep = currentChess.transform.position.y;
                }
            }

            if (goTimeCount++ > 110)
            {
                isFalling = false;

                //fire.SetActive(false);

                tornado.transform.position = new Vector3(99f, 99f, 99f);

                goTimeCount = 0;

                currentChess.transform.position = startpos[currentPlayerid*4+currentChessid];
                currentChess.transform.localRotation = startrot[currentPlayerid * 4 + currentChessid];
            }
        }

    }

}
