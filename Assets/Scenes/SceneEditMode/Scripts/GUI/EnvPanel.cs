/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : EnvironmentMenuEvents.cs
 * @brief      : Event handler for Environment menu
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
public class EnvPanel : MenuPanel{
	public Dropdown editMenuDropdown;
	public TerrainMenu TerrainMenu;
	public EnvMenu EnvMenu;
	public PLYMenu PLYMenu;

	void Start(){
		editMenuDropdown.onValueChanged.AddListener(delegate {
			chooseEditMenuType(editMenuDropdown);
		});
		editMenuDropdown.onValueChanged.Invoke(0);
	}
	new public void Show(){
		base.Show ();
		editMenuDropdown.onValueChanged.Invoke(0);
	}
	new public void Hide(){
		base.Hide ();

	}
	//--------------------------------------------------------------------//
	//                    PUBLIC FUNCTION DEFINITIONS                     //
	//--------------------------------------------------------------------//

	//--------------------------------------------------------------------//
	//                  END PUBLIC FUNCTION DEFINITIONS                   //
	//--------------------------------------------------------------------//
	//--------------------------------------------------------------------//
	//                    PRIVATE FUNCTION DEFINITIONS                    //
	//--------------------------------------------------------------------//
	// edit menu dropdown select event handler
	private void chooseEditMenuType(Dropdown target)
	{
		switch (target.value)
		{
		case 0:
			TerrainMenu.Show ();
			EnvMenu.Hide ();
			PLYMenu.Hide ();
			break;
		case 1:
			TerrainMenu.Hide ();
			EnvMenu.Show ();
			PLYMenu.Hide ();
			break;
		case 2:
			TerrainMenu.Hide ();
			EnvMenu.Hide();
			PLYMenu.Show();
			break;
		default:
			break;

		}
	}

	private void setEditMenuType(int index)
	{
		editMenuDropdown.value = index;
	}
	//--------------------------------------------------------------------//
	//                  END PRIVATE FUNCTION DEFINITIONS                  //
	//--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//

