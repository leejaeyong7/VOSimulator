using UnityEngine;
using System.Collections;

public class ObjectEvents : MonoBehaviour {
    public bool isObjectDragged;
    private Vector3 screenPoint;
    private Vector3 offset;
    void OnMouseDown()
    {
        isObjectDragged = true;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        offset = transform.position - Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        PanelDragHandler.isPanelDragged = false;
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;

    }
    void OnMouseUp()
    {
        isObjectDragged = false;
        
        if(GetComponentInParent<ObjectMenuEvents>())
        {
            Debug.Log("imhere");
            GetComponentInParent<ObjectMenuEvents>().removeCurrObject();
        }
        transform.parent = Terrain.activeTerrain.transform;
    }
}
