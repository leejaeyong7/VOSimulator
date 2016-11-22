/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : test.cs
 * @brief      : Event handler for trajectory create UI
 * Copyright (c) Jae Yong Lee / UIUC Fall 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using com.ootii.Messages;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//--------------------------//
//     HELPER CLASSES
//--------------------------//
// class representing featurePoint
public class featurePoint
{
	public int trueId;
	public int capturedId;
	public Vector2 trueScreenPos;
	public Vector2 capturedScreenPos;
	public Vector3 truePos;

	new public string ToString()
	{
		return trueId.ToString() + " " + capturedId.ToString() + " " +
					 trueScreenPos.x.ToString() + " " + trueScreenPos.y.ToString() + " " +
					 capturedScreenPos.x.ToString() + " " + capturedScreenPos.y.ToString() + " " +
					 truePos.x.ToString() + " " + truePos.y.ToString() + " " + truePos.z.ToString();
	}
}
public class viewpoint
{
	public Vector3 position;
	public Quaternion rotation;
	List<featurePoint> features;

	public void setFeatures(List<featurePoint> l)
	{
		features = l;
	}

	new public string ToString()
	{
		string content = "";
		content += position.x.ToString() + " " + position.y.ToString() + " " +
			position.z.ToString() + " " + rotation.x.ToString() + " " +
			rotation.y.ToString() + " " + rotation.z.ToString() + " " +
			rotation.w.ToString() + "\n";
		foreach (featurePoint f in features)
		{
			content += f.ToString() + "\n";
		}
		return content;
	}

}

