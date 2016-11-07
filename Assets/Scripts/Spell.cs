using System;
using UnityEngine;

public class Spell : MonoBehaviour
{
	public string spellName;
	GameObject caster;

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
			return;
		} else {
			Debug.Log (col.gameObject + " " + this.caster);
		}
		var health = col.gameObject.GetComponent<Health> ();
		bool friendly;

		if (col.tag == "Wall") {
			hitWall (col.gameObject);
		} else if (col.tag == "Player") {
			hitPlayer (col.gameObject);
		} else if (col.tag == "Spell") {
			hitSpell (col.gameObject);
		}
		if (health != null) {
			hitHealth (col.gameObject, this);
		}
	}

}
