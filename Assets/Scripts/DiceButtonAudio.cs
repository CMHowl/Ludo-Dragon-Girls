using System.Collections;
using UnityEngine;

public class DiceButtonAudio : MonoBehaviour {

    public void playDiceButtonAudio()
    {
        UnityEngine.Object tmpSong = Resources.Load<AudioClip>("Audios/DiceButton");
        //UnityEngine.Object tmpSong = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Audios/DiceButton.mp3", typeof(AudioClip));
        AudioSource music = GameObject.Find("DiceAudio").GetComponent<AudioSource>();
        music.clip = (AudioClip)Instantiate(tmpSong);
        music.Play();
    }

    public void diceCallBack()//回调函数，就做两件事：禁用控件和放音效     
    {
        GameObject.Find("ChessManager").GetComponent<Action>().hideDiceButtonUI();
        playDiceButtonAudio();
    }
  
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
