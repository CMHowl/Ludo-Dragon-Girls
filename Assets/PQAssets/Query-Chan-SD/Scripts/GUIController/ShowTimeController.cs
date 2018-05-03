using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowTimeController : MonoBehaviour {

	[SerializeField]
	string NextSceneName = "";
	
	[SerializeField]
	string NextSceneButtonLabel = "";

	[SerializeField]
	GameObject[] Queries;

	[SerializeField]
	QuerySDMecanimController.QueryChanSDAnimationType[] QueriesDefaultAnimType;

	[SerializeField]
	float intervalChangeAnimTime;
	float nextChangeAnimTime;
	
	[SerializeField]
	float intervalChangeEmotionTime;
	float nextChangeEmotionTime;

	[SerializeField]
	Image imagePref;

	[SerializeField]
	Sprite[] spritePrefs;


	// Use this for initialization
	void Start () {
		ChangeAnimQueries ();
		ChangeEmotionQueries ();
		nextChangeAnimTime = Time.time + intervalChangeAnimTime;
		nextChangeEmotionTime = Time.time + intervalChangeEmotionTime;

		imagePref.sprite = spritePrefs[1];
	}
	
	// Update is called once per frame
	void Update () {

		if (nextChangeAnimTime < Time.time)
		{
			ChangeAnimQueries ();
			nextChangeAnimTime = Time.time + intervalChangeAnimTime;
		}

		if (nextChangeEmotionTime < Time.time)
		{
			ChangeEmotionQueries ();
			nextChangeEmotionTime = Time.time + intervalChangeEmotionTime;
		}

	}

	void OnGUI () {

		//SceneChange --------------------------------------------
		
		if (GUI.Button (new Rect (Screen.width -150, Screen.height-100, 150,100), NextSceneButtonLabel))
		{
			Application.LoadLevel( NextSceneName );
		}

	}

	void ChangeAnimQueries () {

		for (int i=0; i < Queries.Length; i++)
		{
			Queries[i].GetComponent<QuerySDMecanimController>().ChangeAnimation(QueriesDefaultAnimType[Random.Range(0, QueriesDefaultAnimType.Length)]);
		}

	}

	void ChangeEmotionQueries () {

		for (int i=0; i < Queries.Length; i++)
		{
			Queries[i].GetComponent<QuerySDEmotionalController>().ChangeEmotion( (QuerySDEmotionalController.QueryChanSDEmotionalType)Random.Range(0, 7));
		}

	}

	public void GotoFanPage () {

		Application.OpenURL ("http://query-chan.com/queryparty/");

	}

	public void ChangePanelPref (int panelNumber) {

		imagePref.sprite = spritePrefs[panelNumber];

	}
	
}
