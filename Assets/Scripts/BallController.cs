using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BallController : MonoBehaviour {

    public float missileSpeed;
    float angle;
    


	void Start () {
		//CmdMoveBall ();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

	public void moveBall()
	{
		angle = transform.eulerAngles.magnitude * Mathf.Deg2Rad;
		GetComponent<Rigidbody>().velocity = new Vector3(missileSpeed * Mathf.Sin(angle), 0, missileSpeed * Mathf.Cos(angle));
		//CmdMoveBall();
		Destroy(gameObject, 2.0f);
	}


	/*
	 * This doesn't appear necessary at the moment
	 * Would need to change MonoBehaviour to NetworkBehaviour for this function to work
	[Command]
    public void CmdMoveBall()
    {
		angle = transform.eulerAngles.magnitude * Mathf.Deg2Rad;
		GetComponent<Rigidbody>().velocity = new Vector3(missileSpeed * Mathf.Sin(angle), 0, missileSpeed * Mathf.Cos(angle));
    }
    */

	void OnTriggerEnter(Collider col)
	{
		var hit = col.gameObject;
		var health = hit.GetComponent<Health>();
		if (health  != null)
		{
			health.TakeDamage(10);
		}		
		Destroy (gameObject);
	}
}
