using UnityEngine;
using System.Collections;

public class Team : MonoBehaviour {

	public string teamName;
	public Color teamColour;
	public GameObject[] members;
	int numCurrentMembers = 0;
	int maxTeamSize = 5;
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
			player.GetComponent<MeshRenderer>().material.color = teamColour;
		}
	}

	public void changeScore(int points)
	{
		score += points;
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
