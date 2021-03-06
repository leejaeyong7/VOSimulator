﻿/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : FileMenu.cs
 * @brief      : Event handler for File menu UI
 * Copyright (c) Jae Yong Lee / UIUC Fall 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.ootii.Messages;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
/**
 * FileMenu Class
 * 
 * Attached to Global Menu Control. Handles All project file related actions
 * 
 */
public class FileMenu : MonoBehaviour {
    //********************************************************************//
    //***************************BEGIN VARIABLES**************************//
    //********************************************************************//
    //====================================================================//
    //                    PUBLIC VARIABLE DEFINITIONS                     //
    //====================================================================//
    // GUI buttons
    public Button newButton;
    public Button saveButton;
    public Button loadButton;
    public Button helpButton;
    public Button quitButton;
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
    /**
     * Initialization.Attaches callback functions to UI
     */
    void Start()
    {
        // attach callback functions for buttons
        newButton.onClick.AddListener(newButtonCallback);
        saveButton.onClick.AddListener(saveButtonCallback);
        loadButton.onClick.AddListener(loadButtonCallback);
        helpButton.onClick.AddListener(helpButtonCallback);
        quitButton.onClick.AddListener(quitButtonCallback);
    }

    /**
     * Sets state to FILE in IO_STATE handler if this tab is enabled
     */
    void OnEnable()
    {
		MessageDispatcher.SendMessage("FILE_MENU_ENABLED");
    }
	void OnDisable()
	{
		MessageDispatcher.SendMessage("FILE_MENU_DISABLED");
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
    /**
     * Creates Empty project. Should Prompt User when clicked
     */
    void newButtonCallback()
    {
		MessageDispatcher.SendMessage("FILE_MENU_NEW_PRESSED");
    }

    /**
     * Loads Project from existing vosproj file
     */
    void loadButtonCallback()
    {
		MessageDispatcher.SendMessage("FILE_MENU_LOAD_PRESSED");
    }

    /**
     * Save Project to vosproj file
     */
    void saveButtonCallback()
    {
		MessageDispatcher.SendMessage("FILE_MENU_SAVE_PRESSED");
    }

    /**
     * Displays Help Screen Above all UI
     */
    void helpButtonCallback()
    {
		MessageDispatcher.SendMessage("FILE_MENU_HELP_PRESSED");
    }

    /**
     * Quits Program
     */
    void quitButtonCallback()
    {
        Application.Quit();
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
