/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : ExecuteHandler.cs
 * @brief      : Event handler for Execute menu 
 * Copyright (c) Jae Yong Lee / UIUC Fall 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.ootii.Messages;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class ExecuteHandler : MonoBehaviour
{
    //********************************************************************//
    //***************************BEGIN VARIABLES**************************//
    //********************************************************************//
    //====================================================================//
    //                    PUBLIC VARIABLE DEFINITIONS                     //
    //====================================================================//
    public Models models;
    public TrajectoryHandler th;
    public GameObject GUI;
    //====================================================================//
    //                  END PUBLIC VARIABLE DEFINITIONS                   //
    //====================================================================//
    //====================================================================//
    //                    PRIVATE VARIABLE DEFINITIONS                    //
    //====================================================================//
    int numImages;
    int numIndexSkip;

    int executionId;
    int executionCount;
    string executionName;
    List<int> executionQueue;
    List<Vector3> features;

    bool numImagesMode;
    bool captureMode;
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

	void Start()
    {
        captureMode = false;
        executionQueue = new List<int>();
        features = new List<Vector3>();
        MessageDispatcher.AddListener("TOGGLE_SKIP_METHOD", numImagesToggleHandler);
        MessageDispatcher.AddListener("SET_NUM_IMAGES",setNumImagesHandler);
        MessageDispatcher.AddListener("SET_NUM_INDEX_SKIP", setNumIndexSkipHandler);
        MessageDispatcher.AddListener("EXECUTE_TRAJECTORY", executeHandler);
        MessageDispatcher.AddListener("EXECUTE_ALL_TRAJECTORIES", executeAllHandler);
    }

    void Update()
    {
        if (captureMode)
        {
            if (!models.Trajectories[th.currentTrajectoryId].execute())
            {
                executeNext();
            }
        }
    }
    
    //====================================================================//
    //               END MONOBEHAVIOR FUNCTION DEFINITIONS                //
    //====================================================================//
    //====================================================================//
    //                     PUBLIC METHOD DEFINITIONS                      //
    //====================================================================//
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
    void numImagesToggleHandler(IMessage rMessage)
    {
        numImagesMode = (bool)rMessage.Data;
    }
    void setNumImagesHandler(IMessage rMessage)
    {
        numImages = (int)Mathf.Floor((float)rMessage.Data);
    }
    void setNumIndexSkipHandler(IMessage rMessage)
    {
        numIndexSkip = (int)Mathf.Floor((float)rMessage.Data);
    }
    void executeHandler(IMessage rMessage)
    {
        executionQueue.Add(th.currentTrajectoryId);
        executeNext();
    }
    void executeAllHandler(IMessage rMessage)
    {
        for(int i = 0; i < models.Trajectories.Count; i++)
        {
            executionQueue.Add(i);
        }
        executeNext();
    }

    void setupExecution(int id)
    {
        th.currentTrajectoryId = id;
        th.updateTrajectory();
        captureMode = true;


        GUI.SetActive (false);
        Camera.main.cullingMask = 1;
        enableCameraCollider (false);
    }
   
    void executeNext()
    {
        if (executionQueue.Count > 0)
        {
            int nextid = executionQueue[0];
            executionQueue.RemoveAt(0);
            setupExecution(nextid);
        }
        else
        {
            Camera.main.cullingMask = -1;
            enableCameraCollider(true);
            GUI.SetActive(true);
            captureMode = false;
        }
    }
    
    // enables camera collider for gizmo selections
    void enableCameraCollider(bool enable)
    {
        BoxCollider[] mcs = models.TrajectoryGameObject.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider mc in mcs)
        {
            mc.enabled = enable;
        }
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