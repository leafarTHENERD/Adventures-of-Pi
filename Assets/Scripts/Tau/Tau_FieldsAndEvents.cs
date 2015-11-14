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
		PhaseTransition,
		Dying
	};
	
	void Start () 
	{
		_myAmImmune = false;
		_myDoneFirstCicle = false;
		_myAnimator = GetComponent<Animator>();
		_myRenderer = GetComponent<SpriteRenderer>();
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
	public int bossMaxHealth;
	public int bossCurrentHealth;
	public float immunityTime;

	[Header("Object References")]
	public Tau_Sphere[] bigSpheres;
	public Tau_Sphere[] smallSpheres;
	public SpriteRenderer deathScreen;

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
	private SpriteRenderer _myRenderer;
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
	private bool _myAmImmune;
	public bool IsImmune
	{
		get
		{
			return _myAmImmune;
		}
	}

	public bool GotHit()
	{
		bool toReturn = false;
		_myAmImmune = true;
		bossCurrentHealth = bossCurrentHealth - 1;
		if(bossCurrentHealth == 0)
		{
			toReturn = true;
			Died ();
		}
		else Invoke("LoseImmunity", immunityTime);

		return toReturn;
	}

	private void LoseImmunity()
	{
		_myAmImmune = false;
	}
}
