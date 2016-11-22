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
	void OnEnable()
	{
		MessageDispatcher.SendMessageData("SET_STATE", "TrajectoryCreate");
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
		MessageDispatcher.SendMessage("IMPORT_TRAJECTORY");
    }
    void noiseMeanUSliderCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_2D_NOISE_MU_U",value);

    }
    void noiseMeanVSliderCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_2D_NOISE_MU_V",value);

    }
    void noiseSTDUSliderCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_2D_NOISE_SIG_U",value);

    }
    void noiseSTDVSliderCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_2D_NOISE_SIG_V",value);

    }
    void maxFeatureNumSliderCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_MAX_FEATURE_POINT",value);

    }
    void flipRateSliderCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_FLIPRATE",value);

    }
    void dropRateSliderCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_DROPRATE",value);
    }
    void FocalLengthSliderCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_FOCAL_LENGTH",value);

    }
    void FOVSliderCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_FOV",value);

    }
    void AspectSliderCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_ASPECT",value);
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