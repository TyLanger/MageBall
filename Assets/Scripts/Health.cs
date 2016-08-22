using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {

	public const int maxHealth = 100;

	public bool destroyOnDeath;

	Vector3 spawnLocation;

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;
	public RectTransform healthBar;

	void Start()
	{
		spawnLocation = transform.position;
	}

	public void TakeDamage(int amount)
	{
		if (!isServer)
		{
			// if no server, this quits
			return;
		}

		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			if (destroyOnDeath) {
				Destroy (gameObject);
			}
			currentHealth = maxHealth;

			// called on the Server, but invoked on the Clients
			RpcRespawn ();

		}

		//healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
		//OnChangeHealth(currentHealth);
	}

	void OnChangeHealth (int health)
	{
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			// move back to starting location
			transform.position = spawnLocation;
		}
	}
}