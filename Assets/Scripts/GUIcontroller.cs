using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIcontroller : MonoBehaviour {

	public Text scores;
	public Canvas canvas;


	TeamController teamController;

	Team[] teams;

	void Start()
	{
		teamController = GetComponent<TeamController> ();
		teams = teamController.teams;
	}

	public void updateScores()
	{
		//scores.text = "Green team: " + teams [0].score + " Yellow team: " + teams [1].score;

		scores.text = "";
		for (int i = 0; i < teams.Length; i++) {
			scores.text += teams [i].teamName + ": " + teams [i].score + " ";
		}
	}

}
