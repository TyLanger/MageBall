using UnityEngine;
using System.Collections;

public class TeamController : MonoBehaviour {

	[SerializeField]
	GameObject[] players;
	int numPlayers = 0;
	int maxNumPlayers = 10;

	int numStartingTeams = 2;
	// this leaves room if I want to make more than just 2 teams.
	int maxNumberTeams = 4;
	public Team[] teams;

	// Use this for initialization
	void Start () {
		players = new GameObject[maxNumPlayers];
		//teams = new Team[maxNumberTeams];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Team addPlayerToGame(GameObject player)
	{
		if ((players [numPlayers] == null) && numPlayers < maxNumPlayers) {
			players [numPlayers] = player;
			teams [numPlayers % numStartingTeams].addPlayerToTeam (player);
			teams [numPlayers % numStartingTeams].OnTeamMemberDeath += teamDied;
			numPlayers++;
			return teams [numPlayers - 1];
		} else {
			return null;
		}

	}

	void teamDied()
	{
		Debug.Log ("Player from a team died");
		// when a member on one team dies, add points to the other team?
	}
		
}
