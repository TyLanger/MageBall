using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public Vector3 cameraOffset;
	public bool hasPlayer;

	// Use this for initialization
	void Start () {
		// camera is in the scene before the player is.
		// this will never work.
		// Either have the player call the camera to say it's in the scene or make a delegate/event
		//player = FindObjectOfType<PlayerController> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {



		if (hasPlayer) {
			//this.transform.rotation = Quaternion.Euler(new Vector3(60, -player.transform.rotation.y, 0));
			this.transform.position = player.transform.position + cameraOffset;
		}
	}

	public void addPlayer(GameObject target)
	{
		player = target;
		if (player != null)
			hasPlayer = true;
	}
}
