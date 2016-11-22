using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using com.ootii.Messages;


public class CustomTrajectory : Trajectory {

	public CustomTrajectory(){
		origpositions = new List<Vector3> ();	
		rotations = new List<Quaternion> ();
	}
	public List<ControlPoint> controlPoints;

	new public void update(){
		if (controlPoints.Count < 2) {
			return;
		}
		int fps = 30;
		bool isLoop = true;

		origpositions.Clear ();
		origpositions = Interpolate.NewCatmullRom (
			controlPoints.Select (p => p.position).ToArray(),fps-1,false).ToList();
		rotations.Clear ();

		float spf = 1.0f / fps;
		for (int i = 0; i < controlPoints.Count-2; i++) {
			for (int j = 0; j < fps; j++) {
				rotations.Add (
					Quaternion.Slerp(controlPoints[i].rotation, controlPoints[i+1].rotation,spf*j)
				);
			}
		}
		if (isLoop) {
			for (int j = 0; j < fps; j++) {
				rotations.Add (
					Quaternion.Slerp (controlPoints[controlPoints.Count - 2].rotation,
						controlPoints[controlPoints.Count - 1].rotation, spf * j)
				);
			}
			for (int j = 0; j < fps + 1; j++) {
				rotations.Add (
					Quaternion.Slerp (controlPoints[controlPoints.Count - 1].rotation,
						controlPoints[0].rotation, spf * j)
				);
			}
		} else {
			for (int j = 0; j < fps+1; j++) {
				rotations.Add (
					Quaternion.Slerp (controlPoints[controlPoints.Count - 2].rotation,
						controlPoints[controlPoints.Count - 1].rotation, spf * j )
				);
			}
		}
		base.update ();
	}
}
