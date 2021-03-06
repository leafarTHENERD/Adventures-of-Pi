﻿using UnityEngine;
using System.Collections;
using Prime31;


public partial class MainCharacter : MonoBehaviour
{
	// As funcionalidades estao modularizadas em arquivos diferentes, deixando o loop principal apenas como algoritmo.
	void Update()
	{
		ResetVerticalSpeedIfGrounded ();

		CheckMovementInput ();

		CheckJump ();

		ApplyGravity ();
		
		CheckDropJump ();

		CheckFallingAnimation();

		SmoothHorizontalSpeedAndMove ();

		StoreThisFrameMovementData ();

		if(Input.GetKeyDown(KeyCode.Escape))
			Application.LoadLevel(0);
	}
}
