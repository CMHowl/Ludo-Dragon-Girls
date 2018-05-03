using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastAnimate : MonoBehaviour {

    //---骰子相关---
    public GameObject bigDice;
    private Vector3[] diceEulerAngles;
    [SerializeField]
    private GameObject smokePS;
    private Vector3 dicePostition;
    private int curDice;
    //--------------

    public int getDiceNum()
    {
        return curDice;
    }

    public void setDiceNum(int num)
    {
        curDice = num;
    }

    private void enablefastDice()
    {
        bigDice.transform.position = new Vector3(0f, 5f, 0f);
        smokePS.GetComponent<ParticleSystem>().Play();
        print("快速骰子点数：" + getDiceNum());
        Invoke("disablefastDice", 3f);//自动销毁
    }

    private void disablefastDice()
    {
        bigDice.transform.position = dicePostition;
        smokePS.GetComponent<ParticleSystem>().Stop();
    }

    public void throwDicebyNum(int num)
    {
        setDiceNum(num);//存贮当前骰子的值
        setDiceEulerAngles(num);//先设置点数（欧拉角直接赋值）
        enablefastDice();//再显示骰子
    }

    private void setDiceEulerAngles(int num)
    {       
        bigDice.transform.eulerAngles = diceEulerAngles[num - 1];
    }

        // Use this for initialization
        void Start () {
        diceEulerAngles = new Vector3[6];
        //1~6骰子对应0~5下标
        diceEulerAngles[0] = new Vector3(270, 0, 0);
        diceEulerAngles[1] = new Vector3(180, 0, 0);
        diceEulerAngles[2] = new Vector3(0, 0, 270);
        diceEulerAngles[3] = new Vector3(0, 0, 90);
        diceEulerAngles[4] = new Vector3(0, 0, 0);
        diceEulerAngles[5] = new Vector3(90, 0, 0);
        dicePostition = bigDice.transform.position;
        smokePS.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update () {
    }
}