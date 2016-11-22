/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : ViewTrajectoryMenu.cs
 * @brief      : Trajectory view mode event handler for UI
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
public class ViewTrajectoryMenu : MonoBehaviour
{
    //********************************************************************//
    //***************************BEGIN VARIABLES**************************//
    //********************************************************************//
    //====================================================================//
    //                    PUBLIC VARIABLE DEFINITIONS                     //
    //====================================================================//
    public Button pathUpButton;
    public Button pathDownButton;
    public Dropdown pathDropdown;
	public Slider scaleSlider;
    public Text numCamera;
    public Text totalLength;
    public Text deltaX;
    public Text deltaY;
    public Text deltaZ;
	public Models models;
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
    void Start()
    {
        pathUpButton.onClick.AddListener(pathUpButtonCallback);
        pathDownButton.onClick.AddListener(pathDownButtonCallback);
		scaleSlider.onValueChanged.AddListener(scaleChangeCallback);
        pathDropdown.onValueChanged.AddListener(pathDropdownCallback);

		MessageDispatcher.AddListener(
			"SET_TRAJECTORY_DROPDOWN", setTrajectoryDropdownCallback);
    }

	void OnEnable()
	{
		MessageDispatcher.SendMessage("TRAJECTORY_DROPDOWN_REFRESH");
		MessageDispatcher.SendMessageData("SET_STATE","TrajectoryView");
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
		MessageDispatcher.SendMessageData("TRAJECTORY_SELECTED",value);
    }
	void scaleChangeCallback(float value)
	{
		MessageDispatcher.SendMessageData("SET_SCALE", value);
		int index = pathDropdown.value;
		numCamera.text = models.Trajectories [index].num_camera.ToString();
		totalLength.text = models.Trajectories [index].totalLength.ToString();
		deltaX.text = models.Trajectories [index].dist_x.ToString();
		deltaY.text = models.Trajectories [index].dist_y.ToString();
		deltaZ.text = models.Trajectories [index].dist_z.ToString();
	}

	public void setTrajectoryDropdownCallback(IMessage rMessage)
	{
		int index = (int)rMessage.Data;
		pathDropdown.value = index;
		numCamera.text = models.Trajectories [index].num_camera.ToString();
		totalLength.text = models.Trajectories [index].totalLength.ToString();
		deltaX.text = models.Trajectories [index].dist_x.ToString();
		deltaY.text = models.Trajectories [index].dist_y.ToString();
		deltaZ.text = models.Trajectories [index].dist_z.ToString();

	}
	public void loadTrajectoryDropdownCallback(IMessage rMessage)
	{
		List<string> pathnames = (List<string>)rMessage.Data;
		pathDropdown.ClearOptions();
		List<Dropdown.OptionData> optlist = new List<Dropdown.OptionData>();
		foreach (string s in pathnames)
		{
			Dropdown.OptionData opt = new Dropdown.OptionData();
			opt.text = s;
			optlist.Add(opt);
		}
		pathDropdown.AddOptions(optlist);
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
