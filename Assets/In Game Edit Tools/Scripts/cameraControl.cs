using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {

	public float sensitivity;
	[HideInInspector]
	public Vector3 target;
	Vector2 mos;
	public bool allowGoUnderGround = false;

	void Update () {
		if (Input.mouseScrollDelta.y != 0f && Vector3.Distance (transform.position, target) > 3) { // Zooms to target
			transform.position = Vector3.MoveTowards (transform.position, target, sensitivity * Time.deltaTime * Input.mouseScrollDelta.y);
		}
		if (Vector3.Distance (transform.position, target) < 3) { // Doesnt allow to zoom to close
			transform.position = Vector3.MoveTowards (transform.position, target,  (Vector3.Distance (transform.position, target) - 3.1f));
		}

		if (Input.GetMouseButton (2) && Input.GetKey (KeyCode.LeftAlt)) { // Pans camera
			Vector3 curMPosition = Camera.main.ScreenToViewportPoint (Input.mousePosition - new Vector3 (mos.x, mos.y, 0));
			Vector3 curPosition = new Vector3 (-curMPosition.x * sensitivity * 1.5f, -curMPosition.y * sensitivity * 1.5f, 0);
			Vector3 targetOffset = target - transform.position;
			transform.Translate(curPosition, Space.Self);
			target = targetOffset + transform.position; 
		}

		if (Input.GetMouseButton (2) && !Input.GetKey (KeyCode.LeftAlt)) { // Rotates camera based on mouse input
			if (Input.mousePosition.x > mos.x) {
				transform.RotateAround (target, new Vector3 (0f, 1f, 0f), sensitivity * 2 * (Input.mousePosition.x - mos.x) * Time.deltaTime);
			}
			if (Input.mousePosition.x < mos.x) {
				transform.RotateAround (target, new Vector3 (0f, 1f, 0f), -sensitivity * 2 * (mos.x - Input.mousePosition.x) * Time.deltaTime);
			}
			if (Input.mousePosition.y > mos.y && transform.position.y > 0.5f) {
				transform.position -= Vector3.up * sensitivity/6 * (Input.mousePosition.y - mos.y) * Time.deltaTime;
			}
			if (Input.mousePosition.y < mos.y) {
				transform.position += Vector3.up * sensitivity/6 * (mos.y - Input.mousePosition.y) * Time.deltaTime;
			}
		}

		if (allowGoUnderGround == false) { // Doesn't allow to go underground
			if (transform.position.y < 0.5f) {
				Vector3 temp = transform.position;
				temp.y = 0.5f;
				transform.position = temp;
			}
		}
		mos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y); // Saves mouse last pos for rotation
		transform.LookAt (target);
	}

}
