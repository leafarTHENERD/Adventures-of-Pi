using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public static Vector3 last_checkpoint;
	public Vector3 offset;
	private bool is_active;

	private Animator _myAnimator;
	private AudioSource _myAudioSource;

	// Use this for initialization
	void Start () {
		_myAnimator = GetComponent<Animator>();
		_myAudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void set_lastCheckpoint(){
		if(!this.is_active){
			last_checkpoint = this.transform.position;
			this.is_active = true;
			_myAudioSource.PlayOneShot(_myAudioSource.clip);
			Invoke("AnimateCheckpoint", 0.5f);
		}
	}

	public void AnimateCheckpoint()
	{
		_myAnimator.Play(Animator.StringToHash("Activating"));
	}
	
}
