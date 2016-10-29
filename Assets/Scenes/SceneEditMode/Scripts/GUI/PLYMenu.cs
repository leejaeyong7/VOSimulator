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
using CielaSpike;
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
			fb.showBrowser("ply",importPLYThread);
		});
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

	void importPLYThread(System.IO.FileInfo filename){
		StartCoroutine (importPLY(filename));
	}


	IEnumerator importPLY( System.IO.FileInfo filename){
		model = new PLYModel ();
		Terrain t = Terrain.activeTerrain;
		float t_width = t.terrainData.size.x;
		float t_height = t.terrainData.size.z;

		Task task;
		this.StartCoroutineAsync (model.loadFile(filename),out task);
		if (task.State == TaskState.Error) {
			yield return Ninja.JumpToUnity;
			print (task.Exception);
			yield return Ninja.JumpBack;
			yield break;
		}
        yield return StartCoroutine(task.Wait());
		if (task.State == TaskState.Done) {
			if (model.isPointcloud) {
				this.StartCoroutineAsync (model.createPointCloud (),out task);
				if (task.State == TaskState.Error) {
					yield return Ninja.JumpToUnity;
					print (task.Exception);
					yield return Ninja.JumpBack;
					yield break;
				}
				yield return StartCoroutine(task.Wait());
				if (task.State == TaskState.Done) {
					// append object, set terrain to null;
					yield return Ninja.JumpToUnity;
					GameObject plyObjectContainer;
					if (plyObjectContainer = GameObject.Find ("pointclouds")) {
						foreach (Transform g in plyObjectContainer.transform) {
							GameObject.Destroy (g.gameObject);
						}
					} else {
						plyObjectContainer = new GameObject ("pointclouds");
					}
//					plyObjectContainer.transform.Translate (new Vector3 (t_width / 2, 0, t_height / 2));
					for (int m = 0; m < model.meshes.Count; m++) {
						GameObject plyObject = new GameObject ("pointcloud_" + m.ToString ());
						plyObject.transform.parent = plyObjectContainer.transform;
						MeshRenderer mr = plyObject.AddComponent<MeshRenderer> ();
						MeshFilter mf = plyObject.AddComponent<MeshFilter> ();
						mf.mesh = model.meshes [m];
						mr.material = new Material (Shader.Find ("Custom/PointShader"));
//						plyObject.transform.Translate (new Vector3 (t_width / 2, 0, t_height / 2));
//						plyObject.transform.Rotate (new Vector3 (270, 0, 0));
					}
					t.drawHeightmap = false;
					yield return Ninja.JumpBack;
					yield break;
				}
			} else {
				this.StartCoroutineAsync (model.createObject(),out task);
				if (task.State == TaskState.Error) {
					yield return Ninja.JumpToUnity;
					print (task.Exception);
					yield return Ninja.JumpBack;
					yield break;
				}
				yield return StartCoroutine(task.Wait());
				if (task.State == TaskState.Done) {
					yield return Ninja.JumpToUnity;
					GameObject plyObjectContainer;
					if (plyObjectContainer = GameObject.Find ("pointcloudMesh")) {
						foreach (Transform g in plyObjectContainer.transform) {
							GameObject.Destroy (g.gameObject);
						}
					} else {
						plyObjectContainer = new GameObject ("pointcloudMesh");
					}
//					plyObjectContainer.transform.Translate (new Vector3 (t_width / 2, 0, t_height / 2));
					for (int m = 0; m < model.meshes.Count; m++) {
						GameObject plyObject = new GameObject ("pointcloud_" + m.ToString ());
						plyObject.transform.parent = plyObjectContainer.transform;
						MeshRenderer mr = plyObject.AddComponent<MeshRenderer> ();
						MeshFilter mf = plyObject.AddComponent<MeshFilter> ();
						MeshCollider mc = plyObject.AddComponent<MeshCollider> ();
						mr.material = new Material (Shader.Find ("Custom/PointShader"));
						mc.sharedMesh = model.meshes [m];
						mf.mesh = model.meshes [m];
//						plyObject.transform.Translate (new Vector3 (t_width / 2, 0, t_height / 2));
//						plyObject.transform.Rotate (new Vector3 (270, 0, 0));
					}
					t.drawHeightmap = false;
					yield return Ninja.JumpBack;
					yield break;
				}
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
