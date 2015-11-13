using UnityEngine;
using System.Collections;
using Prime31;

public partial class MainCharacter : MonoBehaviour 
{
	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

		_myWaitingForJumpAnticipationAnimation = false;
		_myJumpNextFrame = false;
		_myWasGroundedLastFrame = false;
		_myWaitingForTouchingGround = false;

		receiveInput = true;

		Checkpoint.last_checkpoint = new Vector3(transform.position.x,transform.position.y,transform.position.z);
	}
	
	
	#region Event Listeners
	
	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;
		
		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}
	
	
	void onTriggerEnterEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );

		if(col.gameObject.CompareTag("Cossine"))
		{
			InvertGravity ();
		}
		else if(col.gameObject.CompareTag("EnemyProjectiles"))
		{
			Die ();
		}
		else if(col.gameObject.CompareTag("Respawn")){
			Debug.Log("OI");
			Checkpoint c = col.gameObject.GetComponent<Checkpoint>();
			c.set_lastCheckpoint();
		}
		else if(col.gameObject.CompareTag("End")){
			Invoke("BossPhase",5.0f);

			receiveInput = false;

			AudioSource audio = Camera.main.GetComponent<AudioSource>();
			if(audio != null){
				audio.Stop();
				//audio.clip
				audio.Play();
			}
		}

		CheckEnemiesHit (col.gameObject);
	}
	
	
	void onTriggerExitEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}
	
	#endregion

	public void BossPhase(){
		Application.LoadLevel("BossScene");
	}

}
