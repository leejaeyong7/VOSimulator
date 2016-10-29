using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class JWindowHead : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
	// declare child UI componenets 
	public Text title;
	public Button collapse;
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
