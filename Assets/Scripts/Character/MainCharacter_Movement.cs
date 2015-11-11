using UnityEngine;
using System.Collections;
using Prime31;

public partial class MainCharacter : MonoBehaviour 
{
	private void ResetVerticalSpeedIfGrounded()
	{
		if( _controller.isGrounded )
			_velocity.y = 0;
	}

	private void MoveRight()
	{
		normalizedHorizontalSpeed = 1;
		if( transform.localScale.x < 0f )
			transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
		
		if( _controller.isGrounded && !_myWaitingForJumpAnticipationAnimation )
			_animator.Play( Animator.StringToHash( _myAnimationWalking ) );
	}

	private void MoveLeft()
	{
		normalizedHorizontalSpeed = -1;
		if( transform.localScale.x > 0f )
			transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
		
		if( _controller.isGrounded && !_myWaitingForJumpAnticipationAnimation )
			_animator.Play( Animator.StringToHash( _myAnimationWalking ) );
	}

	private void StopMovingHorizontally()
	{
		normalizedHorizontalSpeed = 0;
		
		if( _controller.isGrounded && !_myWaitingForJumpAnticipationAnimation)
			_animator.Play( Animator.StringToHash( _myAnimationIdle ) );
	}

	private void CheckJump()
	{
		if(_myJumpNextFrame)
		{
			Jump (jumpHeight);
		}
		// we can only jump whilst grounded
		else if( _controller.isGrounded && JumpButtonPressed () && !DropJumpButtonPressed () && !_myWaitingForJumpAnticipationAnimation)
		{
			_animator.Play( Animator.StringToHash( _myAnimationJumping ) );
			Invoke("JumpNextFrame", 0.05f);
			_myWaitingForJumpAnticipationAnimation = true;
		}
		else if(!_controller.isGrounded && JumpButtonReleased ())
		{
			CutOffJump ();
		}
	}

	private void JumpNextFrame()
	{
		_myJumpNextFrame = true;
	}

	private void Jump(float height)
	{
		_myWaitingForJumpAnticipationAnimation = false;
		_myJumpNextFrame = false;
		_velocity.y = height * Mathf.Abs(gravity);
		_velocity.y = 2.0f * _velocity.y;
		_velocity.y = Mathf.Sqrt(_velocity.y);
		if(gravity > 0)
			_velocity.y = _velocity.y * (-1.0f);
		//Debug.Log("JUMP POWER="+_velocity.y);
	}

	private void CutOffJump()
	{
		if(!_myWaitingForJumpAnticipationAnimation)
			_velocity.y *= 0.4f;
	}

	private void CheckDropJump()
	{
		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets uf jump down through one way platforms
		if( _controller.isGrounded && DropJumpButtonPressed () )
		{
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}
	}

	private void ApplyGravity()
	{
		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;
	}

	private void InvertGravity()
	{
		transform.localScale = transform.localScale * (-1);
		gravity = gravity * (-1);
		_controller.isGravityDown = !_controller.isGravityDown;

		Invoke("InvertGravityCameraAdjustment", 0.8f);
	}

	private void InvertGravityCameraAdjustment()
	{
		SmoothFollow sf = GameObject.FindObjectOfType<SmoothFollow>();
		if(sf != null)
		{
			sf.cameraOffset.y *= -1;
		}
	}

	private void CheckFallingAnimation()
	{
		if(_controller.BecameGroundedThisFrame)
		{
			_animator.Play( Animator.StringToHash( _myAnimationTouchingTheGround ) );
			WaitForAnimation ();
		}
		else if(!_controller.isGrounded)
		{
			if(_controller.isGravityDown)
			{
				if(_velocity.y < 0)
				{
					_animator.Play( Animator.StringToHash( _myAnimationFalling ) );
				}
			}
			else if(_velocity.y > 0)
			{
				_animator.Play( Animator.StringToHash( _myAnimationFalling ) );
			}
		}
	}

	private void SmoothHorizontalSpeedAndMove()
	{
		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
		
		_controller.move( _velocity * Time.deltaTime );
	}

	private void StoreThisFrameMovementData()
	{
		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}
}
