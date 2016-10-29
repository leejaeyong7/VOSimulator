using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderPanel : MonoBehaviour {
	public Slider slider;
	public InputField value;
	public float min;
	public float max;
	public bool isPositive; 

	// Use this for initialization
	void Start () {
		slider.onValueChanged.AddListener (onSliderValue);
		slider.minValue = min;
		slider.maxValue = max;
		value.contentType = InputField.ContentType.Standard;
		value.onValueChanged.AddListener (onInputValue);
		value.text = slider.value.ToString ();
	}

	void onSliderValue(float f){
		value.text = f.ToString ("0.00");
	}
	void onInputValue(string s){
		float v;
		if (float.TryParse (s, out v)) {
			if (isPositive) {
				if (v < 0) {
					v = -v;		
				}
				if (v > slider.maxValue) {
					v = slider.maxValue;	
				}
				if (v < slider.minValue) {
					v = slider.minValue;
				}
			}
			value.text = v.ToString ("0.00");
			slider.value = v;
		}
	}
}
