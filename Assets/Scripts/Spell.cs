using System;
using UnityEngine;

public class Spell : MonoBehaviour
{
	public string spellName;
	public GameObject caster;

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
}
