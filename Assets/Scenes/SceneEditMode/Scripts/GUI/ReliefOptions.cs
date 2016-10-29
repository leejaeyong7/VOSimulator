using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.ootii.Messages;

public class ReliefOptions : MenuPanel
{
	public EditModeEvents ev;
	public Transform brushTransform;
	public bool isDragging = false;
	public Slider radius;
	public Slider height;
	public Slider std;
	// Use this for initialization
	void Start ()
	{
		radius.onValueChanged.AddListener (delegate {
			MessageDispatcher.SendMessageData("SET_TERRAIN_BRUSH_RADIUS",radius.value);
		});
		height.onValueChanged.AddListener (delegate {
			MessageDispatcher.SendMessageData("SET_TERRAIN_RELIEF_HEIGHT",height.value);
		});
		std.onValueChanged.AddListener (delegate {
			MessageDispatcher.SendMessageData("SET_TERRAIN_RELIEF_STD",std.value);
		});
	}

	void OnEnable()
	{
		MessageDispatcher.SendMessageData ("SET_STATE", "TerrainRelief");
		MessageDispatcher.SendMessageData("SET_TERRAIN_BRUSH_RADIUS",radius.value);
		MessageDispatcher.SendMessageData("SET_TERRAIN_RELIEF_HEIGHT",height.value);
		MessageDispatcher.SendMessageData("SET_TERRAIN_RELIEF_STD",std.value);
		MessageDispatcher.SendMessageData("SHOW_TERRAIN_BRUSH",true);

	}

	void OnDisable()
	{
		MessageDispatcher.SendMessageData("SHOW_TERRAIN_BRUSH",false);
	}
}
