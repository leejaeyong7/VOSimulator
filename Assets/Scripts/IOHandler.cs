using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
using System.Linq;

enum IO_States : int {
	File = 1,
	Trajectory,
	TerrainTexture,
	TerrainRelief,
	Object,
	Environment,
	Capturing,
	SLAM,
	Loading,
	ERROR
};

public class IOHandler : MonoBehaviour {
	int savedState;
	int currentState;
	// Use this for initialization
	void Start () {
		MessageDispatcher.AddListener ("SET_MODE",setMode);		
		MessageDispatcher.AddListener ("SET_STATE",setState);		
		MessageDispatcher.AddListener ("SAVE_STATE",saveState);		
		MessageDispatcher.AddListener ("LOAD_STATE",loadState);		
	}

	public void setMode(IMessage rMessage){
		string mode = (string)rMessage.Data;
		switch (mode) {
		case "Simulation":
			{
				Terrain.activeTerrain.drawHeightmap = true;
				GameObject pc = GameObject.Find ("Pointclouds");
				GameObject pcm = GameObject.Find ("PointcloudMesh");
				if (pc) {
					pc.SetActive (false);
				}
				if (pcm) {
					pcm.SetActive (false);
				}
			}
			break;
		case "SLAM":
			{
				Terrain.activeTerrain.drawHeightmap = false;
				GameObject pc = GameObject.Find ("Pointclouds");
				GameObject pcm = GameObject.Find ("PointcloudMesh");
				if (pc) {
					pc.SetActive (true);
				}
				if (pcm) {
					pcm.SetActive (true);
				}
			}
			break;
		default:
			break;
		}
	}



	public void setState(IMessage rMessage){
		string state = (string)rMessage.Data;
		switch (state) {
		case "File":
			currentState = (int)IO_States.File;
			break;
		case "Trajectory":
			currentState = (int)IO_States.Trajectory;
			break;
		case "TerrainTexture":
			currentState = (int)IO_States.TerrainTexture;
			break;
		case "TerrainRelief":
			currentState = (int)IO_States.TerrainRelief;
			break;
		case "Object":
			currentState = (int)IO_States.Object;
			break;
		case "Environment":
			currentState = (int)IO_States.Environment;
			break;
		case "Capture":
			currentState = (int)IO_States.Capturing;
			break;
		case "Loading":
			currentState = (int)IO_States.Loading;
			break;
		case "ERROR":
		default:
			currentState = (int)IO_States.ERROR;
			break;
		}
	}
	public void saveState(IMessage rMessage){
		savedState = currentState;
	}
	public void loadState(IMessage rMessage){
		currentState = savedState;
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case (int)IO_States.Trajectory:
			trajectoryIOEvent ();
			break;
		case (int)IO_States.TerrainRelief:
		case (int)IO_States.TerrainTexture:
			terrainIOEvent ();
			break;
		default:
			break;
		}
		commonIOEvent ();
	}

	void trajectoryIOEvent(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			MessageDispatcher.SendMessage ("ADD_TRAJECTORY_POINT");			
		}
	}

	void terrainIOEvent(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Terrain.activeTerrain.GetComponent<TerrainCollider> ().Raycast (ray, out hit, 1000.0f)) {
			MessageDispatcher.SendMessageData ("SHOW_TERRAIN_BRUSH",true);			

			MessageDispatcher.SendMessageData("SET_TERRAIN_BRUSH_POSITION",hit.point);
			if (Input.GetMouseButton (0) && !isGUIClicked()) {
				if (currentState == (int)IO_States.TerrainRelief) {
					MessageDispatcher.SendMessage ("APPLY_TERRAIN_RELIEF");
				} else {
					MessageDispatcher.SendMessage ("APPLY_TERRAIN_TEXTURE");
				}
			}
		} else {
			MessageDispatcher.SendMessageData ("SHOW_TERRAIN_BRUSH",false);			
		}
	}

	void commonIOEvent (){
		
	}

	// if GUI is clicked, returns true
	public bool isGUIClicked(){
		return EventSystem.current.IsPointerOverGameObject ();
	}
}
