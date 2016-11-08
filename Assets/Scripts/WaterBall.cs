using UnityEngine;
using System.Collections;

public class WaterBall : Ball {

	float bubbleTime = 1.5f;
	/*
	public override void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Fire Ball") {
			Destroy (col.gameObject);
		}
		base.OnTriggerEnter(col);
	}
	*/

	public override void hitHealth(GameObject hit, Spell thisSpell)
	{
		// The target that gets hit will only take the initial damage, not the explosion damage
		// Targets will also take damage from walking in to the explosion, not just from being there when it explodes
		if (base.exploded) {
			hit.GetComponent<Health> ().TakeDamage (this.explosionDamage, thisSpell);
		} else {
			hit.GetComponent<Health> ().TakeDamage (this.damage, thisSpell);
		}
		//endOfFlight = true;
	}

	public override void hitPlayer(GameObject hit)
	{
		hit.GetComponent<PlayerController> ().bubbled(this.transform, bubbleTime);
		//hit.transform.position = this.transform.position;
	}
}
