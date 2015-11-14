using UnityEngine;
using System.Collections;

public class Tau_explosion : MonoBehaviour {

	public float explosionDuration;

	// Use this for initialization
	void Start () 
	{
		Invoke("End", explosionDuration);
	}
	
	// Update is called once per frame
	void End () {
		Destroy(this.gameObject);
	}
}
