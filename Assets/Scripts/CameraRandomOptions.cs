using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.ootii.Messages;

public class CameraRandomOptions : MonoBehaviour {
	public InputField MaxPoints;
	public Slider DropRate;
	public Slider FlipRate;

	// Use this for initialization
	void Start () {
		DropRate.onValueChanged.AddListener (delegate(float f) {
			MessageDispatcher.SendMessageData("SET_DROPRATE",f);
		});
		FlipRate.onValueChanged.AddListener (delegate(float f) {
			MessageDispatcher.SendMessageData("SET_FLIPRATE",f);	
		});
		MaxPoints.onValueChanged.AddListener (delegate(string arg) {
			int res;
			if(int.TryParse(arg,out res)){
				if(res < 0){
					res = res * -1;
				}
				MaxPoints.text = res.ToString();
				MessageDispatcher.SendMessageData("SET_MAX_FEATURE_POINT",res);
			}
		});
	}
}
