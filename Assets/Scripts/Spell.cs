using System;
using UnityEngine.Networking;
using UnityEngine;

public class Spell : NetworkBehaviour
{
	public string spellName;

	[SyncVar]
	public GameObject caster;

	public enum spellType {Fire, Water, Air, Earth, None};
	public spellType type;
	public spellType weaknessType;

	public Vector3 spawnOffest;

	private float cost;

	private float cooldown;

	// Maybe put these in a child of Spell
	// RecastableSpell ?
	// public bool recastable;
	// private float recastTime;

	public void setCaster(GameObject player)
	{
		this.caster = player;
	}

	public GameObject getCaster()
	{
		return this.caster;	
	}

	public virtual void castSpell(GameObject player)
	{
		setCaster (player);
		/*
		if (cost < PlayerInput.mana) {
			if (timeSinceLastCast > cooldown) {

			}
		}
		*/
	}

	public virtual void hitSpell(GameObject hit)
	{

	}

	public virtual void hitPlayer(GameObject hit)
	{

	}

	public virtual void hitWall(GameObject hit)
	{

	}

	public virtual void hitHealth(GameObject hit, Spell thisSpell)
	{

	}

	void OnTriggerEnter(Collider col)
	{
		// Ignore the collision if it hits itself.
		// I don't know if this is how I want it forever.
		if (col.gameObject == this.caster) {
			//Debug.Log ("Same");
			return;
		} 

		var health = col.gameObject.GetComponent<Health> ();
		bool friendly = false;

		// check to see if the thing hit has a player controller
		if (col.gameObject.GetComponent<PlayerController> () != null) {
			// if a spell hits a target with the same team, set friendly to true
			if (col.gameObject.GetComponent<PlayerController> ().getTeam () == this.caster.GetComponent<PlayerController> ().getTeam ()) {
				friendly = true;
			}
		}

		if (col.tag == "Wall") {
			hitWall (col.gameObject);
		} else if (col.tag == "Player" && !friendly) {
			// if they have a player tag and are not friendly(i.e. different team), do the player effect
			hitPlayer (col.gameObject);
		} else if (col.tag == "Spell") {
			hitSpell (col.gameObject);
		}
		if (health != null && !friendly) {
			// if they have a health component and are not friendly, do damage
			hitHealth (col.gameObject, this);
		}
	}

}
