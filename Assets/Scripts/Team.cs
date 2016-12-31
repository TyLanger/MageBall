using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Team : NetworkBehaviour {

	public string teamName;
	public Color teamColour;
	public GameObject[] members;
	int numCurrentMembers = 0;
	int maxTeamSize = 5;

	[SyncVar(hook="updateScore")]
	public int score;
	public Transform spawnLocation;

	public event System.Action OnTeamMemberDeath;

	// Use this for initialization
	void Start () {
		members = new GameObject[maxTeamSize];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addPlayerToTeam(GameObject player)
	{
		if (numCurrentMembers < maxTeamSize) {
			members [numCurrentMembers] = player;
			numCurrentMembers++;
			player.GetComponent<Health> ().OnDeath += OnMemberDeath;
			//player.GetComponent<MeshRenderer>().material.color = teamColour;
			//player.GetComponentInChildren<MeshRenderer>().material.color = teamColour;
			player.GetComponent<PlayerController> ().changeHeadColour (teamColour);

		}
	}

	public void changeScore(int points)
	{
		Debug.Log ("changeScore");
		score += points;
	}

	public void updateScore(int newScore)
	{
		Debug.Log ("Update Scores in Team: " + teamName + " " + score + " " + newScore);
		score = newScore;
		FindObjectOfType<GUIcontroller> ().updateScores (this, newScore);
	}

	void OnMemberDeath()
	{
		// when a team member dies.
		if (OnTeamMemberDeath != null) {
			Debug.Log ("OnMemberDeath");
			OnTeamMemberDeath ();
		}
	}
}
