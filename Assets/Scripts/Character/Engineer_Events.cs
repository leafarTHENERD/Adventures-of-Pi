using UnityEngine;
using System.Collections;
using Prime31;

public partial class Engineer : MonoBehaviour 
{
	// movement config
	public float gravity = -25f;
	//public float runSpeed = 8f;
	//public float groundDamping = 20f; // how fast do we change direction? higher means faster
	//public float inAirDamping = 5f;
	//public float jumpHeight = 3f;
	
	//[HideInInspector]
	//private float normalizedHorizontalSpeed = 0;
	
	private CharacterController2D _controller;
	private Animator _animator;
	//private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private float _myShootOffset;

	public GameObject projectileObject;
	public float firstShootTime;
	public float shootTimeDelay;
	public float projectileSpeed;
	public float projectileVerticalOscilation;

	public bool isActive = true;

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_animator.Play( Animator.StringToHash( "Engineer_Idle" ) );
		_controller = GetComponent<CharacterController2D>();
		
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

		_myShootOffset = IsFacingRight() ? 0.5f : -0.5f;

		Invoke("Shoot", firstShootTime);
	}
	
	
	#region Event Listeners
	
	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;
		
		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "ENGINEER flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}
	
	
	void onTriggerEnterEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}
	
	
	void onTriggerExitEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}
	
	#endregion
}
