/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : ReliefOptions.cs
 * @brief      : Event handler for relief option UI
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
 * 
 */
public class ReliefOptions : MonoBehaviour
{
    //********************************************************************//
    //***************************BEGIN VARIABLES**************************//
    //********************************************************************//
    //====================================================================//
    //                    PUBLIC VARIABLE DEFINITIONS                     //
    //====================================================================//
    public Toggle toggleTerrainOn;
    public Toggle toggleTerrainOff;
    public Slider terrainReliefBrushRadius;
    public Slider terrainReliefBrushHeight;
    public Slider terrainReliefBrushSTD;
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
     * Attaches Event callbacks
     */
    void Start()
    {
        toggleTerrainOn.onValueChanged.AddListener(
            toggleTerrainOnCallback);

        terrainReliefBrushRadius.onValueChanged.AddListener(
            terrainReliefBrushRadiusCallback);

        terrainReliefBrushHeight.onValueChanged.AddListener(
            terrainReliefBrushHeightCallback);

        terrainReliefBrushSTD.onValueChanged.AddListener(
            terrainReliefBrushSTDCallback);
    }

    void OnEnable()
    {
        terrainReliefBrushRadiusCallback(terrainReliefBrushRadius.value);
        terrainReliefBrushHeightCallback(terrainReliefBrushHeight.value);
        terrainReliefBrushSTDCallback(terrainReliefBrushSTD.value);
        MessageDispatcher.SendMessageData("SET_STATE", "TerrainRelief");
		MessageDispatcher.SendMessage("TERRAIN_RELIEF_ENABLED");
    }

    void OnDisable()
    {
		MessageDispatcher.SendMessage("TERRAIN_RELIEF_DISABLED");
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
    void toggleTerrainOnCallback(bool ison)
    {
        if (ison)
        {

			MessageDispatcher.SendMessageData("TOGGLE_TERRAIN", true);
        }
        else
        {
			MessageDispatcher.SendMessageData("TOGGLE_TERRAIN", false);
        }
    }

    void terrainReliefBrushRadiusCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_TERRAIN_BRUSH_RADIUS", value);
    }

    void terrainReliefBrushHeightCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_TERRAIN_RELIEF_HEIGHT", value);
    }

    void terrainReliefBrushSTDCallback(float value)
    {
		MessageDispatcher.SendMessageData("SET_TERRAIN_RELIEF_STD", value);
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
