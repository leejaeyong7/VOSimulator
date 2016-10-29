using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.ootii.Messages;

public class RunPanel : MenuPanel {
	public Button execute;

	// Use this for initialization
	void Start () {
		execute.onClick.AddListener (delegate {
			MessageDispatcher.SendMessage ("EXECUTE_TRAJECTORY");	
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
