using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;

public class ControlPoint{
	public Vector3 Position;
	public Vector3 Rotation;
}

public class TrajectoryHandler{
	List<ControlPoint> controlPoints;
	List<Vector3> Path;
	
	// initialize message dispatchers
	void Start() {
		controlPoints = new List<ControlPoint> ();
		MessageDispatcher.AddListener ("ADD_TRAJECTORY_POINT", addControlPoint);
	}

	public void addControlPoint(IMessage rMessage){
		
		updateTrajectory ();
	}

	void updateTrajectory(){

	}

}
