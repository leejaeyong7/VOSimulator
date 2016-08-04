using UnityEngine;
using System.Collections;

public class ObjectEvents : MonoBehaviour {
    public bool isObjectDragged;
    private Vector3 screenPoint;
	private Vector3 raycasthit;
    private Vector3 offset;
	private Ray ray;
    void OnMouseDown()
    {
		// copy object iff is on preview
		if (this.gameObject.transform.parent.name == "PreviewBG") {
			GameObject copy = Instantiate (this.gameObject);
			copy.transform.SetParent (this.gameObject.transform.parent);
			copy.transform.position = this.gameObject.transform.position;
			copy.transform.rotation = this.gameObject.transform.rotation;
			copy.transform.localScale = this.gameObject.transform.localScale;
		}
		print ("Number of mesh triangles: " + this.gameObject.GetComponent<MeshFilter> ().mesh.triangles.Length.ToString ());
		isObjectDragged = true;

		// get local rotation ( straight rotation )
		Quaternion locRot = this.gameObject.transform.localRotation;
		Vector3 locScale = this.gameObject.transform.localScale;

		// set parent to Terrain
		this.gameObject.transform.SetParent (Terrain.activeTerrain.transform);
		this.gameObject.transform.localRotation = locRot;
		this.gameObject.transform.localScale = locScale;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);


        offset = transform.position - Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// create a plane at 0,0,0 whose normal points to +Y:
		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
		float distance = 0; 
		// if the ray hits the plane...
		if (hPlane.Raycast(ray, out distance)){
			// get the hit point:
			transform.position = ray.GetPoint(distance);
		}
    }
    void OnMouseUp()
    {
        isObjectDragged = false;
        if(GetComponentInParent<ObjectMenuEvents>())
        {
            GetComponentInParent<ObjectMenuEvents>().removeCurrObject();
        }
    }
}
