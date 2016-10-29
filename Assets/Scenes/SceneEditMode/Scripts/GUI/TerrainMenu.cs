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
public class TerrainMenu : MenuPanel{
	public Dropdown terrainDropdown;
	public bool isClicked = false;
	public TextureOptions TextureOptions;
	public ReliefOptions ReliefOptions;
    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
    void Start () {
		terrainDropdown.onValueChanged.AddListener(delegate {
			chooseTerrainOptionType(terrainDropdown);
		});
        terrainDropdown.onValueChanged.Invoke(0);
    }
	new public void Show(){
		base.Show ();
        terrainDropdown.onValueChanged.Invoke (0);
	}
    new public void Hide()
    {
        base.Hide();
        TextureOptions.Hide();
        ReliefOptions.Hide();
    }
    //--------------------------------------------------------------------//
    //                  END PUBLIC FUNCTION DEFINITIONS                   //
    //--------------------------------------------------------------------//
    //--------------------------------------------------------------------//
    //                    PRIVATE FUNCTION DEFINITIONS                    //
    //--------------------------------------------------------------------//
	// edit menu dropdown select event handler
	private void chooseTerrainOptionType(Dropdown target)
	{
		switch (target.value) {
		case 0:
            ReliefOptions.Hide();
            TextureOptions.Show ();
			break;
		case 1:
			TextureOptions.Hide();
			ReliefOptions.Show();
			break;
		case 2:
			TextureOptions.Hide ();
			ReliefOptions.Hide ();
			break;
		default:
			break;

		}
	}
    //--------------------------------------------------------------------//
    //                  END PRIVATE FUNCTION DEFINITIONS                  //
    //--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//

