using System;
using UnityEngine;

public class Spell : MonoBehaviour
{
	public string spellName;
	public enum spellType {Fire, Water, Air, Earth, None};
	public spellType type;
	public spellType weaknessType;

	private float cost;

	private float cooldown;

	public virtual void castSpell()
	{
		/*
		if (cost < PlayerInput.mana) {
			if (timeSinceLastCast > cooldown) {

			}
		}
		*/
	}
}
