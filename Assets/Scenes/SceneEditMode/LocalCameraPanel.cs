/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : CameraPanel.cs
 * @brief      : Event handler for trajectory settings
 * Copyright (c) Jae Yong Lee / UIUC Fall 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class LocalCameraPanel : MenuPanel{
	// dropdown for selecting menu panels
	public Dropdown cameraMenuDropdown;
	public NoiseMenu noiseMenu;
	public CameraParamMenu globalCameraMenu;
	public TrajectoryMenu trajectoryMenu;
	public TransformationMenu transformationMenu;


	// ptr to local menu
	public LocalMenu localMenu;

	public float posX = 0;
	public float posY = 0;
	public float posZ = 0;

	public float rotX = 0;
	public float rotY = 0;
	public float rotZ = 0;

	// holds local camera setup values
	public float var_x_2d = 0;
	public float var_y_2d = 0;
	public float var_x_3d = 0;
	public float var_y_3d = 0;
	public float var_z_3d = 0;

	public float focalLength = 0;
	public float fovy = 0;
	public float aspect = 0;
	public float speed = 0;
	public float fps = 0;

	// private vars for raycast on camera frustrums
	private Ray ray;
	private RaycastHit hit;


	void Start(){
		cameraMenuDropdown.onValueChanged.AddListener(delegate {
			chooseCameraMenuType(cameraMenuDropdown);
		});
		cameraMenuDropdown.onValueChanged.Invoke(0);
	}

	// Update is called once per frame
	void Update () {

	}
	new public void Show(){
		base.Show ();
		cameraMenuDropdown.onValueChanged.Invoke (0);
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
	void chooseCameraMenuType(Dropdown target){

		switch (target.value)
		{
		case 0:
			noiseMenu.Show ();
			globalCameraMenu.Hide ();
			trajectoryMenu.Hide ();
			transformationMenu.Hide();
			break;
		case 1:
			noiseMenu.Hide ();
			globalCameraMenu.Show ();
			trajectoryMenu.Hide ();
			transformationMenu.Hide ();
			break;
		case 2:
			noiseMenu.Hide ();
			globalCameraMenu.Hide ();
			trajectoryMenu.Show ();
			transformationMenu.Hide ();
			break;
		case 3:
			noiseMenu.Hide ();
			globalCameraMenu.Hide ();
			trajectoryMenu.Hide ();
			transformationMenu.Show ();
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