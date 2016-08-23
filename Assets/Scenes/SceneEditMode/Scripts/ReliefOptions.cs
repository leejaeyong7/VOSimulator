using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReliefOptions : MenuPanel {

    public Transform brushTransform;
    public Slider radius;
    // Use this for initialization
    void Start () {
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
        brushTransform.gameObject.SetActive(true);
    }
    new public void Hide()
    {
        base.Hide();
        brushTransform.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            brushTransform.position = new Vector3(hit.point.x,
                brushTransform.position.y, hit.point.z);
        }
    }
}
