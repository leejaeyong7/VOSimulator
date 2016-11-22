using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
using System.Linq;

enum IO_States : int {
	FILE = 1,
    ENVIRONMENT_TERRAIN_TEXTURE,
    ENVIRONMENT_TERRAIN_RELIEF,
    ENVIRONMENT_OBJECT,
    TRAJECTORY_CREATE,
    TRAJECTORY_VIEW,
    EXECUTE,
    CAPTURING,
    LOADING,
    ERROR
};

public class IOHandler : MonoBehaviour {
	public TerrainHandler terrainHandler;
	IO_States savedState;
    IO_States currentState;
	// Use this for initialization
	void Start () {
        Screen.SetResolution(752, 480, true);
        MessageDispatcher.AddListener ("SET_STATE",setState);		
		MessageDispatcher.AddListener ("SAVE_STATE",saveState);		
		MessageDispatcher.AddListener ("LOAD_STATE",loadState);		
	}

	public void setState(IMessage rMessage){
		string state = (string)rMessage.Data;
		switch (state) {
		case "File":
			currentState = IO_States.FILE;
			break;
		case "TerrainTexture":
			currentState = IO_States.ENVIRONMENT_TERRAIN_TEXTURE;
			break;
		case "TerrainRelief":
			currentState = IO_States.ENVIRONMENT_TERRAIN_RELIEF;
			break;
		case "Object":
			currentState = IO_States.ENVIRONMENT_OBJECT;
			break;
		case "TrajectoryCreate":
			currentState = IO_States.TRAJECTORY_CREATE;
            break;
		case "TrajectoryView":
			currentState = IO_States.TRAJECTORY_VIEW;
            break;
		case "Capture":
			currentState = IO_States.CAPTURING;
			break;
		case "Loading":
			currentState = IO_States.LOADING;
			break;
		case "ERROR":
		default:
			currentState = IO_States.ERROR;
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
		case IO_States.TRAJECTORY_CREATE:
			trajectoryIOEvent ();
			break;
		case IO_States.ENVIRONMENT_TERRAIN_RELIEF:
		case IO_States.ENVIRONMENT_TERRAIN_TEXTURE:
			terrainIOEvent ();
			break;
		case IO_States.ENVIRONMENT_OBJECT:
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
		if (Input.GetMouseButtonDown (0)) {
			if (!isGUIClicked ()) {
			}
		}
	}

	void terrainIOEvent(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		TerrainCollider tc = Terrain.activeTerrain.GetComponent<TerrainCollider>();
		if (tc.Raycast (ray, out hit, 1000.0f) && terrainHandler.isUsingBrush) {
			MessageDispatcher.SendMessageData ("SHOW_TERRAIN_BRUSH",true);			
			MessageDispatcher.SendMessageData("SET_TERRAIN_BRUSH_POSITION",hit.point);
			if (Input.GetMouseButton (0) && !isGUIClicked()) {
				if (currentState == IO_States.ENVIRONMENT_TERRAIN_RELIEF) {
					MessageDispatcher.SendMessage ("APPLY_TERRAIN_RELIEF");
				} else {
					MessageDispatcher.SendMessage ("APPLY_TERRAIN_TEXTURE");
				}
			}
		} else {
			MessageDispatcher.SendMessageData ("SHOW_TERRAIN_BRUSH",false);			
		}
	}
	void objectIOEvent(){
		
	}

	void commonIOEvent (){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		TerrainCollider tc = Terrain.activeTerrain.GetComponent<TerrainCollider>();
		if (tc.Raycast (ray, out hit, 1000.0f)) {
		}
		
	}

	// if GUI is clicked, returns true
	public bool isGUIClicked(){
		return EventSystem.current.IsPointerOverGameObject ();
	}
}
