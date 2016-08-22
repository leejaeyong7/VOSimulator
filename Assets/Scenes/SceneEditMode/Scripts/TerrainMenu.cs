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
public class TerrainMenu : MenuPanel{
	public Dropdown terrainDropdown;
	public TextureOptions TextureOptions;
	public ReliefOptions ReliefOptions;
    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
    void Start () {
		terrainDropdown.onValueChanged.AddListener(delegate {
			chooseTextureOptionType(terrainDropdown);
		});
    }
	new public void Show(){
		base.Show ();
		terrainDropdown.onValueChanged.Invoke (0);
	}
    public void loadTerrain(string type) {
    }
    //--------------------------------------------------------------------//
    //                  END PUBLIC FUNCTION DEFINITIONS                   //
    //--------------------------------------------------------------------//
    //--------------------------------------------------------------------//
    //                    PRIVATE FUNCTION DEFINITIONS                    //
    //--------------------------------------------------------------------//
    private void UpdateTerrainTexture(int textureId)
    {

    }
	// edit menu dropdown select event handler
	private void chooseTextureOptionType(Dropdown target)
	{
		switch (target.value) {
		case 0:
			TextureOptions.Show ();
			ReliefOptions.Hide ();
			break;
		case 1:
			TextureOptions.Hide();
			ReliefOptions.Show();
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

