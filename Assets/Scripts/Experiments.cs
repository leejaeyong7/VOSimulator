using UnityEngine;
using System.Collections;
using System.Linq;
using com.ootii.Messages;

public class Experiments : MonoBehaviour {
    public bool ison = false;
    float std_start = 0.0f;
    float std_end = 0.0f;
    int std_num = 0;
    int std_curr = 0;
    float flip_start = 0.0f;
    float flip_end = 0.0f;
    int flip_num = 0;
    int flip_curr = 0;
    // Use this for initialization
    void Start () {
        MessageDispatcher.AddListener("EXECUTE_FINISHED", ExecuteRangesHelper);

	}
    public void ExecuteRanges(
        float _std_start, 
        float _std_end, 
        int _std_num, 
        float _flip_start, 
        float _flip_end, 
        int _flip_num)
    {
        std_start = _std_start;
        std_end = _std_end;
        std_num = _std_num;
        std_curr = 0;
        flip_start = _flip_start;
        flip_end = _flip_end;
        flip_num = _flip_num;
        flip_curr = 0;

        MessageDispatcher.SendMessageData("SET_2D_NOISE_SIG_U",0.0f);
        MessageDispatcher.SendMessageData("SET_2D_NOISE_SIG_V",0.0f);
        MessageDispatcher.SendMessageData("SET_FLIPRATE",0.0f);
        MessageDispatcher.SendMessage("EXECUTE_TRAJECTORY");
    }
    void ExecuteRangesHelper(IMessage rMessage)
    {
		if (!ison) {
			return;
		}
		Debug.Log ("Executing Ranges at: ");
		Debug.Log ("std: " + std_curr.ToString ());
		Debug.Log ("flip: " + flip_curr.ToString ());
        if(std_curr > std_num)
        {
            std_curr = 0;
            flip_curr++;
        }
        if(flip_curr > flip_num)
        {
            return;
        }
		if (std_num > 0) {
			float std_val = ((std_end - std_start) / std_num) * std_curr;
	        MessageDispatcher.SendMessageData("SET_2D_NOISE_SIG_U", std_val);
	        MessageDispatcher.SendMessageData("SET_2D_NOISE_SIG_V", std_val);
		}
		if (flip_num > 0) {
			float flip_val = ((flip_end - flip_start) / flip_num)*flip_curr;
			MessageDispatcher.SendMessageData("SET_FLIPRATE", flip_val);
		}
        MessageDispatcher.SendMessage("EXECUTE_TRAJECTORY");
        std_curr++;
    }
}
