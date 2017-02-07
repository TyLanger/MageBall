using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	public MeshCollider meshCollider;
	Mesh mesh;

	int width = 100;
	int height = 100;

	/// <summary>
	/// Makes the verts.
	/// 
	/// </summary>
	/// <returns>The verts.</returns>
	/// <param name="_width">Width.</param>
	/// <param name="_height">Height. is the 2D height of the map</param>
	/// <param name="scale">Scale.</param>
	public Vector3[] makeVerts(int _width, int _height, float scale)
	{
		Vector3[] verts = new Vector3[_width * _height];
		int vertIndex = 0;

		for (int x = 0; x < _width; x++) {
			for (int y = 0; y < _height; y++) {
				float perlinNoise = Mathf.PerlinNoise (x/scale, y/scale);

				verts [vertIndex] = new Vector3(x, perlinNoise, y);
				vertIndex++;
			}
		}

		return verts;
	}


	/// Makes the mesh.
	public void makeMesh()
	{
		// mesh
		mesh = new Mesh ();
		Vector3[] verts = new Vector3[width * height];
		int[] triangles = new int[(width - 1) * (height - 1) * 6];
		int triangleIndex = 0;
		int vertexIndex = 0;
		Vector2[] uvs = new Vector2[width * height];

		// texture
		Texture2D texture = new Texture2D (width, height);
		Color[] colourMap = new Color[width * height];

		verts = makeVerts (width, height, 10);

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				float depth = verts[vertexIndex].y;

				uvs [vertexIndex] = new Vector2 (y / (float)height, x / (float)width);

				/*
				if (depth < 0.2) {
					colourMap [vertexIndex] = Color.black;
				} else if (depth < 0.4) {
					colourMap [vertexIndex] = Color.gray;
				} else if (depth < 0.6) {
					colourMap [vertexIndex] = Color.blue;
				} else if (depth < 0.8) {
					colourMap [vertexIndex] = Color.green;
				} else {
					colourMap [vertexIndex] = Color.red;
				}*/

				colourMap [vertexIndex] = Color.Lerp (Color.gray, Color.green, depth);

				if (x < (width - 1) && y < (height - 1)) {
					triangles [triangleIndex] = vertexIndex;
					triangles [triangleIndex+1] = vertexIndex + width + 1;
					triangles [triangleIndex+2] = vertexIndex + width;
					triangleIndex += 3;

					triangles [triangleIndex] = vertexIndex + width + 1;
					triangles [triangleIndex+1] = vertexIndex;
					triangles [triangleIndex+2] = vertexIndex + 1;
					triangleIndex += 3;
				}
				vertexIndex++;
			}
		}

		mesh.vertices = verts;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals ();

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colourMap);
		texture.Apply ();

		meshFilter.sharedMesh = mesh;
		meshRenderer.sharedMaterial.mainTexture = texture;

		meshCollider.sharedMesh = mesh;


	}

	public void warpMesh()
	{
		int vertIndex = 0;
		Vector3[] newVerts = new Vector3[width * height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				// curve the mesh on itself
				// first line is to map z to y
				// y is 0 to height
				// want z to be from 0 to height to 0 over the same domain
				float z = 2 *y - (height-1);
				// change z so it isn't so linear
				// this makes it look like a curve
				// the 10 is just a scaling factor
				z = ((width-1)*(width-1) - z*z)/10f;

				newVerts[vertIndex] = new Vector3(mesh.vertices[vertIndex].x, mesh.vertices [vertIndex].y + (float)y*y/height, z);

				vertIndex++;
			}
		}

		mesh.vertices = newVerts;
		mesh.RecalculateNormals ();
		// need to call these to update the mesh
		meshFilter.sharedMesh = mesh;
		meshCollider.sharedMesh = mesh;

	}

	public void Start()
	{
		Random.InitState (1);
		makeMesh ();
		//warpMesh ();
		//meshRenderer.sharedMaterial
	}
}
