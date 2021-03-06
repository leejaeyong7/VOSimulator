﻿/*============================================================================
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
//----------------------------------------------------------------------------//
//                              CLASS DEFINITION                              //
//----------------------------------------------------------------------------//
// trajectory handler class
public class TrajectoryHandler: MonoBehaviour{
	//********************************************************************//
	//***************************BEGIN VARIABLES**************************//
	//********************************************************************//
	//====================================================================//
	//                    PUBLIC VARIABLE DEFINITIONS                     //
	//====================================================================//
	public CustomFileBrowser fb;
	// camera prefab that I use to copy on trajectories
	public GameObject cameraObject;
	// Pointer to GUI that I use to hide when running trajectory
	public GameObject GUI;

	public PreviewMenu previewMenu;

	public Models models;

    public int currentTrajectoryId;


    //====================================================================//
    //                  END PUBLIC VARIABLE DEFINITIONS                   //
    //====================================================================//
    //====================================================================//
    //                    PRIVATE VARIABLE DEFINITIONS                    //
    //====================================================================//
    PTSLoader ptsLoader;
	// Used for Noise modelling
	float mu_u = 0;
	float mu_v = 0;
	float sig_u = 1;
	float sig_v = 1;


	float focalLength = 1.0f;
	float FOV = 45.0f;
	float aspect = 4.0f / 3.0f;

	// Used for randomness modelling
	float droprate = 0;
	float fliprate = 0;
    int numImages = 0;

    int numIndexSkip = 1;
    bool numImagesMode = true;
    bool indexSkipMode = false;
    bool distanceSkipMode = true;

    // Used for setting maximum number of features
    int maxFeatures = 500;
    List<Vector3> features;

	CustomTrajectory customTrajectory;
	List<ControlPoint> controlPoints;


    //====================================================================//
    //                  END PRIVATE VARIABLE DEFINITIONS                  //
    //====================================================================//
    //********************************************************************//
    //****************************END VARIABLES***************************//
    //********************************************************************//
    //********************************************************************//
    //****************************BEGIN METHODS***************************//
    //********************************************************************//
    //====================================================================//
    //                 MONOBEHAVIOR FUNCTION DEFINITIONS                  //
    //====================================================================//
	// initialize message dispatchers
	void Start()
	{

		ptsLoader = new PTSLoader();
        features = new List<Vector3>();
		controlPoints = new List<ControlPoint>();
		// set main culling mask to everything
		Camera.main.cullingMask = -1;
		// initialize trajectories
		customTrajectory = new CustomTrajectory ();
		customTrajectory.name = "Custom";
		models.Trajectories.Add ((Trajectory)customTrajectory);

		// setup event listeners
		MessageDispatcher.AddListener("IMPORT_TRAJECTORY",importTrajectory);
		MessageDispatcher.AddListener ("ADD_TRAJECTORY_POINT",addTrajectoryPoint);

		MessageDispatcher.AddListener(
			"TRAJECTORY_UP_PRESSED",
			trajectoryUpButtonHelper);

		MessageDispatcher.AddListener(
			"TRAJECTORY_DOWN_PRESSED",
			trajectoryDownButtonHelper);

		MessageDispatcher.AddListener(
			"TRAJECTORY_UPDATED",
			trajectoryDataHelper);


		MessageDispatcher.AddListener(
			"TRAJECTORY_SELECTED",
			trajectorySelectHelper);

		MessageDispatcher.AddListener(
			"TRAJECTORY_DROPDOWN_REFRESH",
			loadTrajectoryHelper);


		// setup event listners for camera noise model get/setters
		MessageDispatcher.AddListener("SET_2D_NOISE_MU_U",
			delegate (IMessage rMessage) { mu_u = (float)rMessage.Data; });
		MessageDispatcher.AddListener("SET_2D_NOISE_MU_V",
			delegate (IMessage rMessage) { mu_v = (float)rMessage.Data; });
		MessageDispatcher.AddListener("SET_2D_NOISE_SIG_U",
			delegate (IMessage rMessage) { sig_u = (float)rMessage.Data; });
		MessageDispatcher.AddListener("SET_2D_NOISE_SIG_V",
			delegate (IMessage rMessage) { sig_v = (float)rMessage.Data; });
		MessageDispatcher.AddListener("SET_DROPRATE",
			delegate (IMessage rMessage) { droprate = (float)rMessage.Data; });
		MessageDispatcher.AddListener("SET_FLIPRATE",
			delegate (IMessage rMessage) { fliprate = (float)rMessage.Data; });
		MessageDispatcher.AddListener("SET_MAX_FEATURE_POINT",
			delegate (IMessage rMessage) { maxFeatures = (int)((float)rMessage.Data); });
		MessageDispatcher.AddListener("SET_FOCAL_LEGNTH",
			delegate (IMessage rMessage) { focalLength = (float)rMessage.Data; });
		MessageDispatcher.AddListener("SET_FOV",
			delegate (IMessage rMessage) { fliprate = (float)rMessage.Data; });
		MessageDispatcher.AddListener("SET_ASPECT",
			delegate (IMessage rMessage) { aspect = (float)rMessage.Data; });

        MessageDispatcher.AddListener("SET_NUM_IMAGES", 
            delegate(IMessage rMessage) { numImages =  (int)((float)rMessage.Data); });
        MessageDispatcher.AddListener("SET_NUM_INDEX_SKIP",
            delegate (IMessage rMessage) { numIndexSkip =  (int)((float)rMessage.Data); });


		MessageDispatcher.AddListener ("NUM_IMAGE_MODE",
			delegate(IMessage rMessage) { numImagesMode = (bool)rMessage.Data;});
        MessageDispatcher.AddListener("INDEX_SKIP_MODE",
			delegate(IMessage rMessage) { indexSkipMode = (bool)rMessage.Data; });
        MessageDispatcher.AddListener("DISTANCE_CAMERA_MODE", 
			delegate(IMessage rMessage) { distanceSkipMode = (bool)rMessage.Data;});

        MessageDispatcher.AddListener("TOGGLE_SKIP_METHOD",
            delegate (IMessage rMessage) { numImagesMode = (bool)rMessage.Data; });
    }
    //====================================================================//
    //               END MONOBEHAVIOR FUNCTION DEFINITIONS                //
    //====================================================================//
    //====================================================================//
    //                     PUBLIC METHOD DEFINITIONS                      //
    //====================================================================//

    public void updateTrajectory()
    {
        Trajectory t = models.Trajectories[currentTrajectoryId];

        t.mu_u = mu_u;
        t.mu_v = mu_v;
        t.sig_u = sig_u;
        t.sig_v = sig_v;
        t.scale = models.scale;
        t.droprate = droprate;
        t.fliprate = fliprate;
        t.FOV = FOV;
        t.focalLength = focalLength;
        t.aspect = aspect;
        t.maxFeatures = maxFeatures;

		t.maxImageSubsampleMode = numImagesMode;
		t.numMaxImages = numImages;
		t.indexSkipSubsampleMode = indexSkipMode;
		t.numIndexSkip = numIndexSkip;
		t.distanceSkipSubsampleMode = distanceSkipMode;

        t.update();
        t.initialize();
        models.trajectoryLine.SetVertexCount(t.positions.Count);
        models.trajectoryLine.SetPositions(t.positions.ToArray());
        loadFeatures();
        t.featurePoints = features;
    }


    //====================================================================//
    //                   END PUBLIC METHOD DEFINITIONS                    //
    //====================================================================//
    //====================================================================//
    //                     PRIVATE METHOD DEFINITIONS                     //
    //====================================================================//

    void importTrajectory(IMessage rMessage)
	{
		fb.showBrowser("pts", (f) => {
			if(ptsLoader.loadAsync(f)){
				Trajectory t = new Trajectory();
				t.origpositions = new List<Vector3>(ptsLoader.positions);
				t.rotations = new List<Quaternion>(ptsLoader.rotations);
				t.name = f.Name;
				t.update();
				models.Trajectories.Add(t);
			}
		});	
	}

	void addTrajectoryPoint(IMessage rMessage)
	{
		Vector3 newPos = Camera.main.transform.position;
		Quaternion newRot = Camera.main.transform.rotation;
		ControlPoint cp = new ControlPoint ();
		cp.position = newPos;
		cp.rotation = newRot;
		controlPoints.Add (cp);
		customTrajectory.controlPoints = controlPoints;
		addCamera (cp, controlPoints.Count);
		customTrajectory.update ();
		updateTrajectory();
	}


    //====================================================================//
    //                   END PRIVATE METHOD DEFINITIONS                   //
    //====================================================================//
    //********************************************************************//
    //*****************************END METHODS****************************//
    //********************************************************************//
    //********************************************************************//
    //******************************BEGIN ETC*****************************//
    //********************************************************************//
    //====================================================================//
    //                    HELPER FUNCTION DEFINITIONS                     //
    //====================================================================//
	void trajectoryUpButtonHelper(IMessage rMessage)
	{

		currentTrajectoryId--;
		if (currentTrajectoryId < 0)
		{
			currentTrajectoryId = 0;
		}

		MessageDispatcher.SendMessageData(
			"SET_TRAJECTORY_DROPDOWN",
			currentTrajectoryId);
	}


	void trajectoryDownButtonHelper(IMessage rMessage)
	{

		currentTrajectoryId++;
		if (currentTrajectoryId > models.Trajectories.Count - 1)
		{
			currentTrajectoryId = models.Trajectories.Count - 1;
		}

		MessageDispatcher.SendMessageData(
			"SET_TRAJECTORY_DROPDOWN",
			currentTrajectoryId);

        MessageDispatcher.SendMessageData(
            "SET_TRAJECTORY_RANGE",
            models.Trajectories[currentTrajectoryId].positions.Count);
    }

	void loadTrajectoryHelper(IMessage rMessage)
	{
		List<string> pathnames = new List<string>();
		pathnames = models.Trajectories.Select((t) => { return t.name; }).ToList();
		MessageDispatcher.SendMessageData("LOAD_TRAJECTORY_DROPDOWN", pathnames);

        MessageDispatcher.SendMessageData(
            "SET_TRAJECTORY_RANGE",
            models.Trajectories[currentTrajectoryId].positions.Count);
    }


	void trajectorySelectHelper(IMessage rMessage)
	{
		int trjId = (int)rMessage.Data;
		currentTrajectoryId = trjId;
		updateTrajectory();
	}
	void trajectoryDataHelper(IMessage rMessage)
	{
		updateTrajectory();

	}

    void loadFeatures()
    {
        features.Clear();
        MeshFilter[] mfs = models.Features.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mf in mfs)
        {
            features.AddRange(mf.mesh.vertices.Select(p => mf.transform.TransformPoint(p)).ToList());
        }
    }


	// adds camera on scene using prefab
	void addCamera(ControlPoint point,int id){
		GameObject camera = Instantiate (cameraObject);
		camera.GetComponent<Camera> ().enabled = false;
		camera.name = "Trajectory_View_" + id.ToString();
		camera.transform.position = point.position;
		camera.transform.rotation = point.rotation;
		camera.transform.localScale = new Vector3 (10,10,10);
		BoxCollider boxCol = camera.AddComponent<BoxCollider> ();
		boxCol.size = new Vector3 (0.3f,0.3f,0.3f);
		CameraObject co = camera.AddComponent<CameraObject> ();
		co.pm = previewMenu;
		camera.transform.SetParent (models.TrajectoryGameObject.transform);
	}


    //====================================================================//
    //                  END HELPER FUNCTION DEFINITIONS                   //
    //====================================================================//
    //********************************************************************//
    //*******************************END ETC******************************//
    //********************************************************************//
}
//----------------------------------------------------------------------------//
//                            END CLASS DEFINITION                            //
//----------------------------------------------------------------------------//
