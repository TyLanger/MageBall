using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour {

    public float moveSpeed = 10.0f;
    public GameObject ballPrefab;
	public GameObject ballSpawn;
	Vector3 heightCorrectedPoint; 

	//Spells
	public Spell[] spells;
    
    Vector3 velocity;

	// These 2 should be put in a new class or struct maybe?
	public enum statusEffect {Free, Bubbled, Rooted};
	public statusEffect moveStatus;
	float statusTimeLeft;
	public Transform bubble;

	TeamController teamController;
	Team team;


    // Attack variables
    // int maxProjecties = 10; not implemented yet. Commenting it out to surpress warrnings for now
    public float fireRate;
    float nextFire = 0;

    void Start () {
		// when the player spawns, it finds the team controller and adds itself to a team
		teamController = FindObjectOfType<TeamController> ();
		team = teamController.addPlayerToGame (this.gameObject);


	}

	public override void OnStartLocalPlayer()
	{
		// Used to set the player to be blue here.
		// Add something else to differentiate the player
		// Maybe health bar is bigger?


		// This makes the camera work for multiple people
		// Had this originally in Update
		// Seems to work just as well in start. If something goes wrong, move it back?
		// Makes most sense to have it here.
		Camera.main.GetComponent<CameraController> ().addPlayer (this.gameObject);
	}
	
	void Update () {

		if (!isLocalPlayer) {
			//Debug.Log ("PlayerController quit.");
			return;
		}

        
    }

    void FixedUpdate()
    {
        //GetComponent<Rigidbody>().MovePosition(GetComponent<Transform>().position + velocity * Time.fixedDeltaTime);
		//Vector3.MoveTowards ();

		// move towards works regardless of player rotation.
		// Other methods move the player in relation to their orientation
		if (moveStatus == statusEffect.Free) {
			transform.position = Vector3.MoveTowards (transform.position, transform.position + velocity, moveSpeed);
		} else if (moveStatus == statusEffect.Bubbled) {
			transform.position = bubble.transform.position;
		}

		if (statusTimeLeft > 0) {
			statusTimeLeft -= Time.deltaTime;
		} else if (moveStatus != statusEffect.Free) {
			moveStatus = statusEffect.Free;
		}

		//GetComponent<Rigidbody>().transform.Translate(velocity);
		//camClone.transform.position = this.transform.position + cameraOffset.position;
    }

	public void bubbled(Transform bubbler, float bubbleTime)
	{
		moveStatus = statusEffect.Bubbled;
		bubble = bubbler;
		statusTimeLeft = bubbleTime;
	}

	public Team getTeam()
	{
		return team;
	}

	public void move(float xInput, float zInput)
	{

		Vector3 moveInput = new Vector3(xInput, 0, zInput);

		velocity = Vector3.ClampMagnitude ((moveInput * (moveSpeed * Time.deltaTime)), moveSpeed * Time.deltaTime);
			//new Vector3 (xInput, 0, zInput) * moveSpeed;
	}

    public void LookAt(Vector3 lookPoint)
    {
		heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

	public void spellCast(int spellIndex)
	{
		CmdSpellCast(spellIndex, this.gameObject);
	}

	[Command]
	void CmdSpellCast(int spellIndex, GameObject caster)
	{
		Spell spellClone;
		if (spells [spellIndex].GetComponent<TargetAoE> ()) {
			spellClone = (Spell)Instantiate (spells [spellIndex], heightCorrectedPoint, ballSpawn.transform.rotation);
		} else {
			spellClone = (Spell)Instantiate (spells [spellIndex], ballSpawn.transform.position + spells [spellIndex].spawnOffest, ballSpawn.transform.rotation);
		}
		// This debug is only run on the server
		// This means that if the extra window is the server, it doesn't show up in the editor
		// Debug.Log ("this.gameObject: "+this.gameObject);
		// Need to pass in parameters
		// Just because this function is inside this script, doesn't mean it can access them?
		spellClone.GetComponent<Spell> ().castSpell (caster);
		var spellGO = spellClone.gameObject;
		NetworkServer.Spawn (spellGO);
	}

	/* Old version of spell casting
	public void fire()
	{
		if (Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			CmdFire();
			/*
            instanceBall = (GameObject)Instantiate(ball, transform.position, transform.rotation);
            instanceBall.GetComponent<BallController>().moveBall();

		}
	}

    [Command]
    void CmdFire()
	{
		var ball = (GameObject)Instantiate (ballPrefab, ballSpawn.transform.position, ballSpawn.transform.rotation);

		//ball.GetComponent<BallController> ().CmdMoveBall ();
		ball.GetComponent<BallController> ().moveBall ();

		//ball.GetComponent<Rigidbody>().velocity = ball.transform.forward * 6;

		NetworkServer.Spawn (ball);

	}
	*/
}