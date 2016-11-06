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

	TeamController teamController;
	Team team;


    // Attack variables
    // int maxProjecties = 10; not implemented yet. Commenting it out to surpress warrnings for now
    public float fireRate;
    float nextFire = 0;

    // Use this for initialization
    void Start () {
		// when the player spawns, it finds the team controller and adds itself to a team
		teamController = FindObjectOfType<TeamController> ();
		team = teamController.addPlayerToGame (this.gameObject);
		//myCamera = FindObjectOfType<CameraController> ();
		//myCamera.addPlayer (this.gameObject);

	}
	
	// Update is called once per frame
	void Update () {

		if (!isLocalPlayer) {
			//Debug.Log ("PlayerController quit.");
			return;
		} else {
			// This makes the camera work for multiple people
			Camera.main.GetComponent<CameraController> ().addPlayer (this.gameObject);
		}

        
    }

    void FixedUpdate()
    {
        //GetComponent<Rigidbody>().MovePosition(GetComponent<Transform>().position + velocity * Time.fixedDeltaTime);
		//Vector3.MoveTowards ();

		// move towards works regardless of player rotation.
		// Other methods move the player in relation to their orientation
		transform.position = Vector3.MoveTowards(transform.position, transform.position + velocity, moveSpeed);
		//GetComponent<Rigidbody>().transform.Translate(velocity);
		//camClone.transform.position = this.transform.position + cameraOffset.position;
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

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

	public void spellCast(int spellIndex)
	{
		CmdSpellCast(spellIndex);
	}

	[Command]
	void CmdSpellCast(int spellIndex)
	{
		Spell spellClone;
		if (spells [spellIndex].GetComponent<TargetAoE> ()) {
			spellClone = (Spell)Instantiate (spells [spellIndex], heightCorrectedPoint, ballSpawn.transform.rotation);
		} else {
			spellClone = (Spell)Instantiate (spells [spellIndex], ballSpawn.transform.position + spells [spellIndex].spawnOffest, ballSpawn.transform.rotation);
		}
		spellClone.GetComponent<Spell> ().castSpell (this.gameObject);
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