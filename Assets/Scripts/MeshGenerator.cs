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
	float scale = 10;

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

	void makeTunnelHeights(ref int[,] low, ref int[,] high, int lowPoint, int highPoint)
	{
		
		int lowWidth = low.GetLength(0);
		int lowHeight = low.GetLength (1);

		for (int x = 0; x < lowWidth; x++) {
			for (int y = 0; y < lowHeight; y++) {
				low [x, y] = lowPoint * (x - (lowWidth / 2))*(x - (lowWidth / 2));
				high [x, y] = highPoint * (x - (lowWidth / 2));
			}
		}

	}


	Vector3[] makeVertsCave(int _width, int _height, int[,] heights)
	{
		Vector3[] verts = new Vector3[_width * _height];
		int vertIndex = 0;

		for (int x = 0; x < _width; x++) {
			for (int y = 0; y < _height; y++) {
				verts [vertIndex] = new Vector3 (x, heights [x, y], y);
				vertIndex++;
			}
		}

		return verts;
	}
	/*
	int[] makeUVS()
	{
		// placeholder
		return new int[5];
	}
	*/

	public void makeMesh(Vector3[] lowVerts, Vector3[] highVerts)
	{
		mesh = new Mesh ();
		int[] triangles = new int[((width*2) - 1) * (height - 1) * 6];
		Debug.Log (((width * 2) - 1) * (height - 1) * 6);
		int triangleIndex = 0;
		int vertexIndex = 0;
		Vector2[] uvs = new Vector2[width * height * 2];
		Vector3[] finalVerts = new Vector3[width * height * 2];

		// texture
		Texture2D texture = new Texture2D(width*2, height);
		Color[] colourMap = new Color[width * 2 * height];

		// create the lower triangles
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				uvs [vertexIndex] = new Vector2 (y / (float)height, x / (float)width);

				colourMap [vertexIndex] = Color.gray;

				if (x < (width - 1) && y < (height - 1)) {
					triangles [triangleIndex] = vertexIndex;
					// 2 width because it is adding together 2 vert arrays
					triangles [triangleIndex + 1] = vertexIndex + 2 * width + 1;
					triangles [triangleIndex + 2] = vertexIndex + 2 * width;
					triangleIndex += 3;

					triangles [triangleIndex] = vertexIndex + 2 * width + 1;
					triangles [triangleIndex + 1] = vertexIndex;
					triangles [triangleIndex + 2] = vertexIndex + 1;
					triangleIndex += 3;
				}

				finalVerts [vertexIndex] = lowVerts [vertexIndex];

				vertexIndex++;
			}
		}
		Debug.Log (vertexIndex);
		// tie the lower and upper vertices together
		for (int h = 0; h < (height-1); h++) {
			triangles [triangleIndex] = vertexIndex;
			triangles [triangleIndex + 1] = vertexIndex + 2*width + 1;
			triangles [triangleIndex + 2] = vertexIndex + 2*width;
			triangleIndex += 3;

			triangles [triangleIndex] = vertexIndex + 2*width + 1;
			triangles [triangleIndex + 1] = vertexIndex;
			triangles [triangleIndex + 2] = vertexIndex + 1;
			triangleIndex += 3;
		}

		Debug.Log ("Vertex Index: " + vertexIndex);
		for (int x2 = (width-1); x2 >= 0; x2--) {
			for (int y2 = (width-1); y2 >= 0; y2--) {

				uvs [vertexIndex] = new Vector2 (y2 / (float)height, x2 / (float)width);

				colourMap [vertexIndex] = Color.gray;

				triangles [triangleIndex] = vertexIndex;
				triangles [triangleIndex + 1] = vertexIndex + 2*width + 1;
				triangles [triangleIndex + 2] = vertexIndex + 2*width;
				triangleIndex += 3;

				triangles [triangleIndex] = vertexIndex + 2*width + 1;
				triangles [triangleIndex + 1] = vertexIndex;
				triangles [triangleIndex + 2] = vertexIndex + 1;
				triangleIndex += 3;


				finalVerts [vertexIndex] = highVerts [vertexIndex - (width*height)];
				vertexIndex++;
			}
		}

		mesh.vertices = finalVerts;
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

	/// Makes the mesh.
	public void makeMesh(Vector3[] verts)
	{
		// mesh
		mesh = new Mesh ();
		//Vector3[] verts = new Vector3[width * height];
		int[] triangles = new int[(width - 1) * (height - 1) * 6];
		int triangleIndex = 0;
		int vertexIndex = 0;
		Vector2[] uvs = new Vector2[width * height];

		// texture
		Texture2D texture = new Texture2D (width, height);
		Color[] colourMap = new Color[width * height];

		//verts = makeVerts (width, height, 10);

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				float depth = verts[vertexIndex].y;

				uvs [vertexIndex] = new Vector2 (y / (float)height, x / (float)width);

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

				// this stretches the mesh out on the edges
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

		int[,] lowHeights = new int[10,10];
		int[,] highHeights = new int[10, 10];
		makeTunnelHeights (ref lowHeights, ref highHeights, -5, 5);
		Vector3[] vertices = makeVertsCave (10, 10, lowHeights);
		Vector3[] highVertices = makeVertsCave (10, 10, highHeights);
		//Vector3[] vertices = makeVerts (width, height, scale);
		makeMesh (vertices, highVertices);
		// don't have 2 mesh renderers to do this....
		//makeMesh (highVertices);
		//warpMesh ();
		//meshRenderer.sharedMaterial
	}
}
