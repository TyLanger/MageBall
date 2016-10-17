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


	public void setCaster(GameObject player)
	{
		this.caster = player;
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

	public virtual void hitHealth(GameObject hit)
	{

	}

	void OnTriggerEnter(Collider col)
	{
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
			hitHealth (col.gameObject);
		}
	}

}
