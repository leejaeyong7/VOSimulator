using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
using System.Linq;
using CielaSpike;

public class SimulatorHandler : MonoBehaviour
{
	void OnEnable(){
		MessageDispatcher.SendMessageData ("SET_MODE", "Simulation");
	}

	void OnDisable(){
		
	}
//	class ControlPoint{
//		public Vector3 position;
//		public Quaternion rotation;
//	}
//	List<ControlPoint> ControlPoints;
//	void Start()
//	{
//		MessageDispatcher.AddListener ("ADD_TRAJECTORY_POINT", addControlPoint);
//	}
//
//
//
//
//	// adds controlpoint and updates path
//	public void addControlPoint(IMessage rMessage){
//		ControlPoint cp = new ControlPoint ();
//		cp.position = Camera.main.transform.position;
//		cp.rotation = Camera.main.transform.rotation;
//		ControlPoints.Add (cp);
//		updateControlPoints();
//		updatePath ();
//	}
//
//	// updates control point and adds trajectory camera object
//	void updateControlPoints(){
//		foreach (Transform child in Trajectories.transform) {
//			GameObject.Destroy(child.gameObject);
//		}
//		int id = 0;
//		foreach (ControlPoint point in ControlPoints) {
//			addCamera(point,++id);	
//		}
//	}
//
//
//	// adds camera on scene using prefab
//	void addCamera(ControlPoint point,int id){
//		GameObject camera = Instantiate (cameraObject);
//		camera.GetComponent<Camera> ().enabled = false;
//		camera.name = "Trajectory_View_" + id.ToString();
//
//		camera.transform.position = point.position;
//		camera.transform.rotation = point.rotation;
//
//		camera.transform.localScale = new Vector3 (10,10,10);
//		BoxCollider boxCol = camera.AddComponent<BoxCollider> ();
//		boxCol.size = new Vector3 (0.3f,0.3f,0.3f);
//
//		camera.AddComponent<CameraObject> ();
//
//		camera.transform.SetParent (Trajectories.transform);
//	}
//
//	void updatePath(){
//		// if there are less then 2 camera postions, no path is updated
//		if (ControlPoints.Count < 2) {
//			return;
//		}
//
//		bool isLoop = false;
//		int fps = 6;
//
//
//
//		PathPoints.Clear();
//		// setup positions per paths (using new catmullrom
//		PathPoints = Interpolate.NewCatmullRom (
//			ControlPoints.Select(p=>p.position).ToArray(), fps-1, isLoop).ToList();
//
//		PathRotations.Clear ();
//		// setup rotation per paths
//		float spf = 1.0f / fps;
//		for (int i = 0; i < ControlPoints.Count-2; i++) {
//			for (int j = 0; j < fps; j++) {
//				PathRotations.Add (
//					Quaternion.Slerp(ControlPoints [i].rotation, ControlPoints[i+1].rotation,spf*j)
//				);
//			}
//		}
//		if (isLoop) {
//			for (int j = 0; j < fps; j++) {
//				PathRotations.Add (
//					Quaternion.Slerp (ControlPoints [ControlPoints.Count - 2].rotation,
//						ControlPoints [ControlPoints.Count - 1].rotation, spf * j )
//				);
//			}
//			for (int j = 0; j < fps+1; j++) {
//				PathRotations.Add (
//					Quaternion.Slerp (ControlPoints [ControlPoints.Count - 1].rotation,
//						ControlPoints [0].rotation, spf * j)
//				);
//			}
//		} else {
//			for (int j = 0; j < fps+1; j++) {
//				PathRotations.Add (
//					Quaternion.Slerp (ControlPoints [ControlPoints.Count - 2].rotation,
//						ControlPoints [ControlPoints.Count - 1].rotation, spf * j )
//				);
//			}
//		}
//
//		lr.SetVertexCount (PathPoints.Count);
//		lr.SetPositions (PathPoints.ToArray ());
//	}
//

}

