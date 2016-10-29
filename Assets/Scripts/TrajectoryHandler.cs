/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : TrajectoryHandler.cs
 * @brief      : Handles Running path based on path
 * Copyright (c) Jae Yong Lee / UIUC Fall 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                             MODULE DEFINITIONS                             //
//----------------------------------------------------------------------------//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
using System.Linq;
using CielaSpike;
//----------------------------------------------------------------------------//
//                           END MODULE DEFINITIONS                           //
//----------------------------------------------------------------------------//
	//--------------------------//
	//     HELPER CLASSES
	//--------------------------//
	// class representing featurePoint
	public class featurePoint{
		public int trueId;
		public int capturedId;
		public Vector2 trueScreenPos;
		public Vector2 capturedScreenPos;
		public Vector3 truePos;

		new public string ToString(){
			return trueId.ToString () + " " + capturedId.ToString() + " " + 
				trueScreenPos.x.ToString() + " " + trueScreenPos.y.ToString() + " " + 
				capturedScreenPos.x.ToString() + " " + capturedScreenPos.y.ToString() + " " + 
				truePos.x.ToString() + " " + truePos.y.ToString() + " " + truePos.z.ToString();
		}
	}
	public class viewpoint{
		public Vector3 position;
		public Quaternion rotation;
		List<featurePoint> features;

		public void setFeatures(List<featurePoint> l){
			features = l;
		}

		new public string ToString(){
			string content = "";
			content += position.x.ToString () + " " + position.y.ToString () + " " + 
				position.z.ToString () + " " + rotation.x.ToString () + " " + 
				rotation.y.ToString () + " " + rotation.z.ToString () + " " +
				rotation.w.ToString () + "\n";
			foreach (featurePoint f in features) {
				content += f.ToString() + "\n";
			}
			return content;
		}

	}

	// class representing camera controlpoint
	public class ControlPoint{
		public Vector3 position;
		public Quaternion rotation;
}
//----------------------------------------------------------------------------//
//                              CLASS DEFINITION                              //
//----------------------------------------------------------------------------//
// trajectory handler class
public class TrajectoryHandler: MonoBehaviour{
	// Game Object that has trajectory coordinates diaplayed
	public GameObject Trajectories;
	// camera prefab that I use to copy on trajectories
	public GameObject cameraObject;
	// Pointer to GUI that I use to hide when running trajectory
	public GameObject GUI;


	public SLAMHandler slamHandler;

	public SimulatorHandler simHandler;

	// List of Control Points
	List<Vector3> PathPoints;
	List<Quaternion> PathRotations;
	List<Vector3> featurePoints;
	List<viewpoint> viewpoints;
	List<featurePoint> trackedFeatures;



	int trajectoryExecutionId = -1;
	// Used for Noise modelling
	float mu_u = 0;
	float mu_v = 0;
	float sig_u = 1;
	float sig_v = 1;

	// Used for randomness modelling
	float droprate = 0;
	float fliprate = 0;

	// Used for setting maximum number of features
	int maxFeatures = 0;

	
	// initialize message dispatchers
	void Start() {
		// set main culling mask to everything
		Camera.main.cullingMask = -1;
		// initialize vectors

		PathPoints = new List<Vector3> ();
		PathRotations = new List<Quaternion> ();
		featurePoints = new List<Vector3> ();

		viewpoints = new List<viewpoint> ();
		trackedFeatures = new List<featurePoint> ();

		// setup event listeners
		MessageDispatcher.AddListener ("EXECUTE_TRAJECTORY",executeTrajectory);


		// setup event listners for camera noise model get/setters
		MessageDispatcher.AddListener ("SET_2D_NOISE_MU_U", 
			delegate(IMessage rMessage) { mu_u  = (float)rMessage.Data; });
		MessageDispatcher.AddListener ("SET_2D_NOISE_MU_V", 
			delegate(IMessage rMessage) { mu_v  = (float)rMessage.Data; });
		MessageDispatcher.AddListener ("SET_2D_NOISE_SIG_U",
			delegate(IMessage rMessage) { sig_u = (float)rMessage.Data; });
		MessageDispatcher.AddListener ("SET_2D_NOISE_SIG_V", 
			delegate(IMessage rMessage) { sig_v = (float)rMessage.Data; });
		MessageDispatcher.AddListener ("SET_DROPRATE", 
			delegate(IMessage rMessage) { droprate = (float)rMessage.Data; });
		MessageDispatcher.AddListener ("SET_FLIPRATE", 
			delegate(IMessage rMessage) { fliprate = (float)rMessage.Data; });
		MessageDispatcher.AddListener ("SET_MAX_FEATURE_POINT", 
			delegate(IMessage rMessage) { maxFeatures= (int)rMessage.Data; });
	}


