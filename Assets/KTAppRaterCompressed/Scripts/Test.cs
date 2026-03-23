using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AppraterScript.SharedController().AppId = "827880613";
	}
	
	void OnGUI () {
		if (GUI.Button(new Rect(10, 70, 100, 60), "ShowPopup")) {
//			Application.LoadLevel("TestScene2");
			AppraterScript.ShowRaterPopup();
		}
	}
}
