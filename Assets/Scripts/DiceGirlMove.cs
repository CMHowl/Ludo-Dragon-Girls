using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGirlMove : MonoBehaviour
{

    private Animator animator;

    public GameObject home;
    public GameObject dice;

    public float forceVar;                //骰子力因子，range:0.5f~1f  

    public Camera mainCamera;
    public Camera myCamera;

    Vector3 cameraLocal;
    Vector3 dicePositionLocal;            //骰子初始位置
    Vector3 dicePositionXZ;               //骰子落地位置      
    Quaternion diceRotationLocal;         //骰子初始方向
    Vector3 diceSize;                     //骰子大小
    Vector3 rotateAround;
    Vector3 walkDirection;
    Vector3 stopPosition;

    float diceHight;                      //骰子初始高度
    float time1, time2, time3;
    float cameraDirection;

    public int result;                           //骰子结果
    public int waitCount;                        //抛骰子延迟
    int start;

    public void initDice()
    { }

    public void resetDice()
    {
        dice.transform.position = dicePositionLocal;
        dice.transform.rotation = diceRotationLocal;
        dice.GetComponent<Rigidbody>().useGravity = false;
        dice.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        dice.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        mainCamera.enabled = true;
        myCamera.enabled = false;
    }

    public void throwDice()
    {
        dice.GetComponent<Rigidbody>().useGravity = true;
        dice.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePosition & ~RigidbodyConstraints.FreezeRotation;
    }

    public float getForceVar()
    {
        return forceVar;//测试用，可以让安卓端传过来
    }

    public void setForceVar(float mForceVar)
    {
        forceVar = mForceVar;
    }

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        time1 = 3;
        time2 = -1;
        time3 = -3;
        waitCount = 0;
        start = 0;

        dicePositionLocal = dice.transform.position;
        diceRotationLocal = dice.transform.rotation;
        cameraLocal = mainCamera.transform.position;
        if (cameraLocal.z > 0)
            cameraDirection = 1;
        else
            cameraDirection = -1;
        myCamera.transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);
        resetDice();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (animator.GetBool("IsGetMessage"))
        {
            mainCamera.enabled = false;
            myCamera.enabled = true;

            time2 += Time.fixedDeltaTime;
            if (!animator.GetBool("IsGetDice"))
            {
                //向骰子移动
                transform.position = Vector3.Lerp(home.transform.position, dicePositionXZ, 4 * time2 / time1);
                animator.SetBool("IsInHome", false);
            }


            //转身
            if (time2 > -0.5 && time2 < 0)
            {
                //向量叉乘
                rotateAround.x = transform.forward.y * walkDirection.z - transform.forward.z * walkDirection.y;
                rotateAround.y = transform.forward.z * walkDirection.x - transform.forward.x * walkDirection.z;
                rotateAround.z = transform.forward.x * walkDirection.y - transform.forward.y * walkDirection.x;

                transform.RotateAround(transform.position, rotateAround, Time.fixedDeltaTime * 200f);
            }

            //走到骰子旁边
            if (transform.position.x > dicePositionXZ.x - (diceSize.x / 2) * 1.8
                && transform.position.x < dicePositionXZ.x + (diceSize.x / 2) * 1.8
                && transform.position.z > dicePositionXZ.z - (diceSize.z / 2) * 1.8
                && transform.position.z < dicePositionXZ.z + (diceSize.z / 2) * 1.8)
            {
                animator.SetBool("IsGetDice", true);
            }

            if (animator.GetBool("IsGetDice"))
            {
                if (waitCount++ >= 50 && animator.GetBool("IsGetMessage"))
                {
                    Rigidbody rigidbody = dice.GetComponent<Rigidbody>();

                    //附一个斜向上的随机力
                    //float y = Random.Range(0.5f, 1f);
                    float y = getForceVar();
                    rigidbody.AddForce(new Vector3(0, y, 0) * 60, ForceMode.Impulse);
                    //print(walkDirection);
                    //rigidbody.AddForce(walkDirection * (-1), ForceMode.Impulse);
                    //rigidbody.AddForce(new Vector3(17.8f, 0.1f, -9.3f) * (-1), ForceMode.Impulse);
                    UnityEngine.Object tmpSong = Resources.Load<AudioClip>("Audios/Flying");
                    //加入音效
                    //UnityEngine.Object tmpSong = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Audios/Flying.mp3", typeof(AudioClip));
                    AudioSource music = GameObject.Find("DiceAudio").GetComponent<AudioSource>();
                    music.clip = (AudioClip)Instantiate(tmpSong);
                    music.Play();


                    rigidbody.AddForceAtPosition(new Vector3(17.8f, 0.1f, -9.3f) * (-1), new Vector3(1f, 1f, 1f), ForceMode.Impulse);
                    animator.SetBool("IsGetDice", false);
                    animator.SetBool("IsGetMessage", false);

                    stopPosition = transform.position;
                    time2 = -2;
                    waitCount = 0;
                }
            }
        }
        else
        {
            //抛完骰子后回到地毯

            if (!animator.GetBool("IsInHome"))
            {

                time3 += Time.fixedDeltaTime;

                if (time3 > -1 && time3 < 0)
                {
                    transform.RotateAround(transform.position, rotateAround * (-1), Time.fixedDeltaTime * 200f);
                }

                transform.position = Vector3.Lerp(stopPosition, home.transform.position,  time3 / time1);
                if (transform.position == home.transform.position)
                {
                    animator.SetBool("IsInHome", true);
                    start = 0;
                    time3 = -3;
                }

                if (time3 > -3 && time3 < 0)
                {
                    myCamera.transform.position = Vector3.Lerp(myCamera.transform.position,
                        dice.transform.position + new Vector3(0f, 10f, cameraDirection * 12f), Time.fixedDeltaTime * 5);
                }

                if (time3 > 0.5f && time3 < 0.8f)
                {
                    myCamera.transform.RotateAround(dice.transform.position, Vector3.left, cameraDirection * 2.8f);
                }

                if (time3 > 1.5f && time3 < 2)
                {
                    if (time3 > 1.5f && time3 < 1.8f)
                    {
                        myCamera.transform.RotateAround(dice.transform.position, Vector3.right, cameraDirection * 2.8f);
                    }
                    myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, cameraLocal, (time3 - 1.5f) / 0.5f);
                    if (dice.transform.up.y > 0.8f)
                    {
                        result = 5;
                    }
                    if (dice.transform.forward.y > 0.8f)
                    {
                        result = 1;
                    }
                    if (dice.transform.right.y > 0.8f)
                    {
                        result = 4;
                    }
                    if (dice.transform.up.y < -0.8f)
                    {
                        result = 2;
                    }
                    if (dice.transform.forward.y < -0.8f)
                    {
                        result = 6;
                    }
                    if (dice.transform.right.y < -0.8f)
                    {
                        result = 3;
                    }


                }

                if (time3 > 2 && time3 < 3)
                {
                    //print(result);
                }
                if (time3 > 2 && time3 < 3)
                {
                    //取消骰子重力，归回原位,锁定移动
                    dice.transform.position = dicePositionLocal;
                    dice.transform.rotation = diceRotationLocal;
                    dice.GetComponent<Rigidbody>().useGravity = false;
                    dice.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    dice.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

                    mainCamera.enabled = true;
                    myCamera.enabled = false;
                }
            }
        }

        //if (Input.GetMouseButton(1))
        //{
        //    //给骰子赋重力，解除锁定
        //    dice.GetComponent<Rigidbody>().useGravity = true;
        //    dice.GetComponent<Rigidbody>().constraints = ~RigidbodyConstraints.FreezePosition & ~RigidbodyConstraints.FreezeRotation;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Dice"))
        {
            if (start == 0)
            {
                animator.SetBool("IsGetMessage", true);

                //获取骰子落地位置和大小
                dicePositionXZ = new Vector3();
                dicePositionXZ.Set(other.gameObject.transform.position.x, 0, other.gameObject.transform.position.z);
                diceHight = other.gameObject.transform.position.y;
                diceSize = other.bounds.size;
                walkDirection = dicePositionXZ - transform.position;

                start = 1;
            }

        }
    }
}
