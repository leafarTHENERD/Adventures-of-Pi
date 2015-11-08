using UnityEngine;
using System.Collections;

public class SinCollider : MonoBehaviour {

	public LayerMask collisionMask;

	void Update () 
	{
		if(Physics2D.Raycast(transform.position, transform.right, 0.1f, collisionMask))
		{
			GameObject.Destroy(this.transform.parent.gameObject);
		}
	}
}
