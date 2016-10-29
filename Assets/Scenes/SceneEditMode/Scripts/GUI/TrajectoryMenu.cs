/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : TrajectoryMenu.cs
 * @brief      : Trajectory menu event handler
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
using System.Linq;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class TrajectoryMenu : MenuPanel {
	public CustomFileBrowser fb;
	public Toggle DisplayTrajectory;
	public Dropdown TrajectoryType;
	public Button import;
	public Button align;

	public Slider Scale;
	void Start(){
		import.onClick.AddListener (delegate {
			fb.showBrowser("pts",dispatchFileInfo);
		});

		align.onClick.AddListener (delegate {
			fb.showBrowser("out",dispatchAlign);
		});



		Scale.onValueChanged.AddListener (delegate(float arg) {
			MessageDispatcher.SendMessageData("SET_TRAJECTORY_SCALE",arg);
		});
	}

	void dispatchFileInfo(System.IO.FileInfo fp){
		MessageDispatcher.SendMessageData("IMPORT_TRAJECTORY",fp);
	}

	void dispatchAlign(System.IO.FileInfo fp){
		MessageDispatcher.SendMessageData ("ALIGN_TRAJECTORY", fp);
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
	//--------------------------------------------------------------------//
	//                  END PRIVATE FUNCTION DEFINITIONS                  //
	//--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
