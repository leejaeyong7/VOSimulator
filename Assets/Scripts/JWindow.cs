using UnityEngine;
using System.Collections;
using com.ootii.Messages;

public class JWindow : JGUI {
	JWindowHead header = null;
	// Use this for initialization
	void Start () {
		GameObject headerobj= new GameObject();	
		headerobj.transform.SetParent (transform);
		header = headerobj.AddComponent<JWindowHead> ();
	}
}
