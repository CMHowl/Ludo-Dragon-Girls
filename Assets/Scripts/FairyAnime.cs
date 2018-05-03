using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyAnime : MonoBehaviour {

    public void becameVisible()
    {
        gameObject.GetComponent<ParticleSystem>().Play();//播放粒子
    }

    public void becameInvisible()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();//停止粒子
    }
    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();//停止粒子
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
}
