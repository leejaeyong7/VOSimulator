using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ReliefOptions : MenuPanel {
	public EditModeEvents ev;
    public Transform brushTransform;
	public bool isDragging = false;
    public Slider radius;
    public Slider height;
	public Slider std;
    // Use this for initialization
    void Start () {
		TerrainData t = Terrain.activeTerrain.terrainData;
        radius.onValueChanged.AddListener(delegate {
            setBrushRadius(radius);
        });
    }

    public void setBrushRadius(Slider r)
    {
        brushTransform.GetComponent<Projector>().orthographicSize = r.value*2;
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
    void Update () {
		if (ev.isGUIClicked ()) {
			return;
		}
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		isDragging = false;
		if (Terrain.activeTerrain.GetComponent<TerrainCollider> ().Raycast (ray, out hit,1000.0f)) {
			brushTransform.gameObject.SetActive (true);
			brushTransform.position = new Vector3 (hit.point.x,
				brushTransform.position.y, hit.point.z);
			Vector3 tPos = (brushTransform.position - Terrain.activeTerrain.GetPosition ());
			if(Input.GetMouseButton(0)){
				isDragging = true;
				applyHeight(tPos);
			}
		} else {
			brushTransform.gameObject.SetActive (false);
		}
    }
	void applyHeight(Vector3 tPos){
		int rad = (int)radius.value;
		float h = height.value;
		float stdv = std.value;

		TerrainData t = Terrain.activeTerrain.terrainData;
		Vector3 pos;
		pos.x  = (tPos.x / t.size.x);
		pos.y  = (tPos.y / t.size.y);
		pos.z  = (tPos.z / t.size.z);
		Vector3 tsize = t.size;

		int tx = (int)(pos.x * t.heightmapWidth);
		int ty = (int)(pos.z * t.heightmapHeight);
		float sz = t.heightmapScale.y;

		float[,] oldHeight = t.GetHeights (tx-rad, ty-rad, rad * 2, rad * 2);
		float [,] heightChange = new float[rad*2,rad*2];

		for (int x = 0; x < rad * 2; x++) {
			for (int y = 0; y < rad * 2; y++) {
				heightChange[y,x] = h * Mathf.Exp (
					-1 * ((Mathf.Pow(rad - x,2) + Mathf.Pow(rad - y,2)))/(2*Mathf.Pow(stdv,2))
				)/sz;
			}
		}

		for (int x = 0; x < rad * 2; x++) {
			for (int y = 0; y < rad * 2; y++) {
				oldHeight [y, x] += heightChange [y, x];
			}
		}

		Terrain.activeTerrain.terrainData.SetHeights(
			tx - rad, ty - rad, oldHeight);
	}
}
