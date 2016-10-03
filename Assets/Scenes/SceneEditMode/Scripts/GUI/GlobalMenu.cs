/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : GlobalMenuEvents.cs
 * @brief      : Event handler for Global Panel
 * Copyright (c) Jae Yong Lee / UIUC Summer 2016
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
public class GlobalMenu : MonoBehaviour {
	public Dropdown globalMenuDropdown;
	public EnvPanel EnvPanel;
	public FilePanel FilePanel;
	public CameraPanel CameraPanel;
	public RunPanel RunPanel;

    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
    // Use this for initialization
    void Start () {
		// initialize global menu
        globalMenuDropdown.onValueChanged.AddListener(delegate {
            chooseGlobalMenuType(globalMenuDropdown);
        });
        globalMenuDropdown.onValueChanged.Invoke(0);
    }
    //--------------------------------------------------------------------//
    //                  END PUBLIC FUNCTION DEFINITIONS                   //
    //--------------------------------------------------------------------//
    //--------------------------------------------------------------------//
    //                    PRIVATE FUNCTION DEFINITIONS                    //
    //--------------------------------------------------------------------//
	// global menu dropdown select event handler
    private void chooseGlobalMenuType(Dropdown target)
    {
        switch (target.value)
        {
		case 0: // File Panel
			EnvPanel.Hide();
			FilePanel.Show();
			CameraPanel.Hide();
			RunPanel.Hide();
			break;
		case 1: // Edit Panel
			EnvPanel.Show();
			FilePanel.Hide();
			CameraPanel.Hide();
			RunPanel.Hide();
            break;
		case 2: // Camera Panel
			EnvPanel.Hide();
			FilePanel.Hide();
			CameraPanel.Show();
			RunPanel.Hide();
			break;
		case 3: // Run Panel
			EnvPanel.Hide();
			FilePanel.Hide();
			CameraPanel.Hide();
			RunPanel.Show();
			break;
        default:
            break;

        }
    }

    private void setGlobalMenuType(int index)
    {
        globalMenuDropdown.value = index;
    }
    //--------------------------------------------------------------------//
    //                  END PRIVATE FUNCTION DEFINITIONS                  //
    //--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
