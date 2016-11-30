using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GUIcontroller : NetworkBehaviour {

	//[SyncVar(hook = "updateScores")]
	public Text scores;
	public Canvas canvas;
	public Font defaultFont;

	public Text newText;
	GameObject newGO;

	public Text newText2;

	TeamController teamController;

	Team[] teams;

	void Start()
	{
		teamController = GetComponent<TeamController> ();
		teams = teamController.teams;

		/*
		// both work
		newGO = new GameObject ("new Text");
		newText = newGO.AddComponent<Text> ();
		newText.gameObject.transform.SetParent (canvas.transform);
		newText.font = defaultFont;
		newText.color = Color.black;

		newText2 = new GameObject ("new2").AddComponent<Text> ();
		newText2.gameObject.transform.SetParent (canvas.transform);
		newText2.text = "text2";
		newText2.font = defaultFont;
		newText2.color = Color.black;
		// sets the text box in the middle of the canvas
		newText2.rectTransform.position = canvas.transform.position;
		*/
	}

	public void updateScores(Team t, int newScore)
	{
		//scores.text = "Green team: " + teams [0].score + " Yellow team: " + teams [1].score;

		scores.text = "";
		for (int i = 0; i < teams.Length; i++) {
			string name = teams [i].teamName;
			int score = teams [i].score;

			if (name == t.teamName) {
				score = newScore;
			}
			Debug.Log (name + ": " + score + " ");
			scores.text += name + ": " + score + " ";
			if (score > 4) {
				scores.text = name + " wins!";
				scores.rectTransform.position = canvas.transform.position;
				scores.fontSize = 40;
			}
		}
	}

}
