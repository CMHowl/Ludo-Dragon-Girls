using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankList : MonoBehaviour {

    public GameObject rankListDialog;
    public Vector3 dialogPosition;

    public GameObject[] rankItem;
    private List<string> userNameList;

    public void enableRankListDialog()
    {
        rankListDialog.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);

    }

    public void SetColor(GameObject item,int color)//设置颜色
    {
        
        if (color == 0)//红色
        {
            UnityEngine.Object tmpTexture = Resources.Load<Texture>("Textures/RedCharacter");
            // tmpTexture = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Textures/RedCharacter.png", typeof(Texture));
            item.GetComponent<RawImage>().texture = (Texture)Instantiate(tmpTexture);
        }
        if (color == 1)//黄色
        {
            UnityEngine.Object tmpTexture = Resources.Load<Texture>("Textures/YellowCharacter");
            //Object tmpTexture = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Textures/YellowCharacter.png", typeof(Texture));
            item.GetComponent<RawImage>().texture = (Texture)Instantiate(tmpTexture);
        }
        if (color == 2)//蓝色
        {
            UnityEngine.Object tmpTexture = Resources.Load<Texture>("Textures/BlueCharacter");
            //Object tmpTexture = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Textures/BlueCharacter.png", typeof(Texture));
            item.GetComponent<RawImage>().texture = (Texture)Instantiate(tmpTexture);
        }
        if (color == 3)//绿色
        {
            UnityEngine.Object tmpTexture = Resources.Load<Texture>("Textures/GreenCharacter");
            //Object tmpTexture = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Textures/GreenCharacter.png", typeof(Texture));
            item.GetComponent<RawImage>().texture = (Texture)Instantiate(tmpTexture);
        }

    }

    public void testRankList()//这里提供了一个示范，教你怎么用RankList
    {
        inputRankItem(1, 0, userNameList[0], 8888, 4, "24:40");//可以用列表项0号
        inputRankItem(2, 3, "猹泽瑞尔2号", 3670, 1, "17:31");//也支持自己另设置名字
        inputRankItem(3, -1, "", 0, 0, "");
        inputRankItem(4, -1,"", 0, 0, "");
    }

    public void GetNamefromMainUI()//从主游戏窗口获取用户名
    {
        userNameList = new List<string>();
        userNameList.Add(GameObject.Find("Canvas/RedUI/Text").GetComponent<Text>().text);
        userNameList.Add(GameObject.Find("Canvas/YellowUI/Text").GetComponent<Text>().text);
        userNameList.Add(GameObject.Find("Canvas/BlueUI/Text").GetComponent<Text>().text);
        userNameList.Add(GameObject.Find("Canvas/GreenUI/Text").GetComponent<Text>().text);
    }


    //设置一条列表项
    public void inputRankItem(int rank, int color, string playerName, int score, int chessNum, string totalTime)
    {
        
        GameObject parentItem = GameObject.Find("RankListDialog/Rankitem" + rank.ToString());//找到父对象（一个列表项）
        if (color == -1)//发现此项为-1，即代表不需要显示这一列表项
        {
            parentItem.SetActive(false);
            return;
        }
        Transform[] allChildren = parentItem.GetComponentsInChildren<Transform>();
       foreach (Transform child in allChildren)//通过Transform快速遍历子对象，就不要用Find了
        {
            if (child.gameObject.name == "RawImage")//设置图片
            {
                SetColor(child.gameObject, color);
                continue;
            }
            if (child.gameObject.name == "Name")//设置玩家名
            {
                child.gameObject.GetComponent<Text>().text = playerName;
                continue;
            }
            if (child.gameObject.name == "Score")//设置分数
            {
                child.gameObject.GetComponent<Text>().text = score.ToString();
                continue;
            }
            if (child.gameObject.name == "ChessNum")//设置完成棋子数
            {
                child.gameObject.GetComponent<Text>().text = chessNum.ToString();
                continue;
            }
            if (child.gameObject.name == "TotalTime")//设置时间（原路程接口）
            {
                child.gameObject.GetComponent<Text>().text = totalTime;
                continue;
            }
        }
    }


	// Use this for initialization
	void Start () {
        dialogPosition = rankListDialog.GetComponent<RectTransform>().anchoredPosition;
        GetNamefromMainUI();
        //testRankList();//记得禁用这里，或者改成你想要的
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
