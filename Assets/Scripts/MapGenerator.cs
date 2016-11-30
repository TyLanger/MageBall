using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public float maxMapX;
	public float maxMapZ;

	public float circleRad = 4;
	public bool spreadOverlap = true;

	public GameObject wall;
	public GameObject floor;

	// Use this for initialization
	void Start () {
		//makeSquareRoom ();
		makeCircleOfCubes();
	}

	void makeCircleOfCubes()
	{
		// only works for:
		// walls that are boxes
		// &&
		// using the x size of the box
		float boxSize = wall.GetComponent<BoxCollider> ().size.x;
		//float h = Mathf.Sqrt( circleRad * circleRad - (boxSize / 2) * (boxSize / 2) );

		float theta;
		float phi;
		float a;
		float numDivisions;

		theta = 2 * Mathf.Asin (boxSize / (2*circleRad)) * Mathf.Rad2Deg;
		numDivisions = (360 / theta);

		// This will shrink the circle of boxes down to spread the overlap evenly
		// A circle of given radius r might accomodate 20.3 boxes.
		// This shrinks the circle so that exactly 20 boxes fit
		// each box will overlap its neighbours by a small amount instead of one box overlapping a lot
		if (spreadOverlap) {
			float extraDeg = 360 - theta * (int)numDivisions;
			theta += extraDeg / (int)numDivisions;
			// theta has changed so need to change the radius of the circle
			circleRad = boxSize * Mathf.Sin(((180 - theta) / 2)*Mathf.Deg2Rad) / Mathf.Sin (theta * Mathf.Deg2Rad);
		}


		phi = 90 - theta / 2;
		a = Mathf.Sin (phi * Mathf.Deg2Rad) * circleRad;

		// This will make a circle using cubes. Each should be touching corner to corner.
		// However, there may be some overlap of the last 2 cubes



		for (int i = 0; i < numDivisions; i++) {
			float x = (boxSize/2 + a) * Mathf.Cos(theta*i*Mathf.Deg2Rad);
			float z = (boxSize/2 + a) * Mathf.Sin(theta*i*Mathf.Deg2Rad);
			Instantiate(wall, new Vector3(x, 0, z), Quaternion.Euler( Vector3.up * theta *i *-1));
		}

		//floor = new Plane (Vector3.up, circleRad);
		floor = GameObject.CreatePrimitive (PrimitiveType.Plane);
		floor.GetComponent<Transform> ().position = new Vector3 (0, -0.5f, 0);
		floor.GetComponent<Transform> ().localScale = new Vector3 (circleRad/5.0f, 0, circleRad/5.0f);
	}


	void makeOvalMap()
	{
		/*
		Plane floor = new Plane (Vector3.up, new Vector3(maxMapX/2, 0, maxMapZ/2));

		for(int i=0; i < (maxMapZ / wall.GetComponent<BoxCollider>().size.z) /2;	i++)
		{
			//float z = (i + (maxMapZ / 2)) % maxMapZ;
			float z = i;
			float x = maxMapX - ((i/maxMapZ) * maxMapX);
			Instantiate(wall, new Vector3(x, 0, z), transform.rotation);
			Instantiate(wall, new Vector3(-x, 0, z), transform.rotation);
			Instantiate(wall, new Vector3(x, 0, -z), transform.rotation);
			Instantiate(wall, new Vector3(-x, 0, -z), transform.rotation);
		}
		*/




	}

	void makeSquareRoom()
	{
		for (int i = 0; i < maxMapX; i++) {
			
				
			float x = (maxMapX / 2) - i * wall.GetComponent<BoxCollider> ().size.x;
			float z = (maxMapZ / 2) - i * wall.GetComponent<BoxCollider> ().size.z;
			Instantiate (wall, new Vector3 (x, 0, transform.position.z - maxMapZ / 2), transform.rotation);
			Instantiate (wall, new Vector3 (x, 0, transform.position.z + maxMapZ / 2), transform.rotation);
			Instantiate (wall, new Vector3 (transform.position.x - maxMapX / 2, 0, z), transform.rotation);
			Instantiate (wall, new Vector3 (transform.position.x + maxMapX / 2, 0, z), transform.rotation);
		}
	}
	

}
