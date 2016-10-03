using UnityEngine;
using System.Collections;

public class globalGrid2D : MonoBehaviour {

	public float cellSize = 1f;
	public float wireSize = 0.06f;
	public Material material;
	public float outterCellSize = 10f;
	public float outterWireSize = 0.2f;
	public Material outterMaterial;
	public bool outterCell = true;
	public float gridSize = 12f;
	public bool endLine = true;
	public GameObject lineObj;
	public bool regenerateGrid = false;

	void Start () {
		generateGrid ();
	}

	void Update () {
		if (regenerateGrid == true) {
			foreach (Transform obj in transform) {
				Destroy (obj.gameObject);
			}
			generateGrid ();
			regenerateGrid = false;
		}
	}

	void generateGrid () {
		int cells = (int)(Mathf.Round (gridSize / cellSize));
		for (int i = 0; i <= cells; i++) {
			if (i == 0 && endLine == true) {
				drawLines (i, cells);
			} else if (i == cells && endLine == true) {
				drawLines (i, cells);
			} else if (i != 0 && i != cells) {
				drawLines (i, cells);
			}
		}
		if (outterCell == true) {
			int outterCells = (int)(Mathf.Round (gridSize / outterCellSize));
			for (int i = 0; i <= outterCells; i++) {
				if (i == 0 && endLine == true) {
					drawOutterLines (i, outterCells);
				} else if (i == outterCells && endLine == true) {
					drawOutterLines (i, outterCells);
				} else if (i != 0 && i != outterCells) {
					drawOutterLines (i, outterCells);
				}
			}
		}
	}

	void drawLines (int i, int cells) {
		GameObject obj = (GameObject)Instantiate (lineObj);
		obj.transform.parent = transform;
		obj.transform.GetComponent<MeshRenderer>().material = material;
		obj.transform.position = new Vector3 (-cellSize * cells / 2f, 0, -cellSize * cells / 2f + cellSize * i);
		obj.transform.LookAt (new Vector3 (cellSize * cells / 2f, 0, -cellSize * cells / 2f + cellSize * i));
		obj.transform.localScale = new Vector3 (wireSize, wireSize, Vector3.Distance (obj.transform.position, new Vector3 (cellSize * cells / 2f, 0, -cellSize * cells / 2f + cellSize * i)));
		obj = (GameObject)Instantiate (lineObj);
		obj.transform.parent = transform;
		obj.transform.GetComponent<MeshRenderer>().material = material;
		obj.transform.position = new Vector3 (-cellSize * cells / 2f + cellSize * i, 0, -cellSize * cells / 2f);
		obj.transform.LookAt (new Vector3 (-cellSize * cells / 2f + cellSize * i, 0, cellSize * cells / 2f));
		obj.transform.localScale = new Vector3 (wireSize, wireSize, Vector3.Distance (obj.transform.position, new Vector3 (-cellSize * cells / 2f + cellSize * i, 0, cellSize * cells / 2f)));
	}

	void drawOutterLines (int i, int cells) {
		GameObject obj = (GameObject)Instantiate (lineObj);
		obj.transform.parent = transform;
		obj.transform.GetComponent<MeshRenderer>().material = outterMaterial;
		obj.transform.position = new Vector3 (-outterCellSize * cells / 2f, 0, -outterCellSize * cells / 2f + outterCellSize * i);
		obj.transform.LookAt (new Vector3 (outterCellSize * cells / 2f, 0, -outterCellSize * cells / 2f + outterCellSize * i));
		obj.transform.localScale = new Vector3 (outterWireSize, outterWireSize, Vector3.Distance (obj.transform.position, new Vector3 (outterCellSize * cells / 2f, 0, -outterCellSize * cells / 2f + outterCellSize * i)));
		obj = (GameObject)Instantiate (lineObj);
		obj.transform.parent = transform;
		obj.transform.GetComponent<MeshRenderer>().material = outterMaterial;
		obj.transform.position = new Vector3 (-outterCellSize * cells / 2f + outterCellSize * i, 0, -outterCellSize * cells / 2f);
		obj.transform.LookAt (new Vector3 (-outterCellSize * cells / 2f + outterCellSize * i, 0, outterCellSize * cells / 2f));
		obj.transform.localScale = new Vector3 (outterWireSize, outterWireSize, Vector3.Distance (obj.transform.position, new Vector3 (-outterCellSize * cells / 2f + outterCellSize * i, 0, outterCellSize * cells / 2f)));
	}

}
