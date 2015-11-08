using UnityEngine;
using System.Collections;
using Prime31;

public partial class MainCharacter : MonoBehaviour 
{
	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}
	
	
	#region Event Listeners
	
	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;
		
		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}
	
	
	void onTriggerEnterEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );

		if(col.gameObject.transform.CompareTag("Cossine"))
		{
			InvertGravity ();
		}
		else if(col.gameObject.transform.CompareTag("EnemyProjectiles"))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
		else if(col.gameObject.transform.parent != null)
		{
			if(col.gameObject.transform.parent.CompareTag("Enemy"))
			{
				Engineer e = col.gameObject.transform.parent.GetComponent<Engineer>();
				if(e != null)
				{
					if(!_controller.isGrounded)
					{
						if(e.isActive)
						{
							Jump (jumpHeight * 1.3f);
						}
						else
						{
							Jump (jumpHeight * 0.7f);
						}
						e.GotHit ();
					}
					else if(e.isActive)
					{
						Application.LoadLevel(Application.loadedLevel);
					}
				}
			}
		}
	}
	
	
	void onTriggerExitEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}
	
	#endregion
}
