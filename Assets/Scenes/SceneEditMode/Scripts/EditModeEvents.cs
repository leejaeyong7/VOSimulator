/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : EditModeEvents.cs
 * @brief      : Event handler for Edit mode scene in VOS
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
public class EditModeEvents : MonoBehaviour {
    public ObjectMenuEvents objectMenuEvents;
    public ObjectEvents objEvents;
	public GameObject selectedObject;
	private Shader[] defaultShaders;
	private Shader highlighter;
	void Start(){
		highlighter = Shader.Find ("Custom/GlowShader");
		print (highlighter.ToString ());
	}
    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
	public void selectObject(GameObject obj){
		// check if any object selected, if so, unselect object
		if (selectedObject) {
			unselectObject ();
		}
		selectedObject = obj;
		MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer> ();
		defaultShaders = new Shader[meshRenderer.materials.Length];
		for (int i = 0; i < meshRenderer.materials.Length; i++) {
			defaultShaders [i] = meshRenderer.materials [i].shader;
		}
		highlightObject ();
	}
	public void unselectObject(){
		if (selectedObject) {
			unhighlightObject ();
		}
		defaultShaders = null;
		selectedObject = null;
	}
    //--------------------------------------------------------------------//
    //                  END PUBLIC FUNCTION DEFINITIONS                   //
    //--------------------------------------------------------------------//
    //--------------------------------------------------------------------//
    //                    PRIVATE FUNCTION DEFINITIONS                    //
    //--------------------------------------------------------------------//
	void highlightObject(){
		MeshRenderer meshRenderer = 
			selectedObject.GetComponent<MeshRenderer> ();
		foreach(Material m in meshRenderer.materials) {
			m.shader = highlighter;
		}
	}
	void unhighlightObject(){
		if (defaultShaders != null) {
			MeshRenderer meshRenderer = 
				selectedObject.GetComponent<MeshRenderer> ();
			for (int i = 0; i < meshRenderer.materials.Length; i++) {
				meshRenderer.materials [i].shader = defaultShaders [i];
			}
		}
	}
    //--------------------------------------------------------------------//
    //                  END PRIVATE FUNCTION DEFINITIONS                  //
    //--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
