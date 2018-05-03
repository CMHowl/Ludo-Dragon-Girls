using UnityEngine;
using System.Collections;

public class QuerySDEmotionalController : MonoBehaviour {

	[SerializeField]
	Material[] emotionalMaterial;

	[SerializeField]
	GameObject queryFace;

	public enum QueryChanSDEmotionalType
	{
		// Normal emotion
		NORMAL_DEFAULT = 0,
		NORMAL_ANGER = 1,
		NORMAL_BLINK = 2,
		NORMAL_GURUGURU = 3,
		NORMAL_SAD = 4,
		NORMAL_SMILE = 5,
		NORMAL_SURPRISE = 6

	}


	public void ChangeEmotion (QueryChanSDEmotionalType faceNumber)
	{
		queryFace.GetComponent<Renderer>().material = emotionalMaterial[(int)faceNumber];
	}
	
}
