using UnityEngine;
using System.Collections;

public class TargetAoE : Spell {

	public float range;
	public float AreaEffect;
	public float armTime;
	public int damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		armTime -= Time.deltaTime;
		if (armTime <= 0) {
			//GetComponent<SphereCollider> ().enabled = true;
		}

		if (transform.position.y < 0) {
			Destroy (this.gameObject);
		}
	}

	public override void castSpell()
	{
		transform.position += new Vector3 (0, range, 0);
	}

	void OnTriggerEnter(Collider col)
	{
		var health = col.gameObject.GetComponent<Health> ();
		if (health != null) {
			health.TakeDamage (damage);
		}
	}
}
