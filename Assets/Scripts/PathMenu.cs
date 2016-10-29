using UnityEngine;
using System.Collections;
using com.ootii.Messages;

public class PathMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable(){
		MessageDispatcher.SendMessageData ("SET_STATE", "Trajectory");
	}

	void OnDisable(){
		
	}
}
