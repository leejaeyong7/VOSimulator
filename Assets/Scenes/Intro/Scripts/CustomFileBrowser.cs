/*============================================================================
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
	private string allowedExtension = null;

	//initialize file browser
	FileBrowser fb = new FileBrowser();
	System.IO.FileInfo output = null;

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
				output = fb.outputFile;
				showFB = false;
			}
		}
	}
	public System.IO.FileInfo getSelectedFile(){
		return output;
	}
	public void showBrowser(string extension){
		showFB = true;
		allowedExtension = extension;
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

