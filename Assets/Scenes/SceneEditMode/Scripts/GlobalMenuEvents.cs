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
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class GlobalMenuEvents : MonoBehaviour {
    public Dropdown globalMenuDropdown;
    public GameObject TerrainMenu;
    public GameObject ObjectMenu;
    public GameObject LocalEditPanel;
    public ObjectMenuEvents objMenuEvents;
    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
    // Use this for initialization
    void Start () {
        globalMenuDropdown.onValueChanged.AddListener(delegate {
            chooseMenuType(globalMenuDropdown);
        });
        globalMenuDropdown.onValueChanged.Invoke(0);
    }
    //--------------------------------------------------------------------//
    //                  END PUBLIC FUNCTION DEFINITIONS                   //
    //--------------------------------------------------------------------//
    //--------------------------------------------------------------------//
    //                    PRIVATE FUNCTION DEFINITIONS                    //
    //--------------------------------------------------------------------//
    private void chooseMenuType(Dropdown target)
    {
        switch (target.value)
        {
            case 0:
                TerrainMenu.SetActive(true);
                ObjectMenu.SetActive(false);
                LocalEditPanel.SetActive(false);
                break;
            case 1:
                TerrainMenu.SetActive(false);
                ObjectMenu.SetActive(true);
                LocalEditPanel.SetActive(true);
                objMenuEvents.ObjectMenuDropdown.onValueChanged.Invoke(0);
                break;
            case 2:
                TerrainMenu.SetActive(false);
                ObjectMenu.SetActive(false);
                LocalEditPanel.SetActive(false);
                break;
            default:
                break;

        }
    }
    private void setMenuType(int index)
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
