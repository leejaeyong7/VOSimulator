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
using UnityEngine.UI;
using System.Collections;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class ObjectMenuEvents : MonoBehaviour
{
    public Dropdown ObjectMenuDropdown;
    public Terrain terrain;
    public GameObject PreviewBG;
    public GameObject TreeModel;
    public GameObject BuildingModel;
    public GameObject RobotModel;
	private MeshCollider mc;
    private RectTransform rt;
    private GameObject currObject;
    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
    void Start()
    {
        ObjectMenuDropdown.onValueChanged.AddListener(delegate {
			clearObject(PreviewBG);
            addObject(ObjectMenuDropdown);
        });
        currObject = null;
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
                GameObject treeClone = Instantiate(TreeModel);
                treeClone.transform.parent = PreviewBG.transform;
                rt = treeClone.AddComponent<RectTransform>();
                rt.localEulerAngles = new Vector3(270, 0, 0);
                rt.localScale = new Vector3(300, 300, 300);
                currObject = treeClone;
                break;
            case 1:// "Building":
                GameObject buildingClone = Instantiate(BuildingModel);
                buildingClone.transform.parent = PreviewBG.transform;
                rt = buildingClone.AddComponent<RectTransform>();
                rt.localScale = new Vector3(15, 15, 15);
                rt.localEulerAngles = new Vector3(0, 0, 0);
                currObject = buildingClone;
                break;
            case 2:// "Robot":
                GameObject robotClone = Instantiate(RobotModel);
                robotClone.transform.parent = PreviewBG.transform;
                rt = robotClone.AddComponent<RectTransform>();
                rt.localScale = new Vector3(35, 35, 35);
                rt.localEulerAngles = new Vector3(0, 180, 0);
                currObject = robotClone;
                break;
            default:
                break;
        }
        rt.anchorMax = new Vector2(1, 0.8f);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchoredPosition = new Vector2(0.5f, 0.5f);
        rt.localPosition = new Vector3(0, 0, 0);
        rt.offsetMax = new Vector2(0, 0);
        rt.offsetMin = new Vector2(0, 0);

        mc = currObject.AddComponent<MeshCollider>();
		mc.isTrigger = true;
		mc.convex = true;

        currObject.AddComponent<ObjectEvents>();
    }
    public void setDropDown(int index)
    {
        ObjectMenuDropdown.value = index;
    }
   
    public void removeCurrObject()
    {
        currObject = null;
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

