using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	public float maxMapX;
	public float maxMapZ;

	public GameObject wall;

	// Use this for initialization
	void Start () {
		makeSquareRoom ();
	}

	void makeOvalMap()
	{
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
