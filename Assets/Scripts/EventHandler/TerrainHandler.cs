/*============================================================================
 * @author     : Jae Yong Lee (leejaeyong7@gmail.com)
 * @file       : test.cs
 * @brief      : Event handler for trajectory create UI
 * Copyright (c) Jae Yong Lee / UIUC Fall 2016
 =============================================================================*/
//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
using System.Linq;
using CielaSpike;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class TerrainHandler : MonoBehaviour {

    //********************************************************************//
    //***************************BEGIN VARIABLES**************************//
    //********************************************************************//
    //====================================================================//
    //                    PUBLIC VARIABLE DEFINITIONS                     //
    //====================================================================//
    public Transform brushTransform;
	public CustomFileBrowser fb;
	public Models models;
    //====================================================================//
    //                  END PUBLIC VARIABLE DEFINITIONS                   //
    //====================================================================//
    //====================================================================//
    //                    PRIVATE VARIABLE DEFINITIONS                    //
    //====================================================================//
	int textureId;
	float reliefHeight;
	float reliefSTD;
	bool terrainShown;

	PLYLoader plyLoader;
    //====================================================================//
    //                  END PRIVATE VARIABLE DEFINITIONS                  //
    //====================================================================//
    //********************************************************************//
    //****************************END VARIABLES***************************//
    //********************************************************************//
    //********************************************************************//
    //****************************BEGIN METHODS***************************//
    //********************************************************************//
    //====================================================================//
    //                 MONOBEHAVIOR FUNCTION DEFINITIONS                  //
    //====================================================================//

	/**
	 * Attach eventlisteners from UIs
	 */
	void Start () {
		// initialize variables
		plyLoader = new PLYLoader();
		textureId = 0;
		terrainShown = true;
		plyLoader.pointsGameObject = GameObject.Find("Features");
		plyLoader.meshGameObject = GameObject.Find("Surfaces");

		// attach listeners
		MessageDispatcher.AddListener (
			"SHOW_TERRAIN_BRUSH",
			showTerrainBrushCallback);

		MessageDispatcher.AddListener (
			"SET_TERRAIN_BRUSH_POSITION",
			setBrushPositionCallback);

		MessageDispatcher.AddListener (
			"SET_TERRAIN_BRUSH_RADIUS",
			setTerrainBrushRadiusCallback);

		MessageDispatcher.AddListener (
			"SET_TERRAIN_RELIEF_HEIGHT", 
			setTerrainReliefHeightCallback);

		MessageDispatcher.AddListener (
			"SET_TERRAIN_RELIEF_STD",
			setTerrainReliefSTDCallback);

		MessageDispatcher.AddListener (
			"APPLY_TERRAIN_RELIEF",
			applyReliefCallback);

		MessageDispatcher.AddListener (
			"SET_TERRAIN_TEXTURE_TYPE",
			setTerrainTextureTypeCallback);

		MessageDispatcher.AddListener (
			"SET_TERRAIN_TEXTURE_STRENGTH",
			setTerrainTextureStrengthCallback);

		MessageDispatcher.AddListener (
			"APPLY_TERRAIN_TEXTURE",
			applyTextureCallback);

		MessageDispatcher.AddListener(
			"TOGGLE_TERRAIN",
			toggleTerrainCallback);

		MessageDispatcher.AddListener(
			"TERRAIN_IMPORT_FEATURE_PRESSED",
			importFeaturesCallback
		);

		MessageDispatcher.AddListener(
			"TERRAIN_IMPORT_SURFACE_PRESSED",
			importSurfaceCallback
		);
	}

	//====================================================================//
	//               END MONOBEHAVIOR FUNCTION DEFINITIONS                //
	//====================================================================//
	//====================================================================//
	//                     PUBLIC METHOD DEFINITIONS                      //
	//====================================================================//
	public void DisplayBrush(bool show)
	{
		brushTransform.gameObject.SetActive(show);
	}

	public void SetBrushPosition(Vector3 position)
	{
		brushTransform.position = position;
	}

	public void SetBrushRadius(float r)
	{
		brushTransform.GetComponent<Projector> ().orthographicSize = r;
	}

	public void DisplayTerrain(bool show)
	{
		terrainShown = show;
		Terrain.activeTerrain.drawHeightmap = show;
	}

	//====================================================================//
	//                   END PUBLIC METHOD DEFINITIONS                    //
	//====================================================================//
	//====================================================================//
	//                     PRIVATE METHOD DEFINITIONS                     //
	//====================================================================//
	void EnabledCallback()
	{
		DisplayBrush(true && terrainShown);
	}

	void DisabledCallback()
	{
		DisplayBrush(false);
	}

	void setBrushPositionCallback(IMessage rMessage){
		Vector3 hit = (Vector3)rMessage.Data;
		brushTransform.position = new Vector3 (
			hit.x, brushTransform.position.y, hit.z + 4);
		Vector3 tPos = 
			(brushTransform.position - Terrain.activeTerrain.GetPosition ());
		SetBrushPosition(tPos);
	}

	void setTerrainReliefHeightCallback(IMessage rMessage){
		reliefHeight = (float)rMessage.Data;
	}

	void setTerrainReliefSTDCallback(IMessage rMessage){
		reliefSTD = (float)rMessage.Data;
	}

	void setTerrainTextureTypeCallback(IMessage rMessage){
		textureId = (int)rMessage.Data;
	}

	void setTerrainTextureStrengthCallback(IMessage rMessage)
	{

	}

	void setTerrainBrushRadiusCallback(IMessage rMessage){
		float radius = (float)rMessage.Data;
		SetBrushRadius(radius);
	}

	void showTerrainBrushCallback(IMessage rMessage){
		bool show = (bool)rMessage.Data;
		DisplayBrush(show && terrainShown);
	}


	void toggleTerrainCallback(IMessage rMessage)
	{
		bool show = (bool)rMessage.Data;
		DisplayTerrain(show);
	}

	void applyReliefCallback(IMessage rMessage)
	{
		if (terrainShown)
		{
			applyRelief();
		}
	}

	void applyTextureCallback(IMessage rMessage)
	{
		if (terrainShown)
		{
			applyTexture();
		}
	}

	void importFeaturesCallback(IMessage rMessage)
	{
		fb.showBrowser("ply", (f) => {
			StartCoroutine(plyImportHelper(f));
		});	
	}

	void importSurfaceCallback(IMessage rMessage)
	{
		fb.showBrowser("ply", (f) => {
			StartCoroutine(plyImportHelper(f));
		});	
	}
    //====================================================================//
    //                   END PRIVATE METHOD DEFINITIONS                   //
    //====================================================================//
    //********************************************************************//
    //*****************************END METHODS****************************//
    //********************************************************************//
    //********************************************************************//
    //******************************BEGIN ETC*****************************//
    //********************************************************************//
    //====================================================================//
    //                    HELPER FUNCTION DEFINITIONS                     //
    //====================================================================//
    void applyTexture()
    {
		if (!brushTransform.gameObject.activeSelf) {
			return;
		}
		Vector3 tPos = brushTransform.transform.position;
        TerrainData t = Terrain.activeTerrain.terrainData;
		int rad = (int)brushTransform.GetComponent<Projector> ().orthographicSize;

        Vector3 pos;
		pos.x = ((tPos.x )/ t.size.x);
        pos.y = (tPos.y / t.size.y);
		pos.z = ((tPos.z - 4)/ t.size.z);
        Vector3 tsize = t.size;

        int tx = (int)(pos.x * t.heightmapWidth);
        int ty = (int)(pos.z * t.heightmapHeight);


		if (tx < 0 || tx > t.heightmapWidth) {
			return;
		}

		if (ty < 0 || ty > t.heightmapHeight) {
			return;
		}

		int sx =  tx - rad;
		int sy =  ty - rad;
		int ex =  tx + rad;
		int ey =  ty + rad;
		sx = sx > 0 ? sx : 0;
		sy = sy > 0 ? sy : 0;
		ex = ex < t.heightmapWidth ? ex : t.heightmapWidth-1;
		ey = ey < t.heightmapHeight ? ey : t.heightmapHeight-1;

        float[,,] origTextureMap = t.GetAlphamaps(sx, sy, ex - sx, ey - sy);
		float[,,] textureMap = new float[ey - sy,ex - sx ,3];
        for (int x = sx; x < ex; x++)
        {
            for (int y = sy; y < ey; y++)
            {
				int ix = x - sx;
				int iy = y - sy;
				float dx = tx - x;
				float dy = ty - y;
                if(Mathf.Abs(Mathf.Pow(dx,2) +  Mathf.Pow(dy,2)) <= rad*rad)
                {
					float dxr = (dx) / ((float)rad);
					float dyr = (dx) / ((float)rad);
					float norm = 1 / (2*Mathf.PI) * Mathf.Exp (
						-0.5f * (Mathf.Pow (dxr, 2) + Mathf.Pow (dyr, 2)));
					float update = 
						origTextureMap [iy, ix, textureId] + norm > 1.0f ? 
						1.0f :	origTextureMap [ iy, ix, textureId] + norm;
					float totalDiff = 0;
					for (int z = 0; z < 3; z++) {
						if (z != textureId) {
							totalDiff += origTextureMap [iy, ix, z];
						}
					}
                    for (int z = 0; z < 3; z++) {
						if (z == textureId) {
							textureMap [iy, ix, z] = update;
						} else {
							float orig = origTextureMap [iy, ix, z];
							textureMap [iy, ix, z] = (1.0f - update) * (orig / totalDiff);
						}
                    }
                } else {
                    for (int z = 0; z < 3; z++)
                    {
                        textureMap[iy, ix, z] = origTextureMap[iy, ix, z];
                    }
                }
            }
        }

        Terrain.activeTerrain.terrainData.SetAlphamaps( sx,sy, textureMap);
        
    }

	void applyRelief() {
		if (!brushTransform.gameObject.activeSelf) {
			return;
		}
		Vector3 tPos = brushTransform.transform.position;
		int rad = (int)brushTransform.GetComponent<Projector> ().orthographicSize;
		float h = reliefHeight;
		float stdv = reliefSTD;

		TerrainData t = Terrain.activeTerrain.terrainData;
		Vector3 pos;

		pos.x = (tPos.x / t.size.x);
		pos.y = (tPos.y / t.size.y);
		pos.z = ((tPos.z - 4) / t.size.z);
		Vector3 tsize = t.size;

		int tx = (int)(pos.x * t.heightmapWidth);
		int ty = (int)(pos.z * t.heightmapHeight);
		float sz = t.heightmapScale.y;


		int sx = tx - rad;
		int sy = ty - rad;
		int ex = tx + rad;
		int ey = ty + rad;
		sx = sx > 0 ? sx : 0;
		sy = sy > 0 ? sy : 0;
		ex = ex < t.heightmapWidth ? ex : t.heightmapWidth - 1;
		ey = ey < t.heightmapHeight ? ey : t.heightmapHeight - 1;

		float[,] oldHeight = t.GetHeights (sx, sy, ex - sx, ey - sy);
		float[,] heightChange = new float[ey - sy, ex - sx];

		for (int x = sx; x < ex; x++) {
			for (int y = sy; y < ey; y++) {
				int ix = x - sx;
				int iy = y - sy;
				float dx = tx - x;
				float dy = ty - y;
				heightChange [iy, ix] = h * Mathf.Exp (
					-1 / (2 * Mathf.PI) * ((Mathf.Pow (dx, 2) + Mathf.Pow (dy, 2))) / (2 * Mathf.Pow (stdv, 2))
				) / sz;
			}
		}

		for (int x = sx; x < ex; x++) {
			for (int y = sy; y < ey; y++) {
				int ix = x - sx;
				int iy = y - sy;
				oldHeight [iy, ix] += heightChange [iy, ix];
			}
		}

		Terrain.activeTerrain.terrainData.SetHeights (
			sx,sy , oldHeight);
	}

	public IEnumerator plyImportHelper(System.IO.FileInfo fp)
	{
		Task task;
		this.StartCoroutineAsync(plyLoader.loadFile(fp), out task);
		if (task.State == TaskState.Error)
		{
			Debug.Log("Error importing PLY");
			yield break;
		}
		yield return StartCoroutine(task.Wait());
		Debug.Log(task.State);
		if (task.State == TaskState.Done)
		{
			// load Pointcloud or Object
			if (plyLoader.isPointcloud)
			{
				this.StartCoroutineAsync(plyLoader.createPointCloud(), out task);
				if (task.State == TaskState.Error)
				{
					Debug.Log("Error importing PLY");
					yield break;
				}
			}
			else {
				this.StartCoroutineAsync(plyLoader.createObject(), out task);
				if (task.State == TaskState.Error)
				{
					Debug.Log("Error importing PLY");
					yield break;
				}
			}
		}
		yield return task.Wait();
		MessageDispatcher.SendMessageData("SET_SCALE",models.scale);
		yield break;
	}
    //====================================================================//
    //                  END HELPER FUNCTION DEFINITIONS                   //
    //====================================================================//
    //********************************************************************//
    //*******************************END ETC******************************//
    //********************************************************************//
}
//----------------------------------------------------------------------------//
//                           END CLASS DEFINITIONS                            //
//----------------------------------------------------------------------------//
