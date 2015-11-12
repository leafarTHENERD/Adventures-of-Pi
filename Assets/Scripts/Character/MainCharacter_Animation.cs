using UnityEngine;
using System.Collections;

public partial class MainCharacter : MonoBehaviour 
{
	enum AnimationState
	{
		Idle,
		Walking,
		Jumping,
		Falling
	};

	private AnimationState _myAnimationState;
	private AnimationState _myLastState;

	private void UpdateAnimation()
	{
	}

	private void ChangeAnimation(AnimationState newState)
	{

	}

}
