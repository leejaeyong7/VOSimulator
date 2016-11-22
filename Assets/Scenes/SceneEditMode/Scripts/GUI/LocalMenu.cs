/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : LocalMenuEvents.cs
 * @brief      : Event handler for local menu interactions
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
public class LocalMenu: MenuPanel{
    public IOHandler io;
    public GameObject currObj;
    public InputField posX;
    public InputField posY;
    public InputField posZ;

    public InputField rotX;
    public InputField rotY;
    public InputField rotZ;

    public InputField scaleX;
    public InputField scaleY;
    public InputField scaleZ;
    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
    //--------------------------------------------------------------------//
    //                  END PUBLIC FUNCTION DEFINITIONS                   //
    //--------------------------------------------------------------------//
    //--------------------------------------------------------------------//
    //                    PRIVATE FUNCTION DEFINITIONS                    //
    //--------------------------------------------------------------------//
    void Start()
    {
        posX.onValueChanged.AddListener(delegate (string valuestr)
        {
            float value = float.Parse(valuestr);
            Vector3 pos = currObj.transform.position;
            if (currObj != null)
            {
                currObj.transform.position = new Vector3(value, pos.y, pos.z);
            }
        });
        
        posY.onValueChanged.AddListener(delegate (string valuestr)
        {
            float value = float.Parse(valuestr);
            Vector3 pos = currObj.transform.position;
            if (currObj != null)
            {
                currObj.transform.position = new Vector3(pos.x, value, pos.z);
            }
        });

        posZ.onValueChanged.AddListener(delegate (string valuestr)
        {
            float value = float.Parse(valuestr);
            Vector3 pos = currObj.transform.position;
            if (currObj != null)
            {
                currObj.transform.position = new Vector3(pos.x, pos.y, value);
            }
        });
        /*
        rotX.onValueChanged.AddListener(delegate (string valuestr)
        {
            float value = float.Parse(valuestr);
            Vector3 rot = currObj.transform.rotation.eulerAngles;
            if (currObj != null)
            {
                currObj.transform.rotation = Quaternion.EulerAngles(value, rot.y, rot.z);
            }
        });

        rotY.onValueChanged.AddListener(delegate (string valuestr)
        {
            float value = float.Parse(valuestr);
            Vector3 rot = currObj.transform.rotation.eulerAngles;
            if (currObj != null)
            {
                currObj.transform.rotation = Quaternion.EulerAngles(rot.x,value, rot.z);
            }
        });

        rotZ.onValueChanged.AddListener(delegate (string valuestr)
        {
            float value = float.Parse(valuestr);
            Vector3 rot = currObj.transform.rotation.eulerAngles;
            if (currObj != null)
            {
                currObj.transform.rotation = Quaternion.EulerAngles(rot.x, rot.y, value);
            }
        });*/

        scaleX.onValueChanged.AddListener(delegate (string valuestr)
        {
            float value = float.Parse(valuestr);
            Vector3 scale = currObj.transform.localScale;
            if (currObj != null)
            {
                currObj.transform.localScale = new Vector3(value, scale.y, scale.z);
            }
        });
        scaleY.onValueChanged.AddListener(delegate (string valuestr)
        {
            float value = float.Parse(valuestr);
            Vector3 scale = currObj.transform.localScale;
            if (currObj != null)
            {
                currObj.transform.localScale = new Vector3(scale.x, value, scale.z);
            }
        });
        scaleZ.onValueChanged.AddListener(delegate (string valuestr)
        {
            float value = float.Parse(valuestr);
            Vector3 scale = currObj.transform.localScale;
            if (currObj != null)
            {
                currObj.transform.localScale = new Vector3(scale.x, scale.y, value);
            }
        });
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            RaycastHit hit;
            bool hited = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            if (hited)
            {
                if (hit.collider.gameObject.tag != "Gizmo" && hit.collider.gameObject.layer != 8)
                {
                    if (!io.isGUIClicked())
                    {
                        currObj = null;

                    }
                }
            }
        }
        if(currObj != null)
        {
            Vector3 pos = currObj.transform.position;
            Vector3 rot = currObj.transform.rotation.eulerAngles;
            Vector3 scale = currObj.transform.localScale;
            if (!posX.isFocused)
            {
                posX.text = pos.x.ToString();
            }
            if (!posY.isFocused)
            {
                posY.text = pos.y.ToString();
            }
            if (!posZ.isFocused)
            {
                posZ.text = pos.z.ToString();
            }
            if (!rotX.isFocused)
            {
                rotX.text = rot.x.ToString();
            }
            if (!rotY.isFocused)
            {
                rotY.text = rot.y.ToString();
            }
            if (!rotZ.isFocused)
            {
                rotZ.text = rot.z.ToString();
            }
            if (!scaleX.isFocused)
            {
                scaleX.text = scale.x.ToString();
            }
            if (!scaleY.isFocused)
            {
                scaleY.text = scale.y.ToString();
            }
            if (!scaleZ.isFocused)
            {
                scaleZ.text = scale.z.ToString();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    //--------------------------------------------------------------------//
    //                  END PRIVATE FUNCTION DEFINITIONS                  //
    //--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//

