using UnityEngine;
using System.Collections;
using System;

public partial class Tau : MonoBehaviour 
{
	#region Phase 1

	void Phase1()
	{
		currentPhase = BossPhase.Phase1;

		if(Physics2D.Raycast(transform.position, transform.up, lineSight, collisionLayer))
			phase1Speed.y *= (-1);
		else if(Physics2D.Raycast(transform.position, -transform.up, lineSight, collisionLayer))
			phase1Speed.y *= (-1);	
		
		if(Physics2D.Raycast(transform.position, transform.right, lineSight, collisionLayer))
			phase1Speed.x *= (-1);
		else if(Physics2D.Raycast(transform.position, -transform.right, lineSight, collisionLayer))
			phase1Speed.x *= (-1);
		
		transform.position += phase1Speed;
	}

	#endregion

	#region Phase 2
	
	void Phase2_1()
	{
		currentPhase = BossPhase.PhaseTransition;

		float speedFactor = Vector3.Distance(transform.position, returnPoint);
		if(speedFactor > 0.1f)
		{
			speedFactor = 2.0f - (speedFactor / _myPhase2_InitialDistanceToCenter);
			transform.position = Vector3.Lerp(transform.position, returnPoint, speedFactor * phase2Speed * Time.deltaTime);
		}
		else
		{
			_myAnimator.Play(Animator.StringToHash("Tau_Action"));
			transform.position = returnPoint;
			Invoke("StartPhase2", 0.7f);
		}
	}

	void Phase2_2()
	{
		currentPhase = BossPhase.Phase2;
	}

	#endregion

	#region Phase 3

	void Phase3_1()
	{
		currentPhase = BossPhase.PhaseTransition;
		
		float speedFactor = Vector3.Distance(transform.position, returnPoint);
		if(speedFactor > 0.1f)
		{
			speedFactor = 2.0f - (speedFactor / _myPhase2_InitialDistanceToCenter);
			transform.position = Vector3.Lerp(transform.position, returnPoint, speedFactor * phase2Speed * Time.deltaTime);
		}
		else
		{
			_myAnimator.Play(Animator.StringToHash("Tau_Action"));
			transform.position = returnPoint;
			Invoke("StartPhase3", 0.7f);
		}
	}

	#endregion

	public void DyingPhase()
	{
		_myAmImmune = true;
	}

	#region PhaseChangers

	void PhaseNULL(){}

	void StartPhase1()
	{
		currentPhase = BossPhase.Phase1;
		ExecutePhase = Phase1;
		if(_myDoneFirstCicle)
			Invoke("Phase1to3", phase1Duration);
		else
			Invoke("Phase1to2", phase1Duration);
		_myAnimator.Play(Animator.StringToHash("Tau_Idle"));
	}

	void Phase1to2()
	{
		_myPhase2_InitialDistanceToCenter = Vector3.Distance(transform.position, Vector3.zero);
		ExecutePhase = Phase2_1;
	}

	void Phase1to3()
	{
		_myPhase2_InitialDistanceToCenter = Vector3.Distance(transform.position, Vector3.zero);
		ExecutePhase = Phase3_1;
	}

	void StartPhase2()
	{
		ExecutePhase = Phase2_2;
		Invoke("EndPhase2", phase2Duration);
	}

	void EndPhase2()
	{
		_myDoneFirstCicle = true;
		ExecutePhase = PhaseNULL;
		currentPhase = BossPhase.Phase2Ending;
		_myAnimator.Play(Animator.StringToHash("Tau_GatheringPower"));
		Invoke("StartPhase1", 3.0f);
	}

	void StartPhase3()
	{
		ExecutePhase = PhaseNULL;
		currentPhase = BossPhase.Phase3;
		//Invoke("EndPhase2", phase2Duration);
	}

	public void EndPhase3()
	{
		if(currentPhase != BossPhase.Dying)
		{
			_myDoneFirstCicle = false;
			StartPhase1 ();
		}
	}

	#endregion

	void Taunt()
	{
		_myAnimator.Play(Animator.StringToHash("Tau_Action"));
	}


//	void OnDrawGizmos()
//	{
//		Gizmos.color = Color.green;
//
//		Gizmos.DrawLine(transform.position, transform.position + (transform.right * lineSight * (-1)));
//		Gizmos.DrawLine(transform.position, transform.position + (transform.right * lineSight));
//		Gizmos.DrawLine(transform.position, transform.position + (transform.up * lineSight * (-1)));
//		Gizmos.DrawLine(transform.position, transform.position + (transform.up * lineSight));
//	}
}
