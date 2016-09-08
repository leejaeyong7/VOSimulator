/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : PLYOptions.cs
 * @brief      : Handles importing / using plys
 * Copyright (c) Jae Yong Lee / UIUC Fall 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class PLYMenu : MenuPanel{
	public CustomFileBrowser fb;
	public Button ImportPLY;
	public PLYModel model = null;
	//public IList<PLYModel>

	// Use this for initialization
	void Start () {
		ImportPLY.onClick.AddListener (delegate {
			fb.showBrowser("ply",importPLY);
		});
	}
	
	// Update is called once per frame
	void Update () {

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
	void importPLY( System.IO.FileInfo filename){
		model = new PLYModel ();
		Terrain t = Terrain.activeTerrain;
		float t_width = t.terrainData.size.x;
		float t_height = t.terrainData.size.z;
		GameObject plyObjectContainer = new GameObject ("pointclouds");
		plyObjectContainer.transform.Translate (new Vector3 (t_width / 2, 0, t_height / 2));
		if (model.loadFile (filename)) {
			if (model.createMesh ()) {
				for (int m = 0; m < model.meshes.Count; m++) {
					GameObject plyObject = new GameObject ("pointcloud_"+m.ToString());
					plyObject.transform.parent = plyObjectContainer.transform;
					MeshRenderer mr = plyObject.AddComponent<MeshRenderer> ();
					MeshFilter mf = plyObject.AddComponent<MeshFilter> ();
					mf.mesh = model.meshes [m];
					mr.material = new Material(Shader.Find("Custom/PointShader"));
					plyObject.transform.Translate (new Vector3 (t_width / 2, 0, t_height / 2));
					plyObject.transform.Rotate (new Vector3 (270, 0, 0));
//					GameObject sub_ply = new GameObject ();
//					Instantiate (sub_ply);
//					MeshFilter mf = plyObject.AddComponent<MeshFilter> ();
//					mf.mesh = model.meshes [m];
//					sub_ply.transform.SetParent (plyObject.transform);
				}
			} else {
				print ("invalid file");
			}
		} else {
			print("error loading file");
		}
		t.drawHeightmap = false;
	}
	//--------------------------------------------------------------------//
	//                  END PRIVATE FUNCTION DEFINITIONS                  //
	//--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
