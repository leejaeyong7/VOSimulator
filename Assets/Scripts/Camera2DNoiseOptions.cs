using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.ootii.Messages;

public class Camera2DNoiseOptions : MonoBehaviour {
	public Slider mu_u;
	public Slider mu_v;
	public Slider sig_u;
	public Slider sig_v;
	// Use this for initialization
	void Start () {

		mu_u.onValueChanged.AddListener (delegate(float f) {
			MessageDispatcher.SendMessageData("SET_2D_NOISE_MU_U",f);	
		});
		mu_v.onValueChanged.AddListener (delegate(float f) {
			MessageDispatcher.SendMessageData("SET_2D_NOISE_MU_V",f);	
		});
		sig_u.onValueChanged.AddListener (delegate(float f) {
			MessageDispatcher.SendMessageData("SET_2D_NOISE_SIG_U",f);	
		});
		sig_v.onValueChanged.AddListener (delegate(float f) {
			MessageDispatcher.SendMessageData("SET_2D_NOISE_SIG_V",f);	
		});
	}
}
