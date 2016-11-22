using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class PreviewPanelDragHandler : PanelDragHandler,
IBeginDragHandler, IEndDragHandler,IDragHandler {
	public PreviewMenu menu;

    new public void OnBeginDrag(PointerEventData eventData) {
//		menu.enableUpdateViewport = true;
		base.OnBeginDrag(eventData);
    }
    new public void OnEndDrag(PointerEventData eventData)
    {
//		menu.enableUpdateViewport = false;
		base.OnEndDrag (eventData);
    }
}
