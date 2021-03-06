﻿/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : CustomFileBrowser.cs
 * @brief      : Event handler for selected Object
 * Copyright (c) Jae Yong Lee / UIUC Summer 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class CustomFileBrowser : MonoBehaviour {
	//skins and textures
	public GUISkin skin;
	public Texture2D file,folder,back,drive;
	public bool showFB = false;
	public delegate void CallbackFunc( System.IO.FileInfo f);
	protected CallbackFunc cb = null;
	//initialize file browser
	FileBrowser fb = new FileBrowser();

	// Use this for initialization
	void Start () {
		fb.guiSkin = skin;
		fb.fileTexture = file; 
		fb.directoryTexture = folder;
		fb.backTexture = back;
		fb.driveTexture = drive;
		fb.showSearch = true;
		fb.searchRecursively = true;
	}

	void OnGUI(){
		if (showFB) {
			if (fb.draw ()) {
				if (cb != null && fb.outputFile != null) {
					cb (fb.outputFile);
				}
				cb = null;
				showFB = false;
			}
		}
	}
	public void showBrowser(string extension, CallbackFunc f){
		cb = f;
		fb.searchPattern = "*." + extension;
		showFB = true;
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

