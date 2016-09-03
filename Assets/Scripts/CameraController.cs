using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public Vector3 cameraOffset;

	// Use this for initialization
	void Start () {
		// camera is in the scene before the player is.
		// this will never work.
		// Either have the player call the camera to say it's in the scene or make a delegate/event
		player = FindObjectOfType<PlayerController> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = FindObjectOfType<PlayerController> ().gameObject;
		} else {
			this.transform.position = player.transform.position + cameraOffset;
		}
	}
}
