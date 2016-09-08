using UnityEngine;
using System.Collections;

public class Team : MonoBehaviour {

	public string teamName;
	Color teamColour;
	GameObject[] members;
	int numCurrentMembers = 0;
	int maxTeamSize = 5;
	int score;
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
		}
	}

	void OnMemberDeath()
	{
		// when a team member dies.
		if (OnTeamMemberDeath != null) {
			OnTeamMemberDeath ();
		}
	}
}
