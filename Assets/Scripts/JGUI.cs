using UnityEngine;
using System.Collections;
using com.ootii.Messages;

public class JGUI : MonoBehaviour{
	// Use this for initialization
	void Start () {
		init ();
	}
	public void init(){
		gameObject.AddComponent<CanvasRenderer> ();
		gameObject.AddComponent<RectTransform> ();
	}

	public void Show(){
		gameObject.SetActive (true);	
	}

	public void Hide(){
		gameObject.SetActive (false);	
	}
}
