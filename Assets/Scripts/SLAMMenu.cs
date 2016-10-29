//----------------------------------------------------------------------------//
//                               CLASS IMPORTS                                //
//----------------------------------------------------------------------------//
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
using System.Linq;
//----------------------------------------------------------------------------//
//                             END CLASS IMPORTS                              //
//----------------------------------------------------------------------------//
//----------------------------------------------------------------------------//
//                             CLASS DEFINITIONS                              //
//----------------------------------------------------------------------------//
public class SLAMMenu : MonoBehaviour {
	// file browser for importing files
	public CustomFileBrowser fb;

	public Button import_true;
	public Button import_slam;
	public Button import_transformation;
	public Button import_features;
	public Button align;
	public Button import_executable_path;
	public Button execute;

	public Slider Scale;

	void Start(){
		import_true.onClick.AddListener (delegate {
			fb.showBrowser("pts",dispatchImportTruePath);
		});

		import_slam.onClick.AddListener (delegate {
			fb.showBrowser("out",dispatchImportSlamPath);
		});

		import_transformation.onClick.AddListener (delegate {
			fb.showBrowser("trns",dispatchImportTransform);
		});

		import_features.onClick.AddListener ( delegate() {
			fb.showBrowser("ply",dispatchImportFeatures);
		});

		import_executable_path.onClick.AddListener ( delegate() {
			fb.showBrowser("pts",dispatchImportExePath);
		});

		align.onClick.AddListener (dispatchAlign);


		execute.onClick.AddListener ( delegate() {
			MessageDispatcher.SendMessage("EXECUTE_TRAJECTORY");
		});

		Scale.onValueChanged.AddListener (delegate(float arg) {
			MessageDispatcher.SendMessageData("SET_SLAM_TRAJECTORY_SCALE",arg);
		});
	}

	void OnEnable(){
		MessageDispatcher.SendMessageData ("SET_MODE", "SLAM");
	}

	void OnDisable(){
		
	}


	// event dispatcher for importing true path
	void dispatchImportTruePath(System.IO.FileInfo fp){
		MessageDispatcher.SendMessageData("SLAM_IMPORT_TRUE_PATH",fp);
	}

	void dispatchImportSlamPath(System.IO.FileInfo fp){
		MessageDispatcher.SendMessageData("SLAM_IMPORT_SLAM_PATH",fp);
	}

	void dispatchImportTransform(System.IO.FileInfo fp){
		MessageDispatcher.SendMessageData ("SLAM_IMPORT_TRANSFORMATION", fp);
	}

	void dispatchImportFeatures(System.IO.FileInfo fp){
		MessageDispatcher.SendMessageData ("SLAM_IMPORT_FEATURES", fp);
	}

	void dispatchImportExePath(System.IO.FileInfo fp){
		MessageDispatcher.SendMessageData ("SLAM_IMPORT_EXE_PATH", fp);
	}

	void dispatchAlign(){
		MessageDispatcher.SendMessage ("SLAM_ALIGN_PATHS");
	}
}
