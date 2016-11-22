using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PreviewMenu : MonoBehaviour{
	public GameObject attachedObject;
	public GameObject previewScreen;
	public Rect viewport;
//	public bool enableUpdateViewport = true;
	// Use this for initialization
	void Start () {
		attachedObject = null;
		updateViewport ();
	}
	public void updateViewport(){
		RectTransform r = ((RectTransform)previewScreen.transform);
		Vector3 [] corners = new Vector3[4];
		r.GetWorldCorners (corners);
		Vector3 lu = Camera.main.WorldToViewportPoint (corners [0]);
		Vector3 rd = Camera.main.WorldToViewportPoint (corners [2]);
		float x = lu.x > 0 ? lu.x : 0;
		float y = lu.y > 0 ? lu.y : 0;
		float w = rd.x - x;
		float h = rd.y - y;
		viewport = new Rect( x, y, w,h);
	}
	
	// Update is called once per frame
	void Update () {
		if (attachedObject){
			Camera cam = attachedObject.GetComponent<Camera> ();
			if (cam) {
				updateViewport ();
				cam.rect = viewport;
			}
		} 

	}
}
