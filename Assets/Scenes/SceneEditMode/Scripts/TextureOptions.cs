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
        brushTransform.GetComponent<Projector>().orthographicSize = r.value * 2;
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
            brushTransform.position = new Vector3(hit.point.x,
                brushTransform.position.y, hit.point.z);
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
        float[,,] alphas =
          t.GetAlphamaps(
              0, 0, t.alphamapWidth, t.alphamapHeight);

        int rad = (int)radius.value;



        Vector3 pos;
        pos.x = (tPos.x / t.size.x);
        pos.y = (tPos.y / t.size.y);
        pos.z = (tPos.z / t.size.z);
        Vector3 tsize = t.size;

        int tx = (int)(pos.x * t.heightmapWidth);
        int ty = (int)(pos.z * t.heightmapHeight);
        float sz = t.heightmapScale.y;

        float[,,] origTextureMap = t.GetAlphamaps(tx - rad, ty - rad, rad*2,rad*2);
        float[,,] textureMap = new float[rad * 2, rad * 2,3];

        for (int x = 0; x < rad * 2; x++)
        {
            for (int y = 0; y < rad * 2; y++)
            {
                if(Mathf.Abs(Mathf.Pow(x-rad,2) +  Mathf.Pow(y-rad,2)) <= rad*rad)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        textureMap[y, x, z] = 0;
                    }
                    textureMap[y, x, textureDropdown.value] = 1.0f;
                } else
                {
                    for (int z = 0; z < 3; z++)
                    {
                        textureMap[y, x, z] = origTextureMap[y, x, z];
                    }
                }
            }
        }

        Terrain.activeTerrain.terrainData.SetAlphamaps(
            tx - rad, ty - rad, textureMap);
        
    }
}
