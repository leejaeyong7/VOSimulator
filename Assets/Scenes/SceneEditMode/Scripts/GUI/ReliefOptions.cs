using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReliefOptions : MenuPanel
{
	public EditModeEvents ev;
	public Transform brushTransform;
	public bool isDragging = false;
	public Slider radius;
	public Slider height;
	public Slider std;
	// Use this for initialization
	void Start ()
	{
		TerrainData t = Terrain.activeTerrain.terrainData;
		radius.onValueChanged.AddListener (delegate {
			setBrushRadius (radius);
		});
	}

	public void setBrushRadius (Slider r)
	{
		brushTransform.GetComponent<Projector> ().orthographicSize = r.value;
	}

	new public void Show ()
	{
		base.Show ();
	}

	new public void Hide ()
	{
		brushTransform.gameObject.SetActive (false);
		base.Hide ();
	}
	// Update is called once per frame
	void Update ()
	{
		if (ev.isGUIClicked ()) {
			return;
		}
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		isDragging = false;
		if (Terrain.activeTerrain.GetComponent<TerrainCollider> ().Raycast (ray, out hit, 1000.0f)) {
			brushTransform.gameObject.SetActive (true);
			brushTransform.position = new Vector3 (
				hit.point.x, brushTransform.position.y, hit.point.z + 4);
			Vector3 tPos = (brushTransform.position - Terrain.activeTerrain.GetPosition ());
			if (Input.GetMouseButton (0)) {
				isDragging = true;
				applyHeight (tPos);
			}
		} else {
			brushTransform.gameObject.SetActive (false);
		}
	}

	void applyHeight (Vector3 tPos) {
		int rad = (int)radius.value;
		float h = height.value;
		float stdv = std.value;

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
