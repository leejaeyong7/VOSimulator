using UnityEngine;
using System.Collections;

public class MenuPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void Show(){
		this.gameObject.SetActive (true);
	}
	public void Hide(){
		this.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