// class representing camera controlpoint
public class ControlPoint
{
	public Vector3 position;
	public Quaternion rotation;
}
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class Trajectory {
	//********************************************************************//
	//***************************BEGIN VARIABLES**************************//
	//********************************************************************//
	//====================================================================//
	//                    PUBLIC VARIABLE DEFINITIONS                     //
	//====================================================================//
	public string name;

	public List<Vector3> origpositions = null;
	public List<Vector3> positions = new List<Vector3>();
	public List<Quaternion> rotations = null;
	public List<Vector3> featurePoints = null;
	public float scale = 1;
	// Used for Noise modelling
	public float mu_u = 0;
	public float mu_v = 0;
	public float sig_u = 1;
	public float sig_v = 1;


	public float focalLength = 1.0f;
	public float FOV = 45.0f;
	public float aspect = 4.0f / 3.0f;

	// Used for randomness modelling
	public float droprate = 0;
	public float fliprate = 0;

    public float imageCaptureThreshold = 0;

	// Used for setting maximum number of features
	public int maxFeatures = 0;

	public bool maxImageSubsampleMode = true;
	public bool indexSkipSubsampleMode = false;
	public bool distanceSkipSubsampleMode = true;

	public int numMaxImages = 0;
	public int numIndexSkip = 0;
    public int executeId;

	public float num_camera= 0;
	public float totalLength = 0;
	public float dist_x = 0;
	public float dist_y = 0;
	public float dist_z = 0;
	//====================================================================//
	//                  END PUBLIC VARIABLE DEFINITIONS                   //
	//====================================================================//
	//====================================================================//
	//                    PRIVATE VARIABLE DEFINITIONS                    //
	//====================================================================//
	List<featurePoint> trackedFeatures = new List<featurePoint>();
    int totalCaptures = 0;
	float captureThreshold = 0;
	float lengthThreshold = 0;
	float cumulativeLength = 0;
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
    //====================================================================//
    //               END MONOBEHAVIOR FUNCTION DEFINITIONS                //
    //====================================================================//
    //====================================================================//
    //                     PUBLIC METHOD DEFINITIONS                      //
    //====================================================================//
    public void update()
	{
		float max_x = -Mathf.Infinity;
		float max_y = -Mathf.Infinity;
		float max_z = -Mathf.Infinity;

		float min_x = Mathf.Infinity;
		float min_y = Mathf.Infinity;
		float min_z = Mathf.Infinity;

		positions.Clear();
		Vector3 temp = origpositions [0];
		totalLength = 0;
		positions.Add(origpositions[0] * scale);
		for (int i = 1; i < origpositions.Count; i++)
		{
			totalLength += (origpositions [i] - temp).magnitude;
			positions.Add(origpositions[i] * scale);
			temp = origpositions [i];
			max_x = Mathf.Max (max_x, origpositions [i].x);
			max_y = Mathf.Max (max_y, origpositions [i].y);
			max_z = Mathf.Max (max_z, origpositions [i].z);

			min_x = Mathf.Min (min_x, origpositions [i].x);
			min_y = Mathf.Min (min_y, origpositions [i].y);
			min_z = Mathf.Min (min_z, origpositions [i].z);
		}
		totalLength *= scale;
		dist_x = (max_x - min_x) * scale;
		dist_y = (max_y - min_y) * scale;
		dist_z = (max_z - min_z) * scale;
		num_camera = origpositions.Count;

		setupSubsampleConstants ();
	}
    public void initialize()
    {
        executeId = 0;
        totalCaptures = 0;
		cumulativeLength = 0;
    }
	void setupSubsampleConstants()
    {
		float divisionFactor = 1;
		if (maxImageSubsampleMode) {
			divisionFactor = ((float)positions.Count)/numMaxImages;
		}
		if(indexSkipSubsampleMode){
			captureThreshold = divisionFactor*numIndexSkip; 
		}
		else if (distanceSkipSubsampleMode) {
			lengthThreshold = (float)totalLength / numMaxImages;	
		}
    }
	public bool execute()
	{
		// if execute has been finished, return false;
        if(executeId > positions.Count - 1)
        {
            executeId = -1;
            return false;
        }
		// if using disatnce skip mode, 
		if (distanceSkipSubsampleMode) {
			while (cumulativeLength <= lengthThreshold * totalCaptures) {
				if (executeId > 0) {
					float dist = (positions [executeId - 1] - positions [executeId]).magnitude;
					cumulativeLength += dist;
				} else {
					cumulativeLength = 0;	
				}
				executeId++;
				if (executeId > positions.Count - 1) {
					return false;
				}
			}
		} else if(totalCaptures*captureThreshold > executeId){
			// if using other method
			executeId++;
			return true;
		}

		System.Random rnd = new System.Random();
		bool[] featureIndices = new bool[featurePoints.Count];
		Vector3[] featureScreenPos = new Vector3[featurePoints.Count];

		viewpoint v = new viewpoint();
		// get position/rotation from list of cameras
		Vector3 pos = positions[executeId];
		Quaternion rot = rotations[executeId];

		Camera.main.transform.position = pos;
		Camera.main.transform.rotation = rot;

		// prepare writing
		v.position = new Vector3(pos.x,pos.y,pos.z);
        v.rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);

		int totalRemaining = 0;

		// filter all features that are in frustum that are visible
		for (int i = 0; i < featurePoints.Count; i++)
		{
			Vector3 sc = Camera.main.WorldToScreenPoint(featurePoints[i]);
			featureIndices[i] = false;
			if (sc.x >= 0 && sc.x <= Screen.width &&
				sc.y >= 0 && sc.y <= Screen.height &&
				sc.z > 0)
			{
				RaycastHit hit;
				bool isHit = Physics.Linecast(featurePoints[i], Camera.main.transform.position, out hit);
				if (!isHit)
				{
					featureIndices[i] = true;
					featureScreenPos[i] = sc;
				}
			}
		}

		// drop all features that are not in frustum
		trackedFeatures = trackedFeatures.Where(x => featureIndices[x.trueId]).ToList();

		// perform random drop
		trackedFeatures = trackedFeatures.Where(x => (rnd.NextDouble() >= droprate)).ToList();

		// set all feature indices to false for all tracked features
		foreach (featurePoint f in trackedFeatures)
		{
			// update screen position to fit in current camera
			f.trueScreenPos = featureScreenPos[f.trueId];
			f.capturedScreenPos = norm2D(f.trueScreenPos);
			featureIndices[f.trueId] = false;
			totalRemaining--;
		}

		// fill remaining feature points
		// uniformly select toPick points from remaining indices
		int toPick = maxFeatures - trackedFeatures.Count;
		int[] uniformRandomIndices = new int[toPick];
        for(int i = 0; i < toPick; i++)
        {
            uniformRandomIndices[i] = -1;
        }
		int picked = 0;
		for (int i = 0; i < featurePoints.Count; i++)
		{
			if (featureIndices[i])
			{
				if (picked < toPick)
				{
					uniformRandomIndices[picked] = i;
					picked++;
				}
				else {
					int randval = Random.Range(0, picked);
					picked++;
					if (randval < toPick)
					{
						uniformRandomIndices[randval] = i;
					}
				}
			}
		}

		// fill in feature points
		for (int i = 0; i < toPick; i++)
		{
            if(uniformRandomIndices[i] >= 0)
            {   
                featurePoint f = new featurePoint();
                f.trueId = uniformRandomIndices[i];
                f.capturedId = uniformRandomIndices[i];
                f.trueScreenPos = featureScreenPos[f.trueId];
                f.capturedScreenPos = norm2D(f.trueScreenPos);
                f.truePos = featurePoints[f.trueId];
                trackedFeatures.Add(f);
            }
		}

		// perform random flip
		bool[] trackFlipMap = trackedFeatures.Select(x => rnd.NextDouble() < fliprate).ToArray();


		IEnumerable<featurePoint> nonflipped = trackedFeatures.Where((x, i) => !trackFlipMap[i]);

		featurePoint[] flipped = trackedFeatures.Where((x, i) => trackFlipMap[i]).ToArray();


		int[] flipTrueId = flipped.Select(x => x.trueId).OrderBy(r => rnd.Next()).ToArray();

		for (int i = 0; i < flipped.Count(); i++)
		{
			flipped[i].capturedId = flipTrueId[i];
		}
		trackedFeatures = nonflipped.Concat(flipped).ToList();

		// print to file
		v.setFeatures(trackedFeatures);
        System.IO.Directory.CreateDirectory("./Executes/" + name.ToString() + '/');

        System.IO.Directory.CreateDirectory("./Executes/" + name.ToString() + "/f" + fliprate.ToString("0.00") + "s"
            + sig_u.ToString("0.00")+ '/');

        System.IO.File.WriteAllText(
			"./Executes/" + name.ToString()+ "/f" + fliprate.ToString("0.00") + "s" +
            sig_u.ToString("0.00")+ "/" + executeId.ToString() + ".txt",
			v.ToString()
		);
		Application.CaptureScreenshot(
			"./Executes/" + name.ToString()+ "/f" + fliprate.ToString("0.00") + "s" +
            sig_u.ToString("0.00") + "/" + executeId.ToString() + ".png"
		);

        executeId++;
        totalCaptures++;
        return true;
	}
	//====================================================================//
	//                   END PUBLIC METHOD DEFINITIONS                    //
	//====================================================================//
	//====================================================================//
	//                     PRIVATE METHOD DEFINITIONS                     //
	//====================================================================//
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

	// random normal distrubution selector
	Vector3 norm2D(Vector3 input)
	{
		float newx = RandomFromDistribution.RandomNormalDistribution(input.x + mu_u, sig_u);
		float newy = RandomFromDistribution.RandomNormalDistribution(input.y + mu_v, sig_v);
		return new Vector3(newx, newy, input.z);
	}

	//====================================================================//
	//                  END HELPER FUNCTION DEFINITIONS                   //
	//====================================================================//
	//********************************************************************//
	//*******************************END ETC******************************//
	//********************************************************************//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//