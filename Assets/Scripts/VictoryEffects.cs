using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryEffects : MonoBehaviour {

    public GameObject fireworks;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Substring(0, 5) == "Chess")
        {
            UnityEngine.Object tmpSong = Resources.Load<AudioClip>("Audios/Fireworks");
            AudioSource music = GameObject.Find("SceneAudio").GetComponent<AudioSource>();
            music.clip = (AudioClip)Instantiate(tmpSong);
            music.Play();
            fireworks.SetActive(true);
            Invoke("stopFireworks", 2f);
        }
    }

    public void stopFireworks()
    {      
        fireworks.SetActive(false);
    }
    //void OnTriggerExit(Collider collider)
    //{
    //    print(collider.gameObject.name + ":" + Time.time);
    //    GameObject root = GameObject.Find("Sphere");
    //    GameObject son = root.transform.Find("VictoryFireworks").gameObject;
    //    son.SetActive(false);
    //}

    // Use this for initialization
    void Start () {
        stopFireworks();
    }
	
	// Update is called once per frame
	void Update () {
	}
}
