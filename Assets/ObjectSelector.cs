using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;

public class ObjectSelector : MonoBehaviour {
	public EnvironmentObject[] objectModels;
	public GameObject contents;
	public GameObject objectSelectorItemPrefab;

	// Use this for initialization
	void Start () {
		int count = 0;
		foreach(EnvironmentObject eo in objectModels){
			GameObject no = Instantiate (objectSelectorItemPrefab);
			RawImage ri = no.GetComponent<RawImage> ();
			Button btn = no.GetComponent<Button> ();
            
			ri.texture = eo.thumbnail;
			btn.targetGraphic = ri;
			no.name = gameObject.name + "_" + count.ToString ();
			no.transform.SetParent (contents.transform);
			RectTransform rt = (RectTransform)no.transform;
			rt.localScale = new Vector3 (1, 1, 1);
			count++;
		}
	}
	void OnEnable(){
		MessageDispatcher.SendMessageData ("SET_STATE", "Object");
	}
}
