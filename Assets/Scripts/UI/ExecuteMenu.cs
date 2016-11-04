/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : .cs
 * @brief      : 
 * Copyright (c) Jae Yong Lee / UIUC Fall 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using com.ootii.Messages;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class ExecuteMenu : MonoBehaviour
{
    //********************************************************************//
    //***************************BEGIN VARIABLES**************************//
    //********************************************************************//
    //====================================================================//
    //                    PUBLIC VARIABLE DEFINITIONS                     //
    //====================================================================//
    public Button pathUpButton;
    public Button pathDownButton;
    public Dropdown PathDropdown;
    public Toggle numImagesToggle;
    public Toggle distanceOfIndexToggle;
	public Toggle distanceOfCameraToggle;
    public Slider numImagesSlider;
    public Slider distanceOfIndexSlider;
    public Button ExecuteButton;
    public Button ExecuteAllButton;
    //====================================================================//
    //                  END PUBLIC VARIABLE DEFINITIONS                   //
    //====================================================================//
    //====================================================================//
    //                    PRIVATE VARIABLE DEFINITIONS                    //
    //====================================================================//
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
    void Awake()
    {
        MessageDispatcher.AddListener(
            "LOAD_TRAJECTORY_DROPDOWN",
            loadTrajectoryDropdownCallback);

        MessageDispatcher.AddListener(
            "SET_TRAJECTORY_RANGE", setTrajectoryRangeCallback);

    }
    void Start()
    {
        pathUpButton.onClick.AddListener(pathUpButtonCallback);
        pathDownButton.onClick.AddListener(pathDownButtonCallback);
        PathDropdown.onValueChanged.AddListener(pathDropdownCallback);
        
        numImagesToggle.onValueChanged.AddListener(numImagesToggleCallback);
        distanceOfIndexToggle.onValueChanged.AddListener(indexSkipToggleCallback);
        distanceOfCameraToggle.onValueChanged.AddListener(distanceCameraToggleCallback);
        numImagesToggle.onValueChanged.AddListener(numImagesToggleCallback);
        distanceOfIndexToggle.onValueChanged.AddListener(numImagesToggleCallback);
        numImagesSlider.onValueChanged.AddListener(numImagesSliderCallback);
        distanceOfIndexSlider.onValueChanged.AddListener(distanceOfIndexSliderCallback);
        ExecuteButton.onClick.AddListener(executeCallback);
        ExecuteAllButton.onClick.AddListener(executeAllCallback);

        MessageDispatcher.AddListener(
            "SET_TRAJECTORY_DROPDOWN", setTrajectoryDropdownCallback);


    }
    void OnEnable()
    {
        MessageDispatcher.SendMessage("TRAJECTORY_DROPDOWN_REFRESH");
    }
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
    void pathUpButtonCallback()
    {
        MessageDispatcher.SendMessage("TRAJECTORY_UP_PRESSED");
    }
    void pathDownButtonCallback()
    {
        MessageDispatcher.SendMessage("TRAJECTORY_DOWN_PRESSED");
    }
    void pathDropdownCallback(int value)
    {
        MessageDispatcher.SendMessageData("TRAJECTORY_SELECTED", value);
    }
    void setTrajectoryDropdownCallback(IMessage rMessage)
    {
        int index = (int)rMessage.Data;
        PathDropdown.value = index;
    }
    void setTrajectoryRangeCallback(IMessage rMessage)
    {
        numImagesSlider.maxValue = (int)rMessage.Data;
    }


    void loadTrajectoryDropdownCallback(IMessage rMessage)
    {
        List<string> pathnames = (List<string>)rMessage.Data;
        PathDropdown.ClearOptions();
        List<Dropdown.OptionData> optlist = new List<Dropdown.OptionData>();
        foreach (string s in pathnames)
        {
            Dropdown.OptionData opt = new Dropdown.OptionData();
            opt.text = s;
            optlist.Add(opt);
        }
        PathDropdown.AddOptions(optlist);
    }

    void numImagesToggleCallback(bool ison)
    {
        MessageDispatcher.SendMessageData("NUM_IMAGE_MODE", ison);
    }
    void indexSkipToggleCallback(bool ison)
    {
        MessageDispatcher.SendMessageData("INDEX_SKIP_MODE", ison);
    }
    void distanceCameraToggleCallback(bool ison)
    {
        MessageDispatcher.SendMessageData("DISTANCE_CAMERA_MODE", ison);
    }

    void numImagesSliderCallback(float value)
    {
        MessageDispatcher.SendMessageData("SET_NUM_IMAGES", value);
    }
    void distanceOfIndexSliderCallback(float value)
    {
        MessageDispatcher.SendMessageData("SET_NUM_INDEX_SKIP", value);
    }
    void executeCallback()
    {
        MessageDispatcher.SendMessage("EXECUTE_TRAJECTORY");
    }
    void executeAllCallback()
    {
        MessageDispatcher.SendMessage("EXECUTE_ALL_TRAJECTORIES");
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

