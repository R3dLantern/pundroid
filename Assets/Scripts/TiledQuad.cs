using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TiledQuad : MonoBehaviour {
	[SerializeField] private float textureWidthToUnits = 1;
	[SerializeField] private float textureHeightToUnits = 1;
	
	void Start() {
		UpdateTiling();
	}
	
	public void UpdateTiling() {
		float myWidth = transform.localScale.x;
		float myHeight = transform.localScale.y;
		
		int quadColumns = (int)Mathf.Ceil(myWidth / textureWidthToUnits);
		int quadRows = (int)Mathf.Ceil(myHeight / textureHeightToUnits);
		
		float regularColumnWidth, regularRowHeight, endColumnWidth, topRowHeight, endColumnU, topRowV;
		
		if (quadColumns == 1) {
			endColumnWidth = 1;
			regularColumnWidth = 0;
			endColumnU = myWidth / textureWidthToUnits;
		}
		else {
			float leftoverWidth = myWidth % textureWidthToUnits;
			if (leftoverWidth == 0) {
				regularColumnWidth = (myWidth / quadColumns) / myWidth;
				endColumnWidth = regularColumnWidth;
				endColumnU = 1;
			}
			else {
				regularColumnWidth = ((myWidth - leftoverWidth) / (quadColumns - 1)) / myWidth;
				endColumnWidth = leftoverWidth / myWidth;
				endColumnU = endColumnWidth / regularColumnWidth;
			}
		}
		
		if (quadRows == 1) {
			topRowHeight = 1;
			regularRowHeight = 0;
			topRowV = myHeight / textureHeightToUnits;
		}
		else {
			float leftoverHeight = myHeight % textureHeightToUnits;
			if (leftoverHeight == 0) {
				regularRowHeight = (myHeight / quadRows) / myHeight;
				topRowHeight = regularRowHeight;
				topRowV = 1;
			}
			else {
				regularRowHeight = ((myHeight - leftoverHeight) / (quadRows - 1)) / myHeight;
				topRowHeight = leftoverHeight / myHeight;
				topRowV = topRowHeight / regularRowHeight;
			}
		}
		
		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Vector2> uvs = new List<Vector2>();
		
		for (int y = 0; y < quadRows; y++) {
			float quadHeight = (y == quadRows - 1) ? topRowHeight : regularRowHeight;
			float v = (y == quadRows - 1) ? topRowV : 1;
			
			for (int x = 0; x < quadColumns; x++) {
				float quadWidth = (x == quadColumns - 1) ? endColumnWidth : regularColumnWidth;
				float u = (x == quadColumns - 1) ? endColumnU : 1;
				
				vertices.Add(new Vector3(x * regularColumnWidth - 0.5f, y * regularRowHeight - 0.5f, 0));
				vertices.Add(new Vector3(x * regularColumnWidth - 0.5f, y * regularRowHeight + quadHeight - 0.5f, 0));
				vertices.Add(new Vector3(x * regularColumnWidth + quadWidth - 0.5f, y * regularRowHeight - 0.5f, 0));
				vertices.Add(new Vector3(x * regularColumnWidth + quadWidth - 0.5f, y * regularRowHeight + quadHeight - 0.5f, 0));
				
				// texture coordinates
				uvs.Add(new Vector2(0, 0));
				uvs.Add(new Vector2(0, v));
				uvs.Add(new Vector2(u, 0));
				uvs.Add(new Vector2(u, v));
				
				// triangle indices should match up with vertex indices (the order which vertices are added matters)
				int bottomLeftVertexIndex = (y * quadColumns * 4) + (x * 4);
				
				// top left triangle
				triangles.Add(bottomLeftVertexIndex);
				triangles.Add(bottomLeftVertexIndex + 1);
				triangles.Add(bottomLeftVertexIndex + 3);
				
				// bottom right triangle
				triangles.Add(bottomLeftVertexIndex + 3);
				triangles.Add(bottomLeftVertexIndex + 2);
				triangles.Add(bottomLeftVertexIndex);
			}
		}
		
		Mesh m = new Mesh();
		
		m.vertices = vertices.ToArray();
		m.triangles = triangles.ToArray();
		m.uv = uvs.ToArray();
		
		m.RecalculateNormals();
		
		GetComponent<MeshFilter>().mesh = m;
	}
}
