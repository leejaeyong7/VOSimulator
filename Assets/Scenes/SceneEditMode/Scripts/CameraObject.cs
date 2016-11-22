/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : CameraObject.cs
 * @brief      : Event handler for File menu
 * Copyright (c) Jae Yong Lee / UIUC Summer 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class CameraObject : MonoBehaviour{
	public IOHandler io;
	float fov;
	float aspect;
	float focalLength;
	float noiseLevel;
	public PreviewMenu pm;
	private Vector3 raycasthit;
	private Vector3 raycastDiff;
	private bool focusable;
	private bool focused ;

//	private gizmoSelectable gz;

	// Use this for initialization
	void Start () {
		gameObject.tag = "Gizmo";
		gameObject.AddComponent<gizmoSelectable> ();
		focusable = false;
		focused = false;
//		gz  = this.gameObject.GetComponent<gizmoSelectable>();
	}
	void OnMouseEnter(){
		focusable = true;
	}
	void OnMouseExit(){
		focusable = false;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
//			if (!io.isGUIClicked ()) {
			focused = focusable;
			if (focused) {
				pm.attachedObject = gameObject;
				pm.updateViewport ();
				pm.gameObject.SetActive (true);
				Camera cam = gameObject.GetComponent<Camera> ();
				cam.enabled = true;
				cam.rect = pm.viewport;
			} else {
				pm.attachedObject = null;
				gameObject.GetComponent<Camera> ().enabled = false;
				pm.gameObject.SetActive (false);
			}
//			}
		}
//			cameraPanel.focusedObject = null;
//			if (gz.selected) {
//				cameraPanel.focusedObject = this;
//			}
//			cameraPanel.setupLocalMenu ();
//		}
	}

	//--------------------------------------------------------------------//
	//                    PUBLIC FUNCTION DEFINITIONS                     //
	//--------------------------------------------------------------------//
	//--------------------------------------------------------------------//
	//                  END PUBLIC FUNCTION DEFINITIONS                   //
	//--------------------------------------------------------------------//
	//--------------------------------------------------------------------//
	//                    PRIVATE FUNCTION DEFINITIONS                    //
	//--------------------------------------------------------------------//
	//--------------------------------------------------------------------//
	//                  END PRIVATE FUNCTION DEFINITIONS                  //
	//--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
