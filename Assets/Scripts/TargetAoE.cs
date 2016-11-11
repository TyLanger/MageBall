using UnityEngine;
using System.Collections;

public class TargetAoE : Spell {

	public float range;
	public float height;
	public float AreaEffect;
	public float armTime;
	public int damage;


	// Use this for initialization
	void Start () {
		transform.localScale *= AreaEffect / 2;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -GetComponent<SphereCollider>().radius*2) {
			Destroy (this.gameObject);
		}
	}

	void FixedUpdate() {
		GetComponent<Rigidbody> ().velocity = new Vector3 (0f, -height / armTime, 0f);
	}

	public override void castSpell(GameObject player)
	{
		base.castSpell(player);

		transform.position += new Vector3 (0, height, 0);
	}

	public override void hitHealth(GameObject hit, Spell thisSpell)
	{
		hit.GetComponent<Health> ().TakeDamage (this.damage, thisSpell);
	}

	/* Now uses the base version of the spell
	void OnTriggerEnter(Collider col)
	{
		var health = col.gameObject.GetComponent<Health> ();
		if (health != null) {
			health.TakeDamage (damage);
		}
	}
	*/
}
