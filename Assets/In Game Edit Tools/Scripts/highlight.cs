using UnityEngine;
using System.Collections;

public class highlight : MonoBehaviour {

	public int p1, p2, p3;
	public Vector3[] obj;

	public void updateWires () {
		LineRenderer lineRenderer = GetComponent<LineRenderer> ();
		obj [p1] = transform.parent.TransformPoint (obj [p1]);
		obj [p2] = transform.parent.TransformPoint (obj [p2]);
		obj [p3] = transform.parent.TransformPoint (obj [p3]);
		lineRenderer.SetVertexCount (3);
		lineRenderer.SetPosition (0, obj[p1]);
		lineRenderer.SetPosition (1, obj[p2]);
		lineRenderer.SetPosition (2, obj[p3]);
	}

}
