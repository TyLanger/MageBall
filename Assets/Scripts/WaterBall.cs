using UnityEngine;
using System.Collections;

public class WaterBall : Ball {

	/*
	public override void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Fire Ball") {
			Destroy (col.gameObject);
		}
		base.OnTriggerEnter(col);
	}
	*/

	public override void hitPlayer(GameObject hit)
	{
		hit.transform.position = this.transform.position;
	}
}