	void Update(){
		if (trajectoryExecutionId < 0) {
			return;
		} else if (trajectoryExecutionId == PathPoints.Count-1) {
			toggleExecution (false);
			return;
		}
		System.Random rnd = new System.Random();
		bool [] featureIndices = new bool [featurePoints.Count];
		Vector3 [] featureScreenPos  = new Vector3 [featurePoints.Count];

		viewpoint v = new viewpoint ();
		// get position/rotation from list of cameras
		Vector3 pos = PathPoints [trajectoryExecutionId];
		Quaternion rot = PathRotations [trajectoryExecutionId];

		Camera.main.transform.position = pos;
		Camera.main.transform.rotation = rot;

		// prepare writing
		v.position = pos;
		v.rotation = rot;

		int totalRemaining = 0;

		// filter all features that are in frustum that are visible
		for (int i = 0; i < featurePoints.Count; i++) {
			Vector3 sc = Camera.main.WorldToScreenPoint (featurePoints [i]);
			featureIndices [i] = false;
			if (sc.x >= 0 && sc.x <= Screen.width &&
			    sc.y >= 0 && sc.y <= Screen.height &&
			    sc.z > 0) {
				RaycastHit hit;
				bool isHit = Physics.Linecast (featurePoints[i], Camera.main.transform.position, out hit);
				if (!isHit) {
					featureIndices [i] = true;
					featureScreenPos [i] = sc;
				} 
			}
		}

		// drop all features that are not in frustum
		trackedFeatures = trackedFeatures.Where(x => featureIndices[x.trueId]).ToList();

		// perform random drop
		trackedFeatures = trackedFeatures.Where(x => (rnd.NextDouble() >= droprate)).ToList();

		// set all feature indices to false for all tracked features
		foreach (featurePoint f in trackedFeatures) {
			// update screen position to fit in current camera
			f.trueScreenPos = featureScreenPos [f.trueId];
			f.capturedScreenPos = norm2D (f.trueScreenPos);
			featureIndices [f.trueId] = false;
			totalRemaining--;
		}

		// fill remaining feature points
		// uniformly select toPick points from remaining indices
		int toPick = maxFeatures - trackedFeatures.Count;
		int[] uniformRandomIndices = new int[toPick];
		int picked = 0;
		for (int i = 0; i < featurePoints.Count; i++) {
			if (featureIndices [i]) {
				if (picked < toPick) {
					uniformRandomIndices [picked] = i;
					picked++;
				} else {
					int randval = Random.Range (0, picked);
					picked++;
					if (randval < toPick) {
						uniformRandomIndices [randval] = i;	
					}
				}
			}
		}

		// fill in feature points
		for (int i = 0; i < toPick; i++) {
			featurePoint f = new featurePoint ();
			f.trueId = uniformRandomIndices [i];
			f.capturedId = uniformRandomIndices [i];
			f.trueScreenPos = featureScreenPos [f.trueId];
			f.capturedScreenPos = norm2D (f.trueScreenPos);
			f.truePos = featurePoints [f.trueId];
			trackedFeatures.Add (f);
		}

		// perform random flip
		bool[] trackFlipMap = trackedFeatures.Select(x => rnd.NextDouble() < fliprate).ToArray();


		IEnumerable<featurePoint> nonflipped = trackedFeatures.Where ((x, i) => !trackFlipMap [i]);

		featurePoint[] flipped = trackedFeatures.Where ((x, i) => trackFlipMap [i]).ToArray();


		int[] flipTrueId = flipped.Select (x => x.trueId).OrderBy(r=> rnd.Next()).ToArray ();

		for (int i = 0; i < flipped.Count(); i++) {
			flipped [i].capturedId = flipTrueId [i];
		}
		trackedFeatures = nonflipped.Concat(flipped).ToList();
		// print to file
		v.setFeatures(trackedFeatures);

		System.IO.File.WriteAllText (
			"./Executes/output" + trajectoryExecutionId.ToString () + ".txt",
			v.ToString()
		);
		Application.CaptureScreenshot (
			"./Executes/output" + trajectoryExecutionId.ToString () + ".png"
		);

		trajectoryExecutionId++;
	}


	void toggleExecution(bool toggle){
		if (toggle) {
			trajectoryExecutionId = 0;
			trackedFeatures.Clear ();
			Camera.main.cullingMask = 1;
			enableCameraCollider (false);
			loadFeatures ();
			loadPathPoints ();
			GUI.SetActive (false);
		} else {
			trajectoryExecutionId = -1;
			Camera.main.cullingMask = -1;
			enableCameraCollider (true);
			GUI.SetActive (true);
		}
	}

    //
    //	// execute trajectory callback
    //	// loads feature, and 
	public void executeTrajectory(IMessage rMessage){
		toggleExecution (true);
	}


	// loads features from pointclouds(for now)
	void loadFeatures(){
		slamHandler.loadFeatures ();
		featurePoints = slamHandler.feature_points;
	}

	void loadPathPoints(){
		PathPoints = slamHandler.path_positions;
		PathRotations = slamHandler.path_rotations;
	}

	// enables camera collider for gizmo selections
	void enableCameraCollider(bool enable){
		BoxCollider[] mcs = Trajectories.GetComponentsInChildren<BoxCollider> ();
		foreach (BoxCollider mc in mcs) {
			mc.enabled = enable;
		}
	}

	// random normal distrubution selector
	Vector3 norm2D(Vector3 input){
		float newx = RandomFromDistribution.RandomNormalDistribution (input.x + mu_u, sig_u);
		float newy = RandomFromDistribution.RandomNormalDistribution(input.y + mu_v, sig_v); 
		return new Vector3 (newx, newy,input.z);
	}
}
//----------------------------------------------------------------------------//
//                            END CLASS DEFINITION                            //
//----------------------------------------------------------------------------//
