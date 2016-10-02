using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class JWindowHead : JGUI, IBeginDragHandler, IEndDragHandler, IDragHandler {
	// declare child UI componenets 
	Text title;
	Button collapse;

	void Start(){
		// initialize UI components
//		title = new Text ();
		title.transform.SetParent (transform);
		title.fontSize = 10;
		title.alignment = TextAnchor.MiddleCenter;
		title.color = Color.white;

//		collapse = new Button ();
		collapse.transform.SetParent (transform);
//		collapse.image = new Image ();
		collapse.image.sprite = Resources.Load<Sprite> ("DropdownArrow");
		RectTransform rect = (RectTransform)collapse.transform;
		rect.localPosition = new Vector3 (-5,0,0);
		rect.sizeDelta = new Vector2 (10,10);
	}
	//--------------------------------------------------------------------//
	//                    PUBLIC FUNCTION DEFINITIONS                     //
	//--------------------------------------------------------------------//

	// handle panel dragging functions
	public static bool isPanelDragged = false;
	public void OnBeginDrag(PointerEventData eventData)
	{
		isPanelDragged = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		float dx = eventData.delta.x;
		float dy = eventData.delta.y;
		Canvas c = GameObject.Find("Canvas").GetComponent<Canvas>();

		float scale = c.scaleFactor;

		RectTransform trans = (RectTransform)transform.parent.transform;
		Vector3 pos = trans.localPosition;
		pos.x += dx / scale;
		pos.y += dy / scale;
		trans.localPosition = pos;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isPanelDragged = false;
	}
	public void SetTitle(string name){
		title.text = name;
	}
	//--------------------------------------------------------------------//
	//                  END PUBLIC FUNCTION DEFINITIONS                   //
	//--------------------------------------------------------------------//
}
