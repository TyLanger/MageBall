using System;
using UnityEngine;

public class Spell : MonoBehaviour
{
	public string spellName;

	private float cost;

	private float cooldown;

	public virtual void castSpell()
	{
	}
}
