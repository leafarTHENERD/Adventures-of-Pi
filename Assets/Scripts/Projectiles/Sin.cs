using UnityEngine;
using System.Collections;

public class Sin : MonoBehaviour 
{
	public float verticalCicle;
	public float horizontalCicle;
	public float horizontalSpeed;
	public float verticalSpeed;

	public GameObject particle;

	private Vector3 _myVelocity;

	void Start()
	{
		_myVelocity = Vector3.zero;
	}

	// Update is called once per frame
	void Update () 
	{
		transform.position = (transform.right * horizontalSpeed) + transform.position;

		_myVelocity.x = 0.0f;
		_myVelocity.y = Mathf.Sin (Time.time * horizontalCicle) * verticalCicle;

		particle.transform.localPosition = _myVelocity;
	}
}
