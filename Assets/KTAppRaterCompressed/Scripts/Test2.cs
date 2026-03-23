using UnityEngine;
using System.Collections;

public class Test2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(replace());
	}

	IEnumerator replace () {
		yield return new WaitForSeconds(2.0f);
//		Debug.Break();
		Application.LoadLevel("TestScene");
	}
}
