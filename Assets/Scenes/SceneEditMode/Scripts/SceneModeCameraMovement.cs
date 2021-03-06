﻿/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : SceneModeCameraMovement.cs
 * @brief      : Event handler for Main Camera for VOS
 * Copyright (c) Jae Yong Lee / UIUC Summer 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//

public class SceneModeCameraMovement : MonoBehaviour {
    /* Public variables */
    public float zoomSpeed = 10.0f;
    public float rotateXSpeed = 0.1f;
    public float rotateYSpeed = 0.1f;
	public float transSpeed = 1.0f;

	// 0 = orbit mode, 1 = fly mode
	public int type = 0;

	// other public variables
    public IOHandler ev;
	public CustomFileBrowser fb;
	public gizmoScript gizmos;
	public bool isObjectDragged = false;

    /* Private variables */
    private Vector3 target;
    private Vector3 prevMousePos;
    private Vector3 currMousePos;
    
    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
    /**
     * @brief Initializes camera transformation data
     * @action sets target
     */
    void Start()
    {
        // calculate target point(used as pivot)
        Vector3 position = transform.position;
        Vector3 lookAt = transform.forward;
        float v = -(position.y / lookAt.y);
        target = position + v * (lookAt);
    }

    /**
     * @brief Updates camera transformation using mouse input
     * @action sets camera transformation data
     */
    void Update()
    {
		if (!isAbleToUpdate ()) {
			return;
		}
        // if mouse button is being clicked for first time, get its position
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            prevMousePos = Input.mousePosition;
            return;
        }
        // else use mouse position difference to calculate difference
        currMousePos = Input.mousePosition;
        Vector3 diff = currMousePos - prevMousePos;
        prevMousePos = currMousePos;

		if (type == 0) {
	        // left mouse drag handler
	        if (Input.GetMouseButton(0))
	        {
	            // rotate around camera's x axis
	            transform.RotateAround(
	                target, 
	                transform.right, 
	                rotateXSpeed * diff.y);

	            // rotate around pivot's y axis
	            transform.RotateAround(
	                target, 
	                Vector3.up, 
	                -rotateYSpeed * diff.x);
	        }
	        // right mouse drag handler
	        if (Input.GetMouseButton(1))
	        {
	            // calculate translation using mouse position difference
	            Vector3 translation = 
	                new Vector3(-transSpeed*diff.x, -transSpeed*diff.y , 0);
	            Vector3 origPos = transform.position;
	            // translate camera
	            transform.Translate(translation);

	            // move target same as camera translation movement
	            target += transform.position - origPos;
	        }

	        // zoom handler
	        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
	        {
	            Vector3 translation = new Vector3(0,0,zoomSpeed);
	            transform.Translate(translation);
	        }
	        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
	        {
	            Vector3 translation = new Vector3(0, 0, -zoomSpeed);
	            transform.Translate(translation);
	        }
		} else if (type == 1) {
			if (Input.GetMouseButton(1))
	        {
				Quaternion qx = Quaternion.Euler (
					-rotateXSpeed * diff.y, 0, 0);
				Quaternion qy = Quaternion.Euler (
					0, rotateXSpeed * diff.x, 0);

				transform.rotation = qy * transform.rotation * qx;
	        }
			if (Input.GetKey(KeyCode.W)) {
				transform.Translate (0, 0, transSpeed);
			}
			if (Input.GetKey(KeyCode.A)) {
				transform.Translate (-transSpeed, 0, 0);
			}
			if (Input.GetKey(KeyCode.S)) {
				transform.Translate (0, 0, -transSpeed);
			}
			if (Input.GetKey(KeyCode.D)) {
				transform.Translate (transSpeed,0, 0);
			}
		} 
    }
	bool isAbleToUpdate(){
		return !(isObjectDragged || 
			PanelDragHandler.isPanelDragged || 
			fb.showFB ||
			gizmos.isDragged ||
			ev.isGUIClicked() );
	}
    //--------------------------------------------------------------------//
    //                  END PUBLIC FUNCTION DEFINITIONS                   //
    //--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
