using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using com.ootii.Messages;

public class TextureOptions : MenuPanel
{
    public Dropdown textureDropdown;
    public Slider radius;
	public Slider intensity;
    // Use this for initialization
    void Start ()
    {
        radius.onValueChanged.AddListener(delegate {
			MessageDispatcher.SendMessageData("SET_TERRAIN_BRUSH_RADIUS",radius.value);
        });
		textureDropdown.onValueChanged.AddListener (delegate {
			MessageDispatcher.SendMessageData("SET_TERRAIN_TEXTURE_ID",textureDropdown.value);
		});
	}

    void OnEnable()
    {
		MessageDispatcher.SendMessageData ("SET_STATE", "TerrainTexture");
		MessageDispatcher.SendMessageData("SET_TERRAIN_BRUSH_RADIUS",radius.value);
		MessageDispatcher.SendMessageData("SET_TERRAIN_TEXTURE_ID",textureDropdown.value);
		MessageDispatcher.SendMessageData("SHOW_TERRAIN_BRUSH",true);
    }

    void OnDisable()
    {
		MessageDispatcher.SendMessageData("SHOW_TERRAIN_BRUSH",false);
    }
}
