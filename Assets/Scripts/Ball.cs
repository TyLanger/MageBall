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

	private bool exploded;

	private SphereCollider sCol;
	Rigidbody rbody;

	public float raySpacing = 0.5f;



	private void Start()
	{
		this.timeAlive = 0f;
		this.sCol = base.GetComponent<SphereCollider>();
		rbody = GetComponent<Rigidbody> ();

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
		
		base.GetComponent<Rigidbody>().velocity = new Vector3(this.currentVelocity * Mathf.Sin(base.transform.eulerAngles.magnitude * Mathf.Deg2Rad), 0f, this.currentVelocity * Mathf.Cos(base.transform.eulerAngles.magnitude * Mathf.Deg2Rad));
	}

	private void bounce()
	{
		if (numBounces > 0) {

			//Ray ray;
			RaycastHit hit;

			float theta = Mathf.PI / (2 *sCol.radius /raySpacing);
			float shortest = 10f;
			Vector3 normal = Vector3.one;
			float xPos;
			float yPos;
			Vector3 rayOrigin;

			for (int i = 1; i <= (int)(2 * sCol.radius / raySpacing); i++)
			{
				/*
				float num = this.sCol.radius - this.raySpacing * (float)i;
				float z = Mathf.Sqrt(this.sCol.radius * this.sCol.radius - num * num);
				ray = new Ray(new Vector3(num, 0f, z), Vector3.forward);
				Debug.DrawRay(base.transform.position + new Vector3(num, 0f, z), Vector3.forward, Color.red);
				*/

				xPos = sCol.radius * Mathf.Cos (theta * i - transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
				yPos = sCol.radius * Mathf.Sin (theta * i - transform.rotation.eulerAngles.y * Mathf.Deg2Rad);

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
			transform.rotation = Quaternion.LookRotation (crossResult);
		}

	}

	public override void castSpell()
	{
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
		base.transform.localScale = Vector3.one * this.AreaEffect;
	}

	private void OnTriggerEnter(Collider col)
	{
		GameObject gameObject = col.gameObject;
		Health health = gameObject.GetComponent<Health>();
		if (col.tag == "Wall") {
			if (numBounces > 0) {
				bounce ();
			} else if (explodesOnImpact) {
				explode ();
			}
		}
		if (health != null)
		{
			this.endOfFlight = true;
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
}
