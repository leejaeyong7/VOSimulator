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
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class CameraObject : MonoBehaviour {
	float fov;
	float aspect;
	float focalLength;
	float noiseLevel;

	private Vector3 raycasthit;
	private Vector3 raycastDiff;

//	private gizmoSelectable gz;

	// Use this for initialization
	void Start () {
		gameObject.tag = "Gizmo";
		gameObject.AddComponent<gizmoSelectable> ();
//		gz  = this.gameObject.GetComponent<gizmoSelectable>();
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetMouseButtonDown (0)) {
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
