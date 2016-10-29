using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
using System.Linq;

public class TerrainHandler : MonoBehaviour {

    public Transform brushTransform;
	int textureId;
	float reliefHeight;
	float reliefSTD;

	// Use this for initialization
	void Start () {
		textureId = 0;

		MessageDispatcher.AddListener ("SHOW_TERRAIN_BRUSH", showBrush);
		MessageDispatcher.AddListener ("SET_TERRAIN_BRUSH_POSITION", setBrushPosition);
		MessageDispatcher.AddListener ("SET_TERRAIN_BRUSH_RADIUS", setBrushRadius);

		MessageDispatcher.AddListener ("SET_TERRAIN_RELIEF_HEIGHT", setReliefHeight);
		MessageDispatcher.AddListener ("SET_TERRAIN_RELIEF_STD", setReliefSTD);
		MessageDispatcher.AddListener ("APPLY_TERRAIN_RELIEF", applyRelief);

		MessageDispatcher.AddListener ("SET_TERRAIN_TEXTURE_ID", setTextureId);
		MessageDispatcher.AddListener ("APPLY_TERRAIN_TEXTURE", applyTexture);
	}

	public void setBrushPosition(IMessage rMessage){
		Vector3 hit = (Vector3)rMessage.Data;
		brushTransform.position = new Vector3 (
			hit.x, brushTransform.position.y, hit.z + 4);
		Vector3 tPos = (brushTransform.position - Terrain.activeTerrain.GetPosition ());
		brushTransform.transform.position = tPos;
	}


	public void setReliefHeight(IMessage rMessage){
		reliefHeight = (float)rMessage.Data;
	}

	public void setReliefSTD(IMessage rMessage){
		reliefSTD = (float)rMessage.Data;
	}

	public void setTextureId(IMessage rMessage){
		textureId = (int)rMessage.Data;
	}

	public void setBrushRadius(IMessage rMessage){
		float radius = (float)rMessage.Data;
		brushTransform.GetComponent<Projector> ().orthographicSize = radius;
	}

	public void showBrush(IMessage rMessage){
		bool show = (bool)rMessage.Data;
		brushTransform.gameObject.SetActive (show);
	}


    public void applyTexture(IMessage rMessage)
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

	public void applyRelief(IMessage rMessage) {
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

}
