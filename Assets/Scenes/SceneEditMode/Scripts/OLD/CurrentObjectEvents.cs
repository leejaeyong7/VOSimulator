/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : CurrentObjectEvents.cs
 * @brief      : Event handler for selected Object
 * Copyright (c) Jae Yong Lee / UIUC Summer 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using System.Collections;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class CurrentObjectEvents : MonoBehaviour {
	private bool wasActive = false;
	private bool isMoved = false;
	private bool rayUpdated = false;

	private SceneModeCameraMovement cameraMovement;
	private EditModeEvents ev;
	private Vector3 raycasthit;
	private Vector3 raycastDiff;
	private Ray ray;

	//--------------------------------------------------------------------//
	//                    PUBLIC FUNCTION DEFINITIONS                     //
	//--------------------------------------------------------------------//
	//--------------------------------------------------------------------//
	//                  END PUBLIC FUNCTION DEFINITIONS                   //
	//--------------------------------------------------------------------//
	//--------------------------------------------------------------------//
	//                    PRIVATE FUNCTION DEFINITIONS                    //
	//--------------------------------------------------------------------//
	void Start(){
		ev = GameObject.Find ("EventSystem")
			.GetComponent<EditModeEvents> ();
		cameraMovement = GameObject.Find ("SceneCamera")
			.GetComponent<SceneModeCameraMovement> ();
	}
	void OnMouseDown(){
		isMoved = false;
		rayUpdated = false;
		// if this was selected already
		if (isSelected()) {
			wasActive = true;
		} else { // if not, select object
			ev.selectObject (this.gameObject);
		}
		cameraMovement.isObjectDragged = true;
	}
	void OnMouseDrag(){
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// create a plane at 0,0,0 whose normal points to +Y:
		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		// Plane.Raycast stores the distance from ray.origin
		// to the hit point in this variable:
		float distance = 0; 
		// if the ray hits the plane...
		if (hPlane.Raycast(ray, out distance)){
			// get the hit point:
			if (!rayUpdated) {
				raycasthit = ray.GetPoint (distance);
				rayUpdated = true;
			} else {
				raycastDiff = ray.GetPoint (distance) - raycasthit;
				raycasthit = ray.GetPoint (distance);
			}
			if (raycastDiff != Vector3.zero) {
				transform.position += raycastDiff;
				isMoved = true;
			}
		}
	}
	void OnMouseUp(){
		// if this was active(i.e has been active previously)
		if (isSelected() && wasActive) {
			if (!isMoved) {
				ev.unselectObject ();
			}
		}
		isMoved = false;
		wasActive = false;

		if (cameraMovement) {
			cameraMovement.isObjectDragged = false;
		}
	}
	bool isSelected(){
		if (ev && ev.selectedObject) {
			return ev.selectedObject == this.gameObject;
		} else {
			return false;
		}
	}
	//--------------------------------------------------------------------//
	//                  END PRIVATE FUNCTION DEFINITIONS                  //
	//--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//

