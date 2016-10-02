/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : TrajectoryMenu.cs
 * @brief      : Trajectory menu event handler
 * Copyright (c) Jae Yong Lee / UIUC Fall 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class TrajectoryMenu : MenuPanel {
	public CameraPanel cameraPanel;
	public GameObject Trajectories;
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

	//Use GameObject in 3d space as your points or define array with desired points

	//Store points on the Catmull curve so we can visualize them
	public List<Vector3> path;
	public List<Transform> points;
	int cameraobjectlayer = 9;
	GameObject lines;
	LineRenderer lr;
	void Start(){
		points = new List<Transform> ();
		path = new List<Vector3> ();
		lines = new GameObject ();
		lines.name = "Path";
		lines.layer = cameraobjectlayer;
		lr = lines.AddComponent<LineRenderer> ();
	}

	public void updatePath(){
		// update all input points
		getPoints();
		path = Interpolate.NewCatmullRom (
			points.ToArray(), 
			points.Count*60,
			false).ToList();
		lr.SetVertexCount (path.Count);
		lr.SetPositions (path.ToArray ());
	}

	void getPoints() {
		points.Clear ();
		foreach (Transform child in Trajectories.transform) {
			points.Add (child);
		}
	}
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
