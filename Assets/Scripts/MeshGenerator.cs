using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	public MeshCollider meshCollider;
	Mesh mesh;

	int width = 10;
	int height = 10;

	public Vector3[] makeVerts(int _width, int _height, float scale)
	{
		Vector3[] verts = new Vector3[_width * _height];
		int vertIndex = 0;
		// put some kind of noise here
		// Mathf.PerlinNoise(x, y)
		for (int x = 0; x < _width; x++) {
			for (int y = 0; y < _height; y++) {
				float perlinNoise = Mathf.PerlinNoise (x/scale, y/scale);
				//Debug.Log (perlinNoise);
				verts [vertIndex] = new Vector3(x, perlinNoise * 1 , y);
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

		verts = makeVerts (width, height, 0.225f);

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				float depth = verts[vertexIndex].y;
				//verts [vertexIndex] = new Vector3 (x, depth, y);
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

				colourMap [vertexIndex] = Color.Lerp (Color.gray, Color.green, depth/1f);

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

		//colourMap [0] = Color.yellow;
		//colourMap [1] = Color.cyan;

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

				float z = 2 *y - (height-1);

				newVerts[vertIndex] = new Vector3(mesh.vertices[vertIndex].x, mesh.vertices [vertIndex].y + (float)y*y/height, ((width-1)*(width-1) - z*z)/10f);

				vertIndex++;
			}
		}

		mesh.vertices = newVerts;
		mesh.RecalculateNormals ();
		meshFilter.sharedMesh = mesh;
		meshCollider.sharedMesh = mesh;

	}

	public void Start()
	{
		makeMesh ();
		warpMesh ();
		//meshRenderer.sharedMaterial
	}
}
