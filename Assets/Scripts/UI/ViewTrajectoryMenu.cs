﻿/*============================================================================
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
	}
    void numCameraHelper(IMessage rMessage)
    {
		int num= (int)rMessage.Data;
		numCamera.text = num.ToString();
    }
    void totalLengthHelper(IMessage rMessage)
    {
		float len = (float)rMessage.Data;
		totalLength.text = len.ToString();
    }
    void deltaXHelper(IMessage rMessage)
    {
		float len = (float)rMessage.Data;
		deltaX.text = len.ToString();
    }
    void deltaYHelper(IMessage rMessage)
    {
		float len = (float)rMessage.Data;
		deltaY.text = len.ToString();
    }
    void deltaZHelper(IMessage rMessage)
    {
		float len = (float)rMessage.Data;
		deltaZ.text = len.ToString();
    }

	public void setTrajectoryDropdownCallback(IMessage rMessage)
	{
		int index = (int)rMessage.Data;
		pathDropdown.value = index;
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
