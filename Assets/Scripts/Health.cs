using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {

	public const int maxHealth = 100;

	public bool destroyOnDeath;

	Vector3 spawnLocation;

	public event System.Action OnDeath;

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
			die ();
		}

		//healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
		//OnChangeHealth(currentHealth);
	}

	//[ContextMenu("Self Destruct")] not sure exactly how this works. Might need to be public virtual void
	public void die ()
	{
		if (OnDeath != null) {
			OnDeath ();
		}
		if (destroyOnDeath) {
			Destroy (gameObject);
		}
		currentHealth = maxHealth;

		// called on the Server, but invoked on the Clients
		RpcRespawn ();
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