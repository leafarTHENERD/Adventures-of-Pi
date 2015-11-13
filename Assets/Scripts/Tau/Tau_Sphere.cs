using UnityEngine;
using System.Collections;
using System;

public class Tau_Sphere : MonoBehaviour 
{
	public enum SphereType
	{
		Big,
		Small
	};

	//Privates
	private Tau _myTau;
	private Vector3 _myInitialPosition;
	private Vector3 _myPhase2Direction;
	private Action ExecutePhase;
	private Tau.BossPhase _myLastPhase;
	private bool _myCheckCollision;
	private Vector3 _myOutDir;
	private float _myInitialDistToTau;
	private float _myDistToTau;
	private bool _myPhase3Final;

	[Header("Object Attributes")]
	public SphereType type;
	public float rotationSpeed;
	public float orbitationSpeed;
	public float collisionDistance;



	// Use this for initialization
	void Start () 
	{
		_myInitialPosition = transform.localPosition;
		_myTau = GameObject.FindObjectOfType<Tau>();
		ExecutePhase = Phase1;
		_myLastPhase = Tau.BossPhase.PhaseTransition;
		_myCheckCollision = true;
	}

	private void Phase1()
	{
		Orbitate (orbitationSpeed);
	}

	private void Phase2()
	{
		if(type == SphereType.Big)
		{
			if(_myCheckCollision)
			{
				_myCheckCollision = false;

				if(Physics2D.Raycast(transform.position, transform.up, collisionDistance, _myTau.collisionLayer))
					_myPhase2Direction.y *= (-1);
				else if(Physics2D.Raycast(transform.position, -transform.up, collisionDistance, _myTau.collisionLayer))
					_myPhase2Direction.y *= (-1);	
				else if(Physics2D.Raycast(transform.position, transform.right, collisionDistance, _myTau.collisionLayer))
					_myPhase2Direction.x *= (-1);
				else if(Physics2D.Raycast(transform.position, -transform.right, collisionDistance, _myTau.collisionLayer))
					_myPhase2Direction.x *= (-1);
				else
					_myCheckCollision = true;

				if(!_myCheckCollision)
				{
					Invoke("ResetCollisionCheck", 0.1f);
				}
			}
			
			transform.position += _myPhase2Direction * (orbitationSpeed * 0.07f);
		}
		else
		{
			Orbitate (orbitationSpeed * 2.0f);
		}
	}

	private void Phase3()
	{
		if(type == SphereType.Big)
		{
			Orbitate (orbitationSpeed * 0.05f);
		}
		else
		{
			_myOutDir = transform.position - _myTau.transform.position;
			_myDistToTau = Vector3.Distance(transform.position, _myTau.transform.position);
			if(transform.parent != null)
			{
				if(!_myPhase3Final)
				{
					if(_myDistToTau < 10.0f)
					{
						transform.parent.position = transform.parent.position + (_myOutDir.normalized * orbitationSpeed * 0.01f);
					}
					else _myPhase3Final = true;
				}
				else
				{
					if(_myDistToTau > _myInitialDistToTau)
					{
						transform.parent.position = transform.parent.position - (_myOutDir.normalized * orbitationSpeed * 0.015f);
					}
					else
					{
						if(!_myTau.IsInvoking("EndPhase3"))
						{
							_myTau.Invoke("EndPhase3", 0.1f);
						}
					}
				}
				Orbitate(orbitationSpeed / _myDistToTau);
			}
		}
	}

	private void Phase2Ending()
	{
		if(type == SphereType.Big)
		{
			if(Vector3.Distance(transform.localPosition, _myInitialPosition) > 0.01f)
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, _myInitialPosition, Mathf.Abs(orbitationSpeed * 0.9f) * Time.deltaTime );
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + rotationSpeed);
			}
			else
			{
				transform.localPosition = _myInitialPosition;
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + rotationSpeed);
			}
		}
		else
		{
			Orbitate (orbitationSpeed * 2.0f);
		}
	}

	private void PhaseTransition()
	{
		if(type == SphereType.Big)
		{
			if(_myTau.FirstCicle)
				Orbitate (orbitationSpeed * 0.5f);
			else
				Orbitate (orbitationSpeed * 0.05f);
		}
		else
		{
			if(_myTau.FirstCicle)
				Orbitate (orbitationSpeed * 2.0f);
			else
				Orbitate (orbitationSpeed * 3.0f);
		}
	}


	// Update is called once per frame
	void Update () 
	{
		if(_myLastPhase != _myTau.currentPhase)
		{
			_myLastPhase = _myTau.currentPhase;
			switch(_myTau.currentPhase)
			{
				case Tau.BossPhase.Phase1:
					gameObject.SetActive(true);
					ExecutePhase = Phase1;			
					break;

				case Tau.BossPhase.Phase2:			
					_myPhase2Direction = UnityEngine.Random.insideUnitSphere;
					_myPhase2Direction.z = 0.0f;
					transform.rotation = Quaternion.identity;
					ExecutePhase = Phase2;			
					break;

				case Tau.BossPhase.Phase2Ending:
					ExecutePhase = Phase2Ending;
					break;

				case Tau.BossPhase.Phase3:
					_myInitialDistToTau = Vector3.Distance(transform.position, _myTau.transform.position);
					_myPhase3Final = false;
					ExecutePhase = Phase3;
					break;

				case Tau.BossPhase.PhaseTransition:	
					ExecutePhase = PhaseTransition;	
					break;

				case Tau.BossPhase.Dying:
					gameObject.SetActive(false);
					break;
			}
		}

		ExecutePhase ();
	}

	private void Orbitate(float speed)
	{
		if(type == SphereType.Big)
		{
			transform.RotateAround(_myTau.transform.position, Vector3.forward, speed);
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + rotationSpeed);
		}
		else
		{
			transform.parent.RotateAround(_myTau.transform.position, Vector3.forward, speed);
			transform.parent.rotation = Quaternion.identity;
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + rotationSpeed);
		}
	}

	private void ResetCollisionCheck()
	{
		_myCheckCollision = true;
	}
}
