using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;

public class ObjectSelectorItem : MonoBehaviour {
	public Button itemClickButton;
	// Use this for initialization
	void Start () {
        GameObject selector = gameObject.transform.parent.parent.parent.parent.gameObject;
        ObjectSelector os = selector.GetComponent<ObjectSelector>();
        itemClickButton.onClick.AddListener(delegate ()
        {
            Debug.Log(selector.name);
            MessageDispatcher.SendMessageData("OBJECT_SELECT_ITEM_CLICKED",
                os.objectModels[gameObject.transform.GetSiblingIndex()].gameObject);
        });
	}
    
    // Update is called once per frame


}
