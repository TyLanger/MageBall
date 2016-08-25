using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public Vector3 cameraOffset;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = FindObjectOfType<PlayerController> ().gameObject;
		}
		this.transform.position = player.transform.position + cameraOffset;
	}
}
