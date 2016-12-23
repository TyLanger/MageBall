using System;
using UnityEngine;

public class Ball : Spell
{
	public float initialVelocity;

	private float currentVelocity;

	public float acceleration;

	private float angle;

	public float range;

	public float flightDuration;

	private float timeAlive;

	private bool endOfFlight;

	public int damage;

	public int explosionDamage;

	public float numBounces;

	public bool explodesOnImpact;

	public bool explodesAtEnd;

	public float AreaEffect;

	protected bool exploded;

	private SphereCollider sCol;
	Rigidbody rbody;
	Transform trans;

	public float raySpacing = 0.1f;
	[SerializeField]
	Vector2[] rayOffsets;
	float nextRayCheck = 0;
	public float timeBetweenChecks = 1;

	Vector3 lastNormal = Vector3.one;
	Vector3 bounceVector;


	private void Start()
	{
		this.timeAlive = 0f;
		this.sCol = base.GetComponent<SphereCollider>();
		rbody = GetComponent<Rigidbody> ();
		trans = this.transform;
		initializeRays ();
	}

	private void FixedUpdate()
	{
		this.timeAlive += Time.deltaTime;
		if (this.timeAlive > this.flightDuration && !this.endOfFlight)
		{
			this.endOfFlight = true;
		}

		if (!this.endOfFlight)
		{
			this.currentVelocity = this.initialVelocity + this.acceleration * this.timeAlive;
		}
		else
		{
			this.spellEnd();
			this.currentVelocity = 0f;
		}
		if ((numBounces > 0) && (Time.time > nextRayCheck)) {
			nextRayCheck = Time.time + timeBetweenChecks;
			checkForObjects ();
		}
		this.calculateVelocity();
		/*
		if (this.numBounces > 0)
		{
			Ray ray = new Ray(base.transform.position, base.transform.forward);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit) && raycastHit.distance < 1f)
			{
				float num = Vector3.Dot(base.GetComponent<Rigidbody>().velocity.normalized, raycastHit.normal);
				base.transform.rotation = Quaternion.LookRotation(base.GetComponent<Rigidbody>().velocity.normalized - 2f * num * (raycastHit.normal / Mathf.Pow(raycastHit.normal.magnitude, 2f)));
				this.numBounces -= 1f;
			}
		}
		*/
	}

	private void calculateVelocity()
	{
		
		base.GetComponent<Rigidbody>().velocity = 
			new Vector3(this.currentVelocity * Mathf.Sin(base.transform.eulerAngles.magnitude * Mathf.Deg2Rad), 0f, this.currentVelocity * Mathf.Cos(base.transform.eulerAngles.magnitude * Mathf.Deg2Rad));
	}

