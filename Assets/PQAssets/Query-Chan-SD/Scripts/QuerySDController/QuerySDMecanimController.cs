using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuerySDMecanimController : MonoBehaviour {

	[SerializeField]
	GameObject queryBodyParts;
	
	public enum QueryChanSDAnimationType
	{
		// Normal Motion
		NORMAL_STAND = 1,
		NORMAL_WALK = 2,
		NORMAL_RUN = 3,
		NORMAL_IDLE = 4,
		NORMAL_DAMAGE = 5,
		NORMAL_ITEMGET = 6,
		NORMAL_LOSE = 7,
		NORMAL_WIN = 8,
		NORMAL_FLY_IDLE = 9,
		NORMAL_FLY_STRAIGHT = 10,
		NORMAL_FLY_LEFT = 11,
		NORMAL_FLY_RIGHT = 12,
		NORMAL_FLY_UP = 13,
		NORMAL_FLY_DOWN = 14,
		NORMAL_FLY_CIRCLE = 15,
		NORMAL_FLY_TURNBACK = 16,

		// Normal Pose
		NORMAL_POSE_CUTE = 50,
		NORMAL_POSE_HELLO = 51,
		NORMAL_POSE_READY = 52,
		NORMAL_POSE_STOP = 53,
		NORMAL_POSE_BOW = 54,
		NORMAL_POSE_ARMCROSSED = 55,
		NORMAL_POSE_PLEASE = 56,
		NORMAL_POSE_SIT = 57,
		NORMAL_POSE_LAYDOWN = 58,
		NORMAL_POSE_ROMANCE = 59,

		// Black Motion
		BLACK_FIGHTING = 105,
		BLACK_PUNCH = 106,
		BLACK_KICK = 107,

		// Black Pose
		BLACK_POSE_1 = 150,
		BLACK_POSE_2 = 151,
		BLACK_POSE_3 = 152,

		// Osaka Motion
		OSAKA_TUKKOMI = 205,
		OSAKA_BOKE = 206,
		OSAKA_CLAP =207,

		// Osaka Pose
		OSAKA_POSE_GOAL = 250,
		OSAKA_POSE_TEHEPERO = 251,
		OSAKA_POSE_EXIT = 252,

		// Fukuoka Motion
		FUKUOKA_DANCE_1 = 305,
		FUKUOKA_DANCE_2 = 306,
		FUKUOKA_WAIWAI =307,
		
		// Fukuoka Pose
		FUKUOKA_POSE_1 = 350,
		FUKUOKA_POSE_2 = 351,
		FUKUOKA_POSE_HIRUNE = 352,

		// Hokkaido Motion
		HOKKAIDO_SNOWBALLING = 405,
		HOKKAIDO_CLIONE = 406,
		HOKKAIDO_IKADANCE = 407,

		// Hokkaido Pose
		HOKKAIDO_POSE_COLD = 450,
		HOKKAIDO_POSE_BEAMBITIOUS = 451,
		HOKKAIDO_POSE_BEAR = 452

	}

	public enum QueryChanSDHandType
	{

		NORMAL = 0,
		STONE = 1,
		PAPER = 2

	}

	void Update()
	{
		queryBodyParts.transform.localPosition = Vector3.zero;
		queryBodyParts.transform.localRotation = Quaternion.identity;

	}


	public void ChangeAnimation (QueryChanSDAnimationType animNumber, bool isChangeMechanimState=true)
	{

		var emoControl = this.GetComponent<QuerySDEmotionalController>();

		switch (animNumber)
		{
		case QueryChanSDAnimationType.NORMAL_WIN:
		case QueryChanSDAnimationType.NORMAL_ITEMGET:
		case QueryChanSDAnimationType.NORMAL_POSE_HELLO:
		case QueryChanSDAnimationType.BLACK_POSE_1:
		case QueryChanSDAnimationType.OSAKA_CLAP:
		case QueryChanSDAnimationType.OSAKA_POSE_GOAL:
		case QueryChanSDAnimationType.FUKUOKA_WAIWAI:
			emoControl.ChangeEmotion(QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_SMILE);
			break;

		case QueryChanSDAnimationType.NORMAL_DAMAGE:
		case QueryChanSDAnimationType.BLACK_POSE_3:
			emoControl.ChangeEmotion(QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_GURUGURU);
			break;

		case QueryChanSDAnimationType.NORMAL_LOSE:
		case QueryChanSDAnimationType.NORMAL_POSE_PLEASE:
		case QueryChanSDAnimationType.BLACK_POSE_2:
			emoControl.ChangeEmotion(QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_SAD);
			break;

		case QueryChanSDAnimationType.NORMAL_FLY_CIRCLE:
		case QueryChanSDAnimationType.OSAKA_BOKE:
		case QueryChanSDAnimationType.OSAKA_POSE_EXIT:
			emoControl.ChangeEmotion(QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_SURPRISE);
			break;

		case QueryChanSDAnimationType.NORMAL_POSE_READY:
		case QueryChanSDAnimationType.NORMAL_POSE_LAYDOWN:
		case QueryChanSDAnimationType.NORMAL_POSE_ROMANCE:
		case QueryChanSDAnimationType.BLACK_KICK:
		case QueryChanSDAnimationType.FUKUOKA_DANCE_2:
		case QueryChanSDAnimationType.FUKUOKA_POSE_HIRUNE:
			emoControl.ChangeEmotion(QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_BLINK);
			break;
		
		case QueryChanSDAnimationType.NORMAL_POSE_BOW:
		case QueryChanSDAnimationType.NORMAL_POSE_ARMCROSSED:
		case QueryChanSDAnimationType.BLACK_FIGHTING:
		case QueryChanSDAnimationType.OSAKA_TUKKOMI:
			emoControl.ChangeEmotion(QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_ANGER);
			break;

		default:
			emoControl.ChangeEmotion(QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_DEFAULT);
			break;
		}


		if (isChangeMechanimState) {
			queryBodyParts.GetComponent<Animator>().SetInteger("AnimIndex", (int)animNumber);
		}

	}

}
