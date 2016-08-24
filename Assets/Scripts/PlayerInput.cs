using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInput : NetworkBehaviour
{
	PlayerController player;

	private void Start()
	{
		player = GetComponent<PlayerController>();
	}

	private void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		player.move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane plane = new Plane(Vector3.up, new Vector3(0f, this.player.ballSpawn.transform.position.y, 0f));
		float distance;
		if (plane.Raycast(ray, out distance))
		{
			Vector3 point = ray.GetPoint(distance);
			this.player.LookAt(point);
		}
		if (Input.GetButtonDown("Fire1"))
		{
			this.player.spellCast(0);
		}
		if (Input.GetButtonDown("Fire2"))
		{
			this.player.spellCast(1);
		}
	}


}
