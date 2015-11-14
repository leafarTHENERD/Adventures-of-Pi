using UnityEngine;
using System.Collections;

public class Cossine : MonoBehaviour {

	// Update is called once per frame
	void Update () 
	{
		transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.eulerAngles.y + 3.0f, transform.rotation.z);
	}
}
