using UnityEngine;
using System.Collections;

public partial class MainCharacter : MonoBehaviour 
{

	private void CheckMovementInput()
	{
		if(MoveRightButtonPressed ())		MoveRight ();
		else if(MoveLeftButtonPressed ())	MoveLeft ();
		else 								StopMovingHorizontally ();
	}

	private bool MoveRightButtonPressed()
	{
		return (Input.GetKey( KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && receiveInput;
	}

	private bool MoveLeftButtonPressed()
	{
		return (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && receiveInput;
	}

	private bool JumpButtonPressed()
	{
		return (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && receiveInput;
	}

	private bool JumpButtonReleased()
	{
		return (Input.GetKeyUp( KeyCode.W ) || Input.GetKeyUp(KeyCode.UpArrow)) && receiveInput;
	}

	private bool DropJumpButtonPressed()
	{
		return (Input.GetKey( KeyCode.S ) || Input.GetKey(KeyCode.DownArrow)) && receiveInput;
	}
}