	private void bounce()
	{
		if (numBounces > 0) {

			//Ray ray;
			RaycastHit hit;

			// 1.0f used to be 2 *sCol.radius
			// sCol.radius was 0.5
			// might not have had anything to do with it, might have jsut been a coincidence
			float theta = Mathf.PI / (1.0f /raySpacing);
			float shortest = 10f;
			Vector3 normal = Vector3.one;
			float xPos;
			float yPos;
			Vector3 rayOrigin;

			// 2 * sCol.radius
			// iterate through every ray
			for (int i = 1; i <= (int)(1.0f / raySpacing); i++)
			{
				/*
				float num = this.sCol.radius - this.raySpacing * (float)i;
				float z = Mathf.Sqrt(this.sCol.radius * this.sCol.radius - num * num);
				ray = new Ray(new Vector3(num, 0f, z), Vector3.forward);
				Debug.DrawRay(base.transform.position + new Vector3(num, 0f, z), Vector3.forward, Color.red);
				*/

				// 0.5f used to be sCol.radius
				// sCol.radius was 0.5
				// I think I want 0.5 here, it just happened that sCol.radius was also 0.5 so it worked
				xPos = trans.localScale.x * 0.5f * Mathf.Cos (theta * i - transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
				yPos = trans.localScale.z * 0.5f * Mathf.Sin (theta * i - transform.rotation.eulerAngles.y * Mathf.Deg2Rad);

				rayOrigin = transform.position + new Vector3 (xPos, 0, yPos);
				//Debug.Log (rayOrigin);
				Debug.DrawRay (rayOrigin, rbody.velocity.normalized, Color.red);

				if(Physics.Raycast(rayOrigin, rbody.velocity.normalized, out hit))
				{
					if (hit.distance < shortest) {
						shortest = hit.distance;
						normal = hit.normal;
					}
				}
			}

			//float dotResult;
			Vector3 crossResult;

			crossResult = rbody.velocity.normalized - (2 * Vector3.Dot (rbody.velocity.normalized, normal) / (normal.magnitude * normal.magnitude)) * normal;
			//Vector3 newDir = Vector3.RotateTowards (rbody.velocity.normalized, crossResult, Mathf.PI, 0);
			Debug.Log("crossResult: " + crossResult);
			transform.rotation = Quaternion.LookRotation (crossResult);
			if (transform.rotation.x > 0 || true) {
				Debug.Log (string.Format("X rotation: {0} crossResult: {1} hit.normal: {2} velocity: {3}", transform.rotation.x, crossResult, normal, rbody.velocity.normalized));
			}
			numBounces--;
		}
	}

	void bounceWithPrecalculatedVector()
	{
		if (bounceVector != null) {
			transform.rotation = Quaternion.LookRotation (bounceVector);
			numBounces--;
			lastNormal = Vector3.one;
		}
	}

	void calculateBounceVector(Vector3 hitNormal)
	{
		// don't compute if the normal is the same as last time
		// when you bounce, need to reset this lastNormal on the chance that you hit something with the same normal
		if (hitNormal != lastNormal) 
		{
			lastNormal = hitNormal;
			bounceVector = rbody.velocity.normalized - (2 * Vector3.Dot (rbody.velocity.normalized, hitNormal) / (hitNormal.magnitude * hitNormal.magnitude)) * hitNormal;
		}
	}

	void checkForObjects()
	{
		RaycastHit hit;
		float distance = 10;
		Vector3 shortestNormal = Vector3.one;
		// go through every ray stored
		// might change to only do one per update or something
		for(int i=0; i<rayOffsets.Length; i++)
		{
			// use the current position of the ball and the offsets for the rays to find the ray's position
			Vector3 rayOrigin = trans.position + new Vector3 (rayOffsets[i].x, 0, rayOffsets[i].y);

			// draw the ray
			Debug.DrawRay (rayOrigin, rbody.velocity.normalized, Color.red);
			// shoot ray
			if (Physics.Raycast (rayOrigin, rbody.velocity.normalized, out hit, 10, LayerMask.GetMask("Walls"))) 
			{
				// if it hits something, save distance
				// find the smallest distance
				if (hit.distance < distance) 
				{
					distance = hit.distance;
					shortestNormal = hit.normal;
				}
			}
		}
		/*
		if (distance < 0.2f) 
		{
			// if distance is very small, bounce the ball
			// already does this when it collides
			bounceWithPrecalculatedVector();
		} */
		if (distance < 10) 
		{
			// calculate the bounce vector ahead of time.
			calculateBounceVector(shortestNormal);
			// cut the time between checks in half
			// is this necessary?
			// timeBetweenChecks /= 2;
		}
	}

	void initializeRays()
	{

		float floatNumRays = (trans.localScale.x / raySpacing);
		int numberOfRays = (int)floatNumRays;
		rayOffsets = new Vector2[numberOfRays];

		// scale / spacing = number of rays
		// theta is the part between each ray.
		// Want number of rays - 1 spaces
		// for 5 rays, there are 4 spaces between
		float theta = Mathf.PI / ((trans.localScale.x / raySpacing) -1);
		float xPos, yPos;

		for (int i = 0; i < numberOfRays; i++) 
		{
			xPos = trans.localScale.x * 0.5f * Mathf.Cos (theta * i - trans.rotation.eulerAngles.y * Mathf.Deg2Rad);
			yPos = trans.localScale.z * 0.5f * Mathf.Sin (theta * i - trans.rotation.eulerAngles.y * Mathf.Deg2Rad);

			rayOffsets [i] = new Vector2 (xPos, yPos);
		}
	}

	public override void castSpell(GameObject player)
	{
		// use base spell to do mana costs, etc.
		base.castSpell(player);
		this.currentVelocity = this.initialVelocity;
		this.calculateVelocity();
	}

	private void spellEnd()
	{
		if (this.explodesAtEnd && !this.exploded)
		{
			this.explode();
		}
		UnityEngine.Object.Destroy(base.gameObject, 1f);
	}

	private void explode()
	{
		if (this.exploded)
		{
			return;
		}
		this.exploded = true;
		// AreaEffect could have a different name
		// should AreaEffect == 2 make the ball to size of 2 or multiply it by 2?
		// setting it to 2 makes some sense, but when I made WaterBall, I thought it multiplied it by 2
		base.transform.localScale = Vector3.one * this.AreaEffect;
	}


	public override void hitHealth(GameObject hit, Spell thisSpell)
	{
		// The target that gets hit will only take the initial damage, not the explosion damage
		// Targets will also take damage from walking in to the explosion, not just from being there when it explodes
		if (exploded) {
			hit.GetComponent<Health> ().TakeDamage (this.explosionDamage, thisSpell);
		} else {
			hit.GetComponent<Health> ().TakeDamage (this.damage, thisSpell);
		}
		endOfFlight = true;
	}

	public override void hitWall(GameObject hit)
	{
		if (numBounces > 0) {
			// this will fail if vector hasn't been precalculated
			// like if a wall appears in between updates
			bounceWithPrecalculatedVector ();
		} else if (explodesOnImpact) {
			this.endOfFlight = true;
			explode ();
		} else {
			this.endOfFlight = true;
		}
	}



	public override void hitSpell(GameObject hit)
	{
		// if the spell I hit is weak to my type
		if (hit.GetComponent<Spell> ().weaknessType == this.GetComponent<Spell> ().type) {
			Destroy (hit);
		} else if (this.GetComponent<Spell> ().weaknessType == hit.GetComponent<Spell> ().type) {
			Destroy (this.gameObject);
		}
	}

	public float getTimeAlive()
	{
		return timeAlive;
	}

	/* old OnTriggerEnter
	public virtual void OnTriggerEnter(Collider col)
	{
		GameObject gameObject = col.gameObject;
		Health health = gameObject.GetComponent<Health>();
		if (col.tag == "Wall") {
			if (numBounces > 0) {
				bounce ();
			} else if (explodesOnImpact) {
				this.endOfFlight = true;
				explode ();
			}
		}
		// this all needs to be refactored to not have so many loopholes
		if (health != null)
		{
			this.endOfFlight = true;
			// doesn't even check if it shoud explode
			this.explode();
			if (this.exploded)
			{
				health.TakeDamage(this.explosionDamage);
			}
			else
			{
				health.TakeDamage(this.damage);
			}
			UnityEngine.Object.Destroy(base.gameObject, 2f);
		}
	}
	*/
}
