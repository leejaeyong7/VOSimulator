﻿/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : EditModeEvents.cs
 * @brief      : Event handler for Edit mode scene in VOS
 * Copyright (c) Jae Yong Lee / UIUC Summer 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEngine.EventSystems;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class PanelDragHandler : 
    MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    public static bool isPanelDragged = false;

    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
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
    //--------------------------------------------------------------------//
    //                  END PUBLIC FUNCTION DEFINITIONS                   //
    //--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
