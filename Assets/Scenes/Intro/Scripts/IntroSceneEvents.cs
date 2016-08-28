/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : IntroSceneEvents.cs
 * @brief      : Event handler for Intro scene in VOS
 * Copyright (c) Jae Yong Lee / UIUC Summer 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class IntroSceneEvents : MonoBehaviour {
    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
    /**
     * @brief Loads scene(.unity) file using index from build
     * @action next scene loaded
     */
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    /**
     * @brief Loads scene using scene name
     * @action next scene loaded
     */
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    //--------------------------------------------------------------------//
    //                  END PUBLIC FUNCTION DEFINITIONS                   //
    //--------------------------------------------------------------------//
    //--------------------------------------------------------------------//
    //                    PRIVATE FUNCTION DEFINITIONS                    //
    //--------------------------------------------------------------------//
    /**
     * @brief function to be called at the very start of intro page
     * @action disables unused project panel(s)
     */
    void Start()
	{
		GameObject.Find("NewProjectPanel").SetActive(false);
		GameObject.Find("LoadProjectPanel").SetActive(false);
    }
    //--------------------------------------------------------------------//
    //                  END PRIVATE FUNCTION DEFINITIONS                  //
    //--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
