using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextureOptions : MenuPanel{
    public Transform brushTransform;
	// Use this for initialization
	void Start () {
        brushTransform.gameObject.SetActive(false);
	}
    new public void Show()
    {
        base.Show();
        brushTransform.gameObject.SetActive(true);
    }
    new public void Hide()
    {
        base.Hide();
        brushTransform.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            brushTransform.position = new Vector3(hit.point.x,
                brushTransform.position.y,hit.point.z);
        }
	}
}
