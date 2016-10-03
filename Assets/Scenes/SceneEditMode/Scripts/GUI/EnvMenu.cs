/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : ObjectMenuEvents.cs
 * @brief      : Event handler for Object Menu
 * Copyright (c) Jae Yong Lee / UIUC Summer 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class EnvMenu : MenuPanel
{
    public Dropdown ObjectMenuDropdown;
    public Terrain terrain;
    public RawImage preview;
    public GameObject TreeModel;
    public GameObject BuildingModel;
    public GameObject RobotModel;
	private Texture2D previewTexture;
	private MeshCollider mc;
    private RectTransform rt;
    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
    void Start()
    {
        ObjectMenuDropdown.onValueChanged.AddListener(delegate {
			//clearObject(PreviewBG);
            addObject(ObjectMenuDropdown);
        });
		ObjectMenuDropdown.onValueChanged.Invoke(0);

    }
	private void clearObject(GameObject obj){
		if (obj.transform.childCount > 0) {
			GameObject preview = obj.transform.GetChild (0).gameObject;
			Destroy (preview);
		}
	}
    private void addObject(Dropdown target)
    {
        switch (target.value)
        {
		case 0:// "Tree"
			previewTexture = AssetPreview.GetAssetPreview (TreeModel);
			preview.texture = previewTexture;
            break;
		case 1:// "Building":
			previewTexture = AssetPreview.GetAssetPreview (BuildingModel);
			preview.texture = previewTexture;
            break;
		case 2:// "Robot":
			previewTexture = AssetPreview.GetAssetPreview (RobotModel);
			preview.texture = previewTexture;
            break;
        default:
            break;
        }
        /*rt.anchorMax = new Vector2(1, 0.8f);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchoredPosition = new Vector2(0.5f, 0.5f);
        rt.localPosition = new Vector3(0, 0, 0);
        rt.offsetMax = new Vector2(0, 0);
        rt.offsetMin = new Vector2(0, 0);*/


		/*MeshRenderer[] rend = 
			currObject.GetComponentsInChildren<MeshRenderer> ();
		SkinnedMeshRenderer[] srend = 
			currObject.GetComponentsInChildren<SkinnedMeshRenderer> ();
		foreach (MeshRenderer r in rend) {
			r.gameObject.AddComponent<MeshCollider> ();
		}
		foreach (SkinnedMeshRenderer s in srend) {
			s.gameObject.AddComponent<MeshCollider> ();
		}*/
		//mc = currObject.AddComponent<MeshCollider> ();
		//mc.isTrigger = true;
		//mc.convex = true;

        //currObject.AddComponent<ObjectEvents>();
    }
    public void setDropDown(int index)
    {
        ObjectMenuDropdown.value = index;
    }
   
    public void removeCurrObject()
    {
    }
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

