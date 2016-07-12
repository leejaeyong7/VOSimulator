using UnityEngine;
using System.Collections;

public class SceneModeCameraMovement : MonoBehaviour {
    public float zoomSpeed = 20.0f;
    public float rotateXSpeed = 0.1f;
    public float rotateYSpeed = 0.1f;
    public float transXSpeed = 1.0f;
    public float transYSpeed = 1.0f;
    private Vector3 target;
    private Vector3 prevMousePos;
    private Vector3 currMousePos;

    // Use this for initialization
    void Start()
    {
        Vector3 position = transform.position;
        Vector3 lookAt = transform.forward;
        float v = -(position.y / lookAt.y);
        target = position + v * (lookAt);
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            prevMousePos = Input.mousePosition;
            return;
        }
        currMousePos = Input.mousePosition;
        Vector3 diff = currMousePos - prevMousePos;
        prevMousePos = currMousePos;
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(target, transform.right, rotateXSpeed * diff.y);
            transform.RotateAround(target, Vector3.up, -rotateYSpeed * diff.x);
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 translation = new Vector3(-transXSpeed*diff.x, -transYSpeed*diff.y , 0);
            Vector3 origPos = transform.position;
            transform.Translate(translation);
            target += transform.position - origPos;
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Vector3 translation = new Vector3(0,0,zoomSpeed);
            transform.Translate(translation);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Vector3 translation = new Vector3(0, 0, -zoomSpeed);
            transform.Translate(translation);
        }
    }
}
