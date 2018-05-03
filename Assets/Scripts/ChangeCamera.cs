using UnityEngine;
using System.Collections;

public class ChangeCamera : MonoBehaviour
{

    public int curCamera = 1;//当前摄像机
	public float myTime = 0f;

    private Vector3 Camera1pos;
    private Vector3 Camera1rot;
    private Vector3 Camera2pos;
    private Vector3 Camera2rot;

	private bool isChanging = false;
    void Start()
    {
        Camera1pos = new Vector3(0f, 50f, -50f);
        Camera1rot = new Vector3(50f, 0f, 0f);
        Camera2pos = new Vector3(0f, 50f, 50f);
        Camera2rot = new Vector3(50f, 180f, 0f);
    }

    // Update is called once per frame  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) autoCamSwitch();
		if (isChanging) {
            myTime++;
            if (curCamera == 1)
            {
                gameObject.transform.eulerAngles = Vector3.Lerp(Camera1rot, Camera2rot, myTime / 100.0f);
                gameObject.transform.position = Vector3.Lerp(Camera1pos, Camera2pos, myTime / 100.0f);

            }
            if (curCamera == 2)
            {
                gameObject.transform.position = Vector3.Lerp(Camera2pos, Camera1pos, myTime / 100.0f);
                gameObject.transform.eulerAngles = Vector3.Lerp(Camera2rot, Camera1rot, myTime / 100.0f);
            }
            if (myTime >= 100.0f) {
				myTime = 0f;
				isChanging = false;
                curCamera++;
			}           
        }
    }

    public void autoCamSwitch()
    {
        if (curCamera == 3) curCamera = 1;
        cameraSwitch(curCamera);
    }

    void cameraSwitch(int currentCam)
    {
		isChanging = true;
		Debug.Log("正在切换视角");
    }
}