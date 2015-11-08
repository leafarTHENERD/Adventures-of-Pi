using UnityEngine;
using System.Collections;
using Prime31;

public partial class MainCharacter : MonoBehaviour 
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	
	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	
	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

	private string _myAnimationIdle = "Pi_Idle";
	private string _myAnimationWalking = "Pi_Walking";
	private string _myAnimationJumping = "Pi_Jump";
	private string _myAnimationFalling = "Pi_Falling";
}
