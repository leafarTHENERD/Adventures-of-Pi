using UnityEngine;
using System.Collections;

public partial class Engineer : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
	{
		if(!_controller.isGrounded)
			_velocity.y += gravity;
		else
			_velocity.y = 0.0f;

		_controller.move (_velocity * Time.deltaTime);
	}

	public void GotHit()
	{
		if(isActive)
		{
			_animator.Play( Animator.StringToHash( "Engineer_Sleeping" ) );
			isActive = false;
			Invoke("GetUp", 10.0f);
		} 
		else
		{
			CancelInvoke("GetUp");
			Invoke("GetUp", 10.0f);
		}
	}

	public void GetUp()
	{
		_animator.Play( Animator.StringToHash( "Engineer_Waking" ) );
		isActive = true;
	}

	public void Shoot()
	{
		if(isActive)
		{
			_animator.Play( Animator.StringToHash( "Engineer_Shooting" ) );
			GameObject go = GameObject.Instantiate(projectileObject);
			Sin s = go.GetComponent<Sin>();
			if(s != null)
			{
				s.horizontalSpeed = projectileSpeed;
				s.verticalCicle = projectileVerticalOscilation;
			}
			Vector3 pos = transform.position;
			pos.y += GetComponent<SpriteRenderer>().bounds.size.y/2.0f;
			go.transform.position = pos;
			if(!IsFacingRight())
				go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
		}

		Invoke("Shoot", shootTimeDelay);
	}

	public bool IsFacingRight()
	{
		return transform.localScale.x < 0;
	}
}
