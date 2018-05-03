using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CharacterListener : MonoBehaviour, IPointerClickHandler
{
    GameObject[] chesses = new GameObject[16];
    public bool isClicked = false;
    public AndroidJavaClass jc;
    public AndroidJavaObject jo;

    private void AutocancelAllFairy()
    {
        if (gameObject.GetComponent<UnityChanControlScriptWithRgidBody>().maintainStand != 5)//当前不为静止
        {
            StartCoroutine(cancelAllFairy());
            Action.fairyQueue.Clear();//清空队列
        }
    }

    public void CharacterVoice(string playernum)
    {
        UnityEngine.Object tmpSong = Resources.Load<AudioClip>("Audios/Player" + playernum + "Howl");
        //UnityEngine.Object tmpSong = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Audios/Player"+playernum+"Howl.mp3", typeof(AudioClip));
        AudioSource music = GameObject.Find("CharacterAudio").GetComponent<AudioSource>();
        music.clip = (AudioClip)Instantiate(tmpSong);
        music.Play();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("点击了" + gameObject.name);
        foreach (GameObject tmp in Action.fairyQueue)
        {
            if (tmp.name == gameObject.name)//出队，发现能匹配到当前点击物体
            {
                isClicked = true;
                StartCoroutine(cancelAllFairy());
                int playernum = int.Parse(gameObject.name.Substring(5));
                playernum = playernum / 4;
                CharacterVoice(playernum.ToString());
                Action.fairyQueue.Clear();//清空队列
                break;
            }
        }

        string text = "You haved clicked: " + gameObject.name;
        jo.Call("recyclerview_addItem", text);
        //对gameObject.name进行分割
        string chess_num = gameObject.name.Substring(5);
        int chess = int.Parse(chess_num);
        jo.Call("Player_setChess", chess);
        //需要用到Unity与安卓交互时此处取消注释，否则报JNI异常
    }

    IEnumerator cancelAllFairy()
    {
        yield return null;//协程挂起，直接偷偷回收光环特效
        foreach (GameObject chess in chesses)
        {
            chess.GetComponentInChildren<FairyAnime>().becameInvisible();
        }
    }


    // Use this for initialization
    void Start () {

        for (int i = 0; i < 16; i++)
        {
            chesses[i] = GameObject.Find("Chess" + i);
        }

        //jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }
	
	// Update is called once per frame
	void Update () {
        AutocancelAllFairy();
	}
}
