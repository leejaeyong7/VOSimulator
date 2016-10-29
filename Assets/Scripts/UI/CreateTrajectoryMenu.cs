/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : CreateTrajectoryMenu.cs
 * @brief      : Event handler for trajectory create UI
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
public class CreateTrajectoryMenu : MonoBehaviour
{

    //********************************************************************//
    //***************************BEGIN VARIABLES**************************//
    //********************************************************************//
    //====================================================================//
    //                    PUBLIC VARIABLE DEFINITIONS                     //
    //====================================================================//
    public Button importTrajectoryButton;
    public Slider noiseMeanUSlider;
    public Slider noiseMeanVSlider;
    public Slider noiseSTDUSlider;
    public Slider noiseSTDVSlider;
    public Slider maxFeatureNumSlider;
    public Slider flipRateSlider;
    public Slider dropRateSlider;
    public Slider FocalLengthSlider;
    public Slider FOVSlider;
    public Slider AspectSlider;
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
        importTrajectoryButton.onClick.AddListener(importTrajectoryButtonCallback);
        noiseMeanUSlider.onValueChanged.AddListener(noiseMeanUSliderCallback);
        noiseMeanVSlider.onValueChanged.AddListener(noiseMeanVSliderCallback);
        noiseSTDUSlider.onValueChanged.AddListener(noiseSTDUSliderCallback);
        noiseSTDVSlider.onValueChanged.AddListener(noiseSTDVSliderCallback);
        maxFeatureNumSlider.onValueChanged.AddListener(maxFeatureNumSliderCallback);
        flipRateSlider.onValueChanged.AddListener(flipRateSliderCallback);
        dropRateSlider.onValueChanged.AddListener(dropRateSliderCallback);
        FocalLengthSlider.onValueChanged.AddListener(FocalLengthSliderCallback);
        FOVSlider.onValueChanged.AddListener(FOVSliderCallback);
        AspectSlider.onValueChanged.AddListener(AspectSliderCallback);
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
    void importTrajectoryButtonCallback()
    {

    }
    void noiseMeanUSliderCallback(float value)
    {

    }
    void noiseMeanVSliderCallback(float value)
    {

    }
    void noiseSTDUSliderCallback(float value)
    {

    }
    void noiseSTDVSliderCallback(float value)
    {

    }
    void maxFeatureNumSliderCallback(float value)
    {

    }
    void flipRateSliderCallback(float value)
    {

    }
    void dropRateSliderCallback(float value)
    {

    }
    void FocalLengthSliderCallback(float value)
    {

    }
    void FOVSliderCallback(float value)
    {

    }
    void AspectSliderCallback(float value)
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