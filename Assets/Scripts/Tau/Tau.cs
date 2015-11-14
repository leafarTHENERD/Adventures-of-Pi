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

		if(currentPhase == BossPhase.Dead)
		{
			Debug.Log("A");
			_myDeathScreenColor.a = Mathf.Lerp(deathScreen.color.a, 0.0f, 0.8f * Time.deltaTime);
		}
		else if(_myFirstDyingCicle)
		{
			Debug.Log("B");
			_myDyingColor.a = Mathf.Lerp(_myRenderer.color.a, 0.0f, 2.8f * Time.deltaTime);
		}
		else
		{
			Debug.Log("C");
			_myDeathScreenColor.a = Mathf.Lerp(deathScreen.color.a, 1.0f, 1.8f * Time.deltaTime);

			_myDyingColor.r = Mathf.Lerp(_myRenderer.color.r, 0.0f, 4.8f * Time.deltaTime);
			_myDyingColor.g = Mathf.Lerp(_myRenderer.color.g, 0.0f, 4.8f * Time.deltaTime);
			_myDyingColor.b = Mathf.Lerp(_myRenderer.color.b, 0.0f, 4.8f * Time.deltaTime);
		}
		_myRenderer.color = _myDyingColor;
		deathScreen.color = _myDeathScreenColor;
	}

	#region PhaseChangers

	void PhaseNULL(){}

	void StartPhase1()
	{
		if(IsNotDying)
		{
			currentPhase = BossPhase.Phase1;
			ExecutePhase = Phase1;
			if(_myDoneFirstCicle)
				Invoke("Phase1to3", phase1Duration);
			else
				Invoke("Phase1to2", phase1Duration);
			_myAnimator.Play(Animator.StringToHash("Tau_Idle"));
		}
	}

	void Phase1to2()
	{
		if(IsNotDying)
		{
			_myPhase2_InitialDistanceToCenter = Vector3.Distance(transform.position, Vector3.zero);
			ExecutePhase = Phase2_1;
		}
	}

	void Phase1to3()
	{
		if(IsNotDying)
		{
			_myPhase2_InitialDistanceToCenter = Vector3.Distance(transform.position, Vector3.zero);
			ExecutePhase = Phase3_1;
		}
	}

	void StartPhase2()
	{
		if(IsNotDying)
		{
			ExecutePhase = Phase2_2;
			Invoke("EndPhase2", phase2Duration);
		}
	}

	void EndPhase2()
	{
		if(IsNotDying)
		{
			_myDoneFirstCicle = true;
			ExecutePhase = PhaseNULL;
			currentPhase = BossPhase.Phase2Ending;
			_myAnimator.Play(Animator.StringToHash("Tau_GatheringPower"));
			Invoke("StartPhase1", 3.0f);
		}
	}

	void StartPhase3()
	{
		if(IsNotDying)
		{
			ExecutePhase = PhaseNULL;
			currentPhase = BossPhase.Phase3;
			//Invoke("EndPhase2", phase2Duration);
		}
	}

	public void EndPhase3()
	{
		if(IsNotDying)
		{
			_myDoneFirstCicle = false;
			StartPhase1 ();
		}
	}

	public void Died()
	{
		currentPhase = BossPhase.Dying;
		ExecutePhase = DyingPhase;
		_myAnimator.Play(Animator.StringToHash("Tau_Die"));
		_myDyingColor = _myRenderer.color;
		_myDeathScreenColor = deathScreen.color;

		AudioClip _myClip = Resources.Load<AudioClip>("SoundsToBeLoaded/BossExplosion");
		AudioSource _myAudioSource = Camera.main.GetComponent<AudioSource>();
		if(_myAudioSource != null)
		{
			_myAudioSource.Stop ();
			_myAudioSource.clip = _myClip;
			_myAudioSource.loop = false;
			_myAudioSource.Play ();
			Invoke("Explosion", 0.3f);
			Invoke("EndDyingCicle", 2.5f);
			Invoke("EndDyingPhase", 5.0f);
		}
	}

	public void EndDyingCicle()
	{
		_myFirstDyingCicle = true;
	}

	public void EndDyingPhase()
	{
		currentPhase = BossPhase.Dead;
		Invoke("PlayWinMusic", 3.0f);
	}

	public void PlayWinMusic()
	{
		Invoke("EndGame", 4.0f);
		AudioClip _myClip = Resources.Load<AudioClip>("SoundsToBeLoaded/Stage Clear - Kirby and the Rainbow Curse [OST]");
		AudioSource _myAudioSource = Camera.main.GetComponent<AudioSource>();
		if(_myAudioSource != null)
		{
			_myAudioSource.Stop ();
			_myAudioSource.clip = _myClip;
			_myAudioSource.loop = false;
			_myAudioSource.Play ();
		}
	}

	public void EndGame()
	{
		Application.LoadLevel(0);
	}

	public bool IsNotDying
	{
		get 
		{
			return (currentPhase != BossPhase.Dying) && (currentPhase != BossPhase.Dead);
		}
	}

	#endregion

	void Taunt()
	{
		_myAnimator.Play(Animator.StringToHash("Tau_Action"));
	}
	
	void Explosion()
	{
		Vector3 pos = UnityEngine.Random.insideUnitSphere * 2.0f;
		pos.z = 0.0f;
		GameObject.Instantiate(explosionObject, pos, Quaternion.identity);
		if(currentPhase == BossPhase.Dying)
			Invoke("Explosion", 0.3f);
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
