/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : CameraPanel.cs
 * @brief      : Event handler for trajectory settings
 * Copyright (c) Jae Yong Lee / UIUC Fall 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class CameraPanel : MenuPanel{
//	// include event system for gui event checks
//	public EditModeEvents ev;
//	// camera object prefab
//	public GameObject cameraObject;
//	public GameObject trajectory;
//
//	// dropdown for selecting menu panels
//	public Dropdown cameraMenuDropdown;
//	public NoiseMenu noiseMenu;
//	public CameraParamMenu globalCameraMenu;
//	public TrajectoryMenu trajectoryMenu;
//
//
//	// ptr to local menu
//	public LocalMenu localMenu;
//	public PreviewMenu previewMenu;
//	public gizmoScript gizmo;
//
//	// ptr to focused cameraobject
//	public GameObject focusedObject = null;
//
//	// holds all list of camera objects
//
//	// holds global camera setup values
//	public float var_x_2d = 0;
//	public float var_y_2d = 0;
//	public float var_x_3d = 0;
//	public float var_y_3d = 0;
//	public float var_z_3d = 0;
//
//
//	public float focalLength = 0;
//	public float fovy = 0;
//	public float aspect = 0;
//	public float speed = 0;
//	public float fps = 0;
//
//	// private vars for raycast on camera frustrums
//	private Ray ray;
//	private RaycastHit hit;
//
//
//	void Start(){
//		cameraMenuDropdown.onValueChanged.AddListener(delegate {
//			chooseCameraMenuType(cameraMenuDropdown);
//		});
//		cameraMenuDropdown.onValueChanged.Invoke(0);
//	}
//
//	// Update is called once per frame
//	void Update () {
//		if (Input.GetMouseButtonDown (0)) {
//			if (!ev.isGUIClicked ()) {
//				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//				if (Physics.Raycast (ray, out hit)) {
//					GameObject g = hit.transform.gameObject;
//					if (g.GetComponent<CameraObject> ()) {
//						if (focusedObject != g) {
//							focusedObject = g;
//							setupLocalMenu ();
//							setupPreviewMenu ();
//						}
//					} else if (!g.GetComponentInParent<gizmoScript>()) {
//						focusedObject = null;
//						setupLocalMenu ();
//						setupPreviewMenu ();
//					} 
//				} else {
//					focusedObject = null;
//					setupLocalMenu ();
//					setupPreviewMenu ();
//				}
//			}
//		}
//	}
//	//--------------------------------------------------------------------//
//	//                    PUBLIC FUNCTION DEFINITIONS                     //
//	//--------------------------------------------------------------------//
//	new public void Show(){
//		base.Show ();
//		MessageDispatcher.SendMessageData ("SET_STATE", "Trajectory");
//	}
//
//	public void setupLocalMenu(){
//		if (focusedObject == null) {
//			localMenu.Hide ();
//		} else {
//			localMenu.Show ();
//		}
//	}
//
//	public void setupPreviewMenu(){
//		if (focusedObject == null) {
//			if (previewMenu.attachedObject) {
//				previewMenu.attachedObject.GetComponent<Camera> ().enabled = false;
//				previewMenu.attachedObject = null;
//			}
//			previewMenu.gameObject.SetActive(false);
//		} else {
//			previewMenu.gameObject.SetActive(false);
//			if (focusedObject != previewMenu.attachedObject) {
//				if (previewMenu.attachedObject) {
//					previewMenu.attachedObject.GetComponent<Camera> ().enabled = false;
//				}
//				previewMenu.attachedObject = focusedObject;
//				focusedObject.GetComponent<Camera> ().enabled = true;
//			}
//		}
//	}
//	//--------------------------------------------------------------------//
//	//                  END PUBLIC FUNCTION DEFINITIONS                   //
//	//--------------------------------------------------------------------//
//	//--------------------------------------------------------------------//
//	//                    PRIVATE FUNCTION DEFINITIONS                    //
//	//--------------------------------------------------------------------//
//	void chooseCameraMenuType(Dropdown target){
//
//		switch (target.value)
//		{
//		case 0:
//			noiseMenu.Show ();
//			globalCameraMenu.Hide ();
//			trajectoryMenu.Hide ();
//			break;
//		case 1:
//			noiseMenu.Hide ();
//			globalCameraMenu.Show ();
//			trajectoryMenu.Hide ();
//			break;
//		case 2:
//			noiseMenu.Hide ();
//			globalCameraMenu.Hide ();
//			trajectoryMenu.Show ();
//			break;
//		case 3:
//			noiseMenu.Hide ();
//			globalCameraMenu.Hide ();
//			trajectoryMenu.Hide ();
//			break;
//		default:
//			break;
//		}
//	}
//	//--------------------------------------------------------------------//
//	//                  END PRIVATE FUNCTION DEFINITIONS                  //
	//--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//