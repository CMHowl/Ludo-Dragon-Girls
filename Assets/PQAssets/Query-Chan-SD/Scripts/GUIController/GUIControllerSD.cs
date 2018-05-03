using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIControllerSD : MonoBehaviour {

	Vector3 PosDefault;
	[SerializeField]
	GameObject CameraObj;
	private bool cameraUp;
	[SerializeField]
	protected GameObject queryChan;
	private int querySoundNumber;
	private int targetNum;
	List<string> targetSounds = new List<string>();
	
	//===============================

	[SerializeField]
	QuerySDMecanimController.QueryChanSDAnimationType defaultAnimType = QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_STAND;

	[SerializeField]
	bool showCommon, showNormal, showBlack, showOsaka, showFukuoka, showHokkaido;
	
	[SerializeField]
	string NextSceneName = "";
	
	[SerializeField]
	string NextSceneButtonLabel = "";
	
	class ButtonInfo {
		public string buttonLabel;
		public int id;
		
		public ButtonInfo(string label, object _id) {
			buttonLabel = label;
			id =  (int)_id;
		}
	}

	private ButtonInfo[] animButtonInfoCommon = {
		// Common animations
		new ButtonInfo("Nomal_Stand",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_STAND),
		new ButtonInfo("Nomal_Walk",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_WALK),
		new ButtonInfo("Nomal_Run",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_RUN),
		new ButtonInfo("Nomal_Idle",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_IDLE),
	};

	private ButtonInfo[] animButtonInfoNormal = {
		// Normal animations
		new ButtonInfo("Nomal_Damage",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_DAMAGE),
		new ButtonInfo("Nomal_ItemGet",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_ITEMGET),
		new ButtonInfo("Nomal_Lose",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_LOSE),
		new ButtonInfo("Nomal_Win",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_WIN),
		new ButtonInfo("Nomal_FlyIdle",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_FLY_IDLE),
		new ButtonInfo("Nomal_FlyStraight",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_FLY_STRAIGHT),
		new ButtonInfo("Nomal_FlyLeft",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_FLY_LEFT),
		new ButtonInfo("Nomal_FlyRight",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_FLY_RIGHT),
		new ButtonInfo("Nomal_FlyUp",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_FLY_UP),
		new ButtonInfo("Nomal_FlyDown",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_FLY_DOWN),
		new ButtonInfo("Nomal_FlyCircle",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_FLY_CIRCLE),
		new ButtonInfo("Nomal_FlyTurnback",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_FLY_TURNBACK),
		new ButtonInfo("Nomal_Pose_Cute",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_CUTE),
		new ButtonInfo("Nomal_Pose_Hello",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_HELLO),
		new ButtonInfo("Nomal_Pose_Ready",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_READY),
		new ButtonInfo("Nomal_Pose_Stop",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_STOP),
		new ButtonInfo("Nomal_Pose_Bow",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_BOW),
		new ButtonInfo("Nomal_Pose_ArmCrossed",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_ARMCROSSED),
		new ButtonInfo("Nomal_Pose_Please",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_PLEASE),
		new ButtonInfo("Nomal_Pose_Sit",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_SIT),
		new ButtonInfo("Nomal_Pose_LayDown",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_LAYDOWN),
		new ButtonInfo("Nomal_Pose_Romance",			QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_ROMANCE),
	};
	
	private ButtonInfo[] animButtonInfoBlack = {
		
		// Black Query-Chan animations
		new ButtonInfo("Black_Fighting",			QuerySDMecanimController.QueryChanSDAnimationType.BLACK_FIGHTING),
		new ButtonInfo("Black_Punch",			QuerySDMecanimController.QueryChanSDAnimationType.BLACK_PUNCH),
		new ButtonInfo("Black_Kick",			QuerySDMecanimController.QueryChanSDAnimationType.BLACK_KICK),
		new ButtonInfo("Black_Pose_1",			QuerySDMecanimController.QueryChanSDAnimationType.BLACK_POSE_1),
		new ButtonInfo("Black_Pose_2",			QuerySDMecanimController.QueryChanSDAnimationType.BLACK_POSE_2),
		new ButtonInfo("Black_Pose_3",			QuerySDMecanimController.QueryChanSDAnimationType.BLACK_POSE_3),
	};

	private ButtonInfo[] animButtonInfoOsaka = {
		
		// Osaka Query-Chan animations
		new ButtonInfo("Osaka_Tukkomi",			QuerySDMecanimController.QueryChanSDAnimationType.OSAKA_TUKKOMI),
		new ButtonInfo("Osaka_Boke",			QuerySDMecanimController.QueryChanSDAnimationType.OSAKA_BOKE),
		new ButtonInfo("Osaka_Clap",			QuerySDMecanimController.QueryChanSDAnimationType.OSAKA_CLAP),
		new ButtonInfo("Osaka_Pose_Goal",			QuerySDMecanimController.QueryChanSDAnimationType.OSAKA_POSE_GOAL),
		new ButtonInfo("Osaka_Pose_Tehepero",			QuerySDMecanimController.QueryChanSDAnimationType.OSAKA_POSE_TEHEPERO),
		new ButtonInfo("Osaka_Pose_Exit",			QuerySDMecanimController.QueryChanSDAnimationType.OSAKA_POSE_EXIT),
	};

	private ButtonInfo[] animButtonInfoFukuoka = {
		
		// Fukuoka Query-Chan animations
		new ButtonInfo("Fukuoka_Dance_1",			QuerySDMecanimController.QueryChanSDAnimationType.FUKUOKA_DANCE_1),
		new ButtonInfo("Fukuoka_Dance_2",			QuerySDMecanimController.QueryChanSDAnimationType.FUKUOKA_DANCE_2),
		new ButtonInfo("Fukuoka_Waiwai",			QuerySDMecanimController.QueryChanSDAnimationType.FUKUOKA_WAIWAI),
		new ButtonInfo("Fukuoka_Pose_1",			QuerySDMecanimController.QueryChanSDAnimationType.FUKUOKA_POSE_1),
		new ButtonInfo("Fukuoka_Pose_2",			QuerySDMecanimController.QueryChanSDAnimationType.FUKUOKA_POSE_2),
		new ButtonInfo("Fukuoka_Pose_Hirune",			QuerySDMecanimController.QueryChanSDAnimationType.FUKUOKA_POSE_HIRUNE),
	};

	private ButtonInfo[] animButtonInfoHokkaido = {
		
		// Hokkaido Query-Chan animations
		new ButtonInfo("Hokkaido_Snowballing",			QuerySDMecanimController.QueryChanSDAnimationType.HOKKAIDO_SNOWBALLING),
		new ButtonInfo("Hokkaido_Clione",			QuerySDMecanimController.QueryChanSDAnimationType.HOKKAIDO_CLIONE),
		new ButtonInfo("Hokkaido_IkaDance",			QuerySDMecanimController.QueryChanSDAnimationType.HOKKAIDO_IKADANCE),
		new ButtonInfo("Hokkaido_Pose_Cold",			QuerySDMecanimController.QueryChanSDAnimationType.HOKKAIDO_POSE_COLD),
		new ButtonInfo("Hokkaido_Pose_BeAmbitious",			QuerySDMecanimController.QueryChanSDAnimationType.HOKKAIDO_POSE_BEAMBITIOUS),
		new ButtonInfo("Hokkaido_Pose_Bear",			QuerySDMecanimController.QueryChanSDAnimationType.HOKKAIDO_POSE_BEAR),
	};
	
	// ------------------------------------
	
	private ButtonInfo[] emotionButtonInfo = {
		new ButtonInfo("Default",		QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_DEFAULT),
		new ButtonInfo("Anger",		QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_ANGER),
		new ButtonInfo("Blink",			QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_BLINK),
		new ButtonInfo("Guruguru",			QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_GURUGURU),
		new ButtonInfo("Sad",			QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_SAD),
		new ButtonInfo("Smile",		QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_SMILE),
		new ButtonInfo("Surprise",			QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_SURPRISE),
	};
	
	//==============================
	
	
	
	void Start() {
		
		PosDefault = CameraObj.transform.localPosition;
		cameraUp = false;
		querySoundNumber = 0;
		
		foreach (AudioClip targetAudio in queryChan.GetComponent<QuerySDSoundController>().soundData)
		{
			targetSounds.Add(targetAudio.name);
		}
		targetNum = targetSounds.Count - 1;
		
		ChangeAnimation((int)defaultAnimType);
		
	}
	
	void OnGUI(){
		
		//AnimationChange ------------------------------------------------
		float animButtonHeight = Screen.height/ (animButtonInfoNormal.Length + animButtonInfoBlack.Length + 1 ) - 3;
		
		
		GUILayout.BeginHorizontal(GUILayout.Width(Screen.width/4));
			
			GUILayout.BeginVertical();
			
				if (showCommon) 	{ ShowAnimationButtons(animButtonInfoCommon, 	animButtonHeight); }
				if (showNormal) 	{ ShowAnimationButtons(animButtonInfoNormal, 	animButtonHeight); }
				if (showBlack)		{ ShowAnimationButtons(animButtonInfoBlack, 		animButtonHeight); }
				if (showOsaka)		{ ShowAnimationButtons(animButtonInfoOsaka, 		animButtonHeight); }
				if (showFukuoka)	{ ShowAnimationButtons(animButtonInfoFukuoka, 	animButtonHeight); }
				if (showHokkaido)	{ ShowAnimationButtons(animButtonInfoHokkaido, animButtonHeight); }
			
			GUILayout.EndVertical();
			
		GUILayout.EndHorizontal();
		
		
		//FaceChange ------------------------------------------------
		float emotionButtonHeight =  (Screen.height-200) / (emotionButtonInfo.Length+1) - 3;
		
		GUILayout.BeginArea(new Rect(Screen.width- Screen.width/4, 0, Screen.width/4, Screen.height-200));
			
			GUILayout.BeginVertical();
				
				foreach (var tmpInfo in emotionButtonInfo) {
					if (GUILayout.Button(tmpInfo.buttonLabel, GUILayout.Height(emotionButtonHeight))) {
						ChangeFace((QuerySDEmotionalController.QueryChanSDEmotionalType)tmpInfo.id);
					}
				}
			
			GUILayout.EndVertical();
		
		GUILayout.EndArea();
		
		
		//CameraChange --------------------------------------------
		
		if (GUI.Button (new Rect (Screen.width / 2 -75, 0, 150, 80), "Camera"))
		{
			if (cameraUp == true)
			{
				CameraObj.GetComponent<Camera>().fieldOfView = 60;
				CameraObj.transform.localPosition = new Vector3(PosDefault.x, PosDefault.y, PosDefault.z);
				cameraUp = false;
			}
			else
			{
				CameraObj.GetComponent<Camera>().fieldOfView = 25;
				CameraObj.transform.localPosition = new Vector3(PosDefault.x, PosDefault.y + 0.1f, PosDefault.z);
				cameraUp = true;
			}
		}
		
		
		//Sound ---------------------------------------------------------
		
		if(GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 100, 50 ,100), "<---"))
		{
			querySoundNumber--;
			if (querySoundNumber < 0)
			{
				querySoundNumber = targetNum;
			}
		}
		if(GUI.Button(new Rect(Screen.width / 2 + 100, Screen.height - 100, 50 ,100), "--->"))
		{
			querySoundNumber++;
			if (querySoundNumber > targetNum)
			{
				querySoundNumber = 0;
			}
			
		}
		if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 70, 200 ,70), "Play"))
		{
			queryChan.GetComponent<QuerySDSoundController>().PlaySoundByNumber(querySoundNumber);
		}
		
		GUI.Label (new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 30), (querySoundNumber+1) + " / " + (targetNum+1) + "  :  " + targetSounds[querySoundNumber]);
		
		
		//SceneChange --------------------------------------------
		
		if (GUI.Button (new Rect (Screen.width -150, Screen.height-100, 150,100), NextSceneButtonLabel))
		{
			Application.LoadLevel( NextSceneName );
		}
		
	}
	
	
	void ShowAnimationButtons(ButtonInfo[] infos, float buttonHeight)
	{
		foreach (var tmpInfo in infos) {
			if ( GUILayout.Button(tmpInfo.buttonLabel, GUILayout.Height(buttonHeight)) ){
				ChangeAnimation(tmpInfo.id);
			}
		}
	}
	
	
	void ChangeFace (QuerySDEmotionalController.QueryChanSDEmotionalType faceNumber) {
		
		queryChan.GetComponent<QuerySDEmotionalController>().ChangeEmotion(faceNumber);
		
	}
	

	void ChangeAnimation (int animNumber)
	{
		queryChan.GetComponent<QuerySDMecanimController>().ChangeAnimation((QuerySDMecanimController.QueryChanSDAnimationType)animNumber);
	}

}
