using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextureOptions : MenuPanel
{
    public EditModeEvents ev;
    public Dropdown textureDropdown;
    public Transform brushTransform;
    public bool isDragging = false;
    public Slider radius;
    // Use this for initialization
    void Start ()
    {
        TerrainData t = Terrain.activeTerrain.terrainData;
        print(t.splatPrototypes.Length);
        radius.onValueChanged.AddListener(delegate {
            setBrushRadius(radius);
        });
	}

    public void setBrushRadius(Slider r)
    {
        brushTransform.GetComponent<Projector>().orthographicSize = r.value;
    }
    new public void Show()
    {
        base.Show();
    }
    new public void Hide()
    {
        brushTransform.gameObject.SetActive(false);
        base.Hide();
    }
    // Update is called once per frame
    void Update() {
        if (ev.isGUIClicked())
        {
            return;
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        isDragging = false;
        if (Terrain.activeTerrain.GetComponent<TerrainCollider>().Raycast(ray, out hit, 1000.0f))
        {
            brushTransform.gameObject.SetActive(true);
            brushTransform.position = new Vector3(
				hit.point.x , brushTransform.position.y, hit.point.z + 4);
            Vector3 tPos = (brushTransform.position - Terrain.activeTerrain.GetPosition());
            if (Input.GetMouseButton(0))
            {
                isDragging = true;
                applyTexture(tPos);
            }
        }
        else
        {
            brushTransform.gameObject.SetActive(false);
        }
    }
    void applyTexture(Vector3 tPos)
    {
        TerrainData t = Terrain.activeTerrain.terrainData;
        int rad = (int)radius.value;

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
						origTextureMap [iy, ix, textureDropdown.value] + norm > 1.0f ? 
						1.0f :	origTextureMap [ iy, ix, textureDropdown.value] + norm;
					float totalDiff = 0;
					for (int z = 0; z < 3; z++) {
						if (z != textureDropdown.value) {
							totalDiff += origTextureMap [iy, ix, z];
						}
					}
                    for (int z = 0; z < 3; z++) {
						if (z == textureDropdown.value) {
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
}
