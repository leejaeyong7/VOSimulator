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
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
/**
 * 
 */
public class ObjectMenu : MonoBehaviour {
    //********************************************************************//
    //***************************BEGIN VARIABLES**************************//
    //********************************************************************//
    //====================================================================//
    //                    PUBLIC VARIABLE DEFINITIONS                     //
    //====================================================================//
    public Dropdown objectTypeDropdown;
    public Button objectAddButton;
    public Button objectEditButton;
    public Button objectRemoveButton;
    public Slider numberOfFeatureSlider;
    public Toggle featureSourceVertex;
    public Toggle featureSourceFace;
    public Toggle featureCollisionOn;
    public Toggle featureCollisionOff;
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
    // Use this for initialization
    void Start()
    {
        objectTypeDropdown.onValueChanged.AddListener(objectTypeDropdownCallback);
        objectAddButton.onClick.AddListener(objectAddButtonCallback);
        objectEditButton.onClick.AddListener(objectEditButtonCallback);
        objectRemoveButton.onClick.AddListener(objectRemoveButtonCallback);
        numberOfFeatureSlider.onValueChanged.AddListener(numberOfFeatureSliderCallback);
        featureSourceVertex.onValueChanged.AddListener(featureSourceVertexCallback);
        featureSourceFace.onValueChanged.AddListener(featureSourceFaceCallback);
        featureCollisionOn.onValueChanged.AddListener(featureCollisionOnCallback);
        featureCollisionOff.onValueChanged.AddListener(featureCollisionOffCallback);
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
    void objectTypeDropdownCallback(int value)
    {

    }
    void objectAddButtonCallback()
    {

    }
    void objectEditButtonCallback()
    {

    }
    void objectRemoveButtonCallback()
    {

    }
    void numberOfFeatureSliderCallback(float value)
    {

    }
    void featureSourceVertexCallback(bool ison)
    {

    }
    void featureSourceFaceCallback(bool ison)
    {

    }
    void featureCollisionOnCallback(bool ison)
    {

    }
    void featureCollisionOffCallback(bool ison)
    {

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
