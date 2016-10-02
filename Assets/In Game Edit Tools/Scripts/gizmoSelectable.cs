using UnityEngine;
using System.Collections;

public class gizmoSelectable : MonoBehaviour {

	[HideInInspector]
	public bool selected = false;
	public bool showHighlight = true;
	public float wireframeSize = 0.06f;
	public Color highlightColor = Color.yellow;
	[HideInInspector]
	public bool highlighted;
	bool gravity;
	Transform[] child;
	public bool ignoreChildTransform = false;

	void Start () {
		if (gameObject.CompareTag ("Untagged")) {
			gameObject.tag = gizmoScript.GS.tagToFind;
		}
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) { // Selects object
			RaycastHit hit;
			bool hited = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit);
			if (hited && gizmoScript.GS.selected == null) { //Does not let to select, while working with gizmo
				if (Input.GetKey (KeyCode.LeftControl)) {
					if (hit.transform == transform) { //Adds to selection if left ctrl is pressed
						selected = !selected;
					}
				} else {
					if (hit.transform == transform) { //Selects only one object in the scene
						selected = true;
					} else {
						selected = false;
					}
				}
			} else if (!hited && gizmoScript.GS.selected == null) {
				if (!Input.GetKey (KeyCode.LeftControl) && selected == true) {
					selected = false;
				}
			}
		}
		if (showHighlight == true && selected == true && highlighted == false) {
			foreach (Transform obj in transform) {
				if (obj.tag == "Wire") {
					Destroy (obj.gameObject);
				}
			}
			if (ignoreChildTransform) {
				if (child != null) {
					foreach (Transform obj in child) {
						obj.parent = transform;
					}
					child = null;
				}
			}
			drawWireframe ();
		}
		if (showHighlight == false || selected == false) {
			foreach (Transform obj in transform) {
				if (obj.tag == "Wire") {
					Destroy (obj.gameObject);
				}
			}if (ignoreChildTransform) {
				if (child != null) {
					foreach (Transform obj in child) {
						obj.parent = transform;
					}
					child = null;
				}
			}
			highlighted = false;
		}
		if (highlighted) {
			if (GetComponent<Rigidbody> ()) {
				if (GetComponent<Rigidbody> ().useGravity && !gravity) {
					gravity = true;
				}
				if (gravity && GetComponent<Rigidbody> ().useGravity) { 
					GetComponent<Rigidbody> ().useGravity = false;
					GetComponent<Rigidbody> ().angularDrag += 100f;
					GetComponent<Rigidbody> ().drag += 100f;
				}
			}
		} else {
			if (GetComponent<Rigidbody> () && gravity && !GetComponent<Rigidbody> ().useGravity) {
				GetComponent<Rigidbody> ().useGravity = true;
				GetComponent<Rigidbody> ().angularDrag -= 100f;
				GetComponent<Rigidbody> ().drag -= 100f;
			}
		}
		if (transform.hasChanged && highlighted) {
			foreach (Transform obj in transform) {
				if (obj.tag == "Wire") {
					obj.GetComponent<highlight> ().obj = GetComponent<MeshFilter> ().mesh.vertices;
					obj.GetComponent<highlight> ().updateWires ();
				}
			}
			transform.hasChanged = false;
		}
	}

	void drawWireframe () {
		if (ignoreChildTransform) {
			child = new Transform[transform.childCount];
			int numb = 0;
			foreach (Transform obj in transform) {
				child [numb] = obj;
				numb++;
			}
			foreach (Transform obj in child) {
				obj.parent = null;
			}
		}
		if (transform.GetComponent<MeshFilter> ()) {
			GameObject obj;
			LineRenderer lineRenderer;
			Mesh mesh = transform.GetComponent<MeshFilter> ().mesh;
			Vector3[] lines = mesh.vertices;
			int[] triang = mesh.triangles;
			for (int i = 0; i < triang.Length / 3; i++) {
				Vector3 p1, p2, p3;
				p1 = lines [triang [i * 3]];
				p2 = lines [triang [i * 3 + 1]];
				p3 = lines [triang [i * 3 + 2]];
				p1 = transform.TransformPoint (p1);
				p2 = transform.TransformPoint (p2);
				p3 = transform.TransformPoint (p3);
				obj = new GameObject ();
				lineRenderer = obj.AddComponent<LineRenderer> ();
				obj.transform.parent = transform;
				lineRenderer.material = new Material (Shader.Find ("Particles/Multiply (Double)"));
				lineRenderer.SetColors (highlightColor, highlightColor);
				lineRenderer.SetWidth (wireframeSize, wireframeSize);
				lineRenderer.SetVertexCount (3);
				lineRenderer.SetPosition (0, p1);
				lineRenderer.SetPosition (1, p2);
				lineRenderer.SetPosition (2, p3);
				highlight high = obj.AddComponent<highlight> ();
				high.p1 = triang [i * 3];
				high.p2 = triang [i * 3 + 1];
				high.p3 = triang [i * 3 + 2];
				obj.transform.tag = "Wire";
			}
		}
		highlighted = true;
	}

}
