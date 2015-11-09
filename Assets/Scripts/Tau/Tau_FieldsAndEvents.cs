using UnityEngine;
using System.Collections;
using System;

public partial class Tau : MonoBehaviour 
{
	public enum BossPhase
	{
		Phase1,
		Phase2,
		Phase2Ending,
		Phase3,
		PhaseTransition
	};
	
	void Start () 
	{
		_myDoneFirstCicle = false;
		_myAnimator = GetComponent<Animator>();
		ExecutePhase = PhaseNULL;
		Invoke("Taunt", 5.0f);
		Invoke("StartPhase1", 7.5f);
	}

	void Update () 
	{
		ExecutePhase ();
	}

	[Header("Control Variables")]
	public LayerMask collisionLayer;
	public float lineSight;

	[Header("Boss Values")]
	public BossPhase currentPhase;

	[Header("Object References")]
	public Tau_Sphere[] bigSpheres;
	public Tau_Sphere[] smallSpheres;

	//Phase 1
	[Header("Phase 1")]
	public float phase1Duration;
	public Vector3 phase1Speed;

	//Phase 2
	[Header("Phase 2")]
	public float phase2Duration;
	public Vector3 returnPoint;
	public float phase2Speed;
	private float _myPhase2_InitialDistanceToCenter;


	//Other privates
	private Animator _myAnimator;
	private Action ExecutePhase;
	private bool _myDoneFirstCicle;
	public bool FirstCicle
	{
		get
		{
			return !_myDoneFirstCicle;
		}
	}

}
