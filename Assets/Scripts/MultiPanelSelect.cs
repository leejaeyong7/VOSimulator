using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MultiPanelSelect : MonoBehaviour {
	public Dropdown multiPanelSelect;
	public GameObject multiPanelContent;

	void Start(){
		setDropdown ();
		multiPanelSelect.onValueChanged.AddListener (selectPanel);
		if (multiPanelContent.transform.childCount > 0) {
			selectPanel (0);
		}
	}

	public void AddPanel(GameObject obj){
		setDropdown ();
		obj.transform.SetParent (multiPanelContent.transform);	
	}

	void setDropdown(){
		List<string> childnames = new List<string> ();	
		foreach (Transform child in multiPanelContent.transform) {
			childnames.Add (child.gameObject.name);
		}
		multiPanelSelect.AddOptions (childnames);
	}

	void selectPanel(int id){
		int count = 0;
		multiPanelSelect.value = id;
		foreach (Transform child in multiPanelContent.transform) {
			if (count == id) {
				child.gameObject.SetActive (true);
				MultiPanelSelect ms = child.GetComponentInChildren<MultiPanelSelect> ();
				if (ms) {
					ms.selectPanel (0);
				}
			} else {
				child.gameObject.SetActive (false);
				MultiPanelSelect ms = child.GetComponentInChildren<MultiPanelSelect> ();
				if (ms) {
					ms.hideAll();
				}
			}
			count++;
		}
	}
	void hideAll(){
		foreach (Transform child in multiPanelContent.transform) {
			child.gameObject.SetActive (false);
			MultiPanelSelect ms = child.GetComponentInChildren<MultiPanelSelect> ();
			if (ms) {
				ms.hideAll();
			}
		}

	}
}
