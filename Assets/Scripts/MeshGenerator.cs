using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	int width = 10;
	int height = 10;



	public void makeMesh()
	{
		// mesh
		Mesh mesh = new Mesh ();
		Vector3[] verts = new Vector3[width * height];
		int[] triangles = new int[(width - 1) * (height - 1) * 6];
		int triangleIndex = 0;
		int vertexIndex = 0;
		Vector2[] uvs = new Vector2[width * height];

		// texture
		Texture2D texture = new Texture2D (width, height);
		Color[] colourMap = new Color[width * height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				float depth = (float)Random.Range (0, 10) / 10f;
				verts [vertexIndex] = new Vector3 (x, depth, y);
				uvs [vertexIndex] = new Vector2 (y / (float)height, x / (float)width);

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
				}

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



	}

	public void Start()
	{
		makeMesh ();
		//meshRenderer.sharedMaterial
	}
}
