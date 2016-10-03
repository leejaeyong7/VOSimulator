/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : ObjectEvents.cs
 * @brief      : Event handler for Object Menu
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
public class ObjectEvents : MonoBehaviour {
	private Vector3 screenPoint;
	private Ray ray;
	private SceneModeCameraMovement cameraMovement;

	//--------------------------------------------------------------------//
	//                    PUBLIC FUNCTION DEFINITIONS                     //
	//--------------------------------------------------------------------//
	/**
     * @brief Initializes camera transformation data
     * @action sets target
     */
	void Start()
	{
		cameraMovement = GameObject.Find ("SceneCamera")
			.GetComponent<SceneModeCameraMovement> ();
	}

    void OnMouseDown()
    {
		// copy object iff is on preview
		if (this.gameObject.transform.parent.name == "PreviewBG") {
			GameObject copy = Instantiate (this.gameObject);
			copy.transform.SetParent (this.gameObject.transform.parent);
			copy.transform.position = this.gameObject.transform.position;
			copy.transform.rotation = this.gameObject.transform.rotation;
			copy.transform.localScale = this.gameObject.transform.localScale;

			// get local rotation ( straight rotation )
			Quaternion locRot = this.gameObject.transform.localRotation;
			Vector3 locScale = this.gameObject.transform.localScale;

			// set parent to Terrain
			this.gameObject.transform.SetParent (Terrain.activeTerrain.transform);
			this.gameObject.transform.localRotation = locRot;
			this.gameObject.transform.localScale = locScale;
		}
		print ("Number of mesh triangles: " + this.gameObject.GetComponent<MeshFilter> ().mesh.triangles.Length.ToString ());
		cameraMovement.isObjectDragged = true;


    }

    void OnMouseDrag()
    {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// create a plane at 0,0,0 whose normal points to +Y:
		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
		float distance = 0; 
		// if the ray hits the plane...
		if (hPlane.Raycast(ray, out distance)){
			// get the hit point:
			transform.position = ray.GetPoint(distance);
		}
    }

    void OnMouseUp()
    {
		cameraMovement.isObjectDragged = false;
		if(GetComponentInParent<EnvMenu>())
        {
			GetComponentInParent<EnvMenu>().removeCurrObject();
        }
		this.gameObject.AddComponent<CurrentObjectEvents>();
		Destroy (this.gameObject.GetComponent<ObjectEvents> ());
	}
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

