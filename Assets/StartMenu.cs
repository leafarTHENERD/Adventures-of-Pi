using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
		{
			int sceneID = Random.Range(1,5);
			Application.LoadLevel(sceneID);
		}
	}
}
