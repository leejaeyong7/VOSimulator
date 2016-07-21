/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : GlobalMenuEvents.cs
 * @brief      : Event handler for Global Panel
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
public class TerrainMenuEvents : MonoBehaviour {
    public Button NoTextureButton;
    public Button GrassTextureButton;
    public Button AsphaltTextureButton;
    public InputField posX;
    public InputField posY;
    public InputField posZ;
    public InputField rotX;
    public InputField rotY;
    public InputField rotZ;
    public InputField scaleX;
    public InputField scaleY;
    public InputField scaleZ;
    public Terrain terrain;
    public Texture2D[] textures;
    private Texture2D emptyTexture;
    private TerrainData terrainData;
    private SplatPrototype[] splats;
    private int currentTextureId;
    //--------------------------------------------------------------------//
    //                    PUBLIC FUNCTION DEFINITIONS                     //
    //--------------------------------------------------------------------//
    void Start () {
        splats = new SplatPrototype[textures.Length+1];

        emptyTexture = new Texture2D(50, 50);
        splats[0] = new SplatPrototype();
        splats[0].texture = emptyTexture;
        splats[0].tileSize = new Vector2(50, 50);
        for (int i = 1; i < splats.Length; i++)
        {
            splats[i] = new SplatPrototype();
            splats[i].texture = textures[i-1];
            splats[i].tileSize = new Vector2(50, 50);
        }
        terrainData = terrain.terrainData;
        terrainData.splatPrototypes = splats;
        
        currentTextureId = 0;
    }
    public void loadTerrain(string type) {
        switch (type)
        {
            case "Grass":
                if(currentTextureId != 1)
                {
                    UpdateTerrainTexture(1);
                    currentTextureId = 1;
                }
                break;
            case "Asphalt":
                if (currentTextureId != 2)
                {
                    UpdateTerrainTexture(2);
                    currentTextureId = 2;
                }
                break;
            default:
                if (currentTextureId != 0)
                {
                    UpdateTerrainTexture(0);
                    currentTextureId = 0;
                }
                break;
        }
    }
    //--------------------------------------------------------------------//
    //                  END PUBLIC FUNCTION DEFINITIONS                   //
    //--------------------------------------------------------------------//
    //--------------------------------------------------------------------//
    //                    PRIVATE FUNCTION DEFINITIONS                    //
    //--------------------------------------------------------------------//
    private void UpdateTerrainTexture(int textureId)
    {
        //get current paint mask
        float[,,] alphas = 
            terrainData.GetAlphamaps(
                0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
        // make sure every grid on the terrain is modified
        for (int i = 0; i < terrainData.alphamapWidth; i++)
        {
            for (int j = 0; j < terrainData.alphamapHeight; j++)
            {
                alphas[i, j, textureId] = 1.0f;
                alphas[i, j, currentTextureId] = 0f;
            }
        }
        // apply the new alpha
        terrainData.SetAlphamaps(0, 0, alphas);
    }
    //--------------------------------------------------------------------//
    //                  END PRIVATE FUNCTION DEFINITIONS                  //
    //--------------------------------------------------------------------//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
