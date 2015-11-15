using UnityEngine;
using System.Collections;

public partial class MainCharacter : MonoBehaviour 
{
	private void CheckEnemiesHit (GameObject go)
	{
		CheckEngineer (go);
		CheckTau (go);
	}

	private void CheckEngineer(GameObject go)
	{
		if(go.transform.parent != null)
		{
			if(go.transform.parent.CompareTag("Enemy"))
			{
				Engineer e = go.transform.parent.GetComponent<Engineer>();
				if(e != null)
				{
					if(!_controller.isGrounded)
					{
						if(e.isActive)
						{
							Jump (jumpHeight * 1.3f);
						}
						else
						{
							Jump (jumpHeight * 0.7f);
						}
						e.GotHit ();
					}
					else if(e.isActive)
					{
						Die ();
					}
				}
			}
		}
	}

	private void CheckTau(GameObject go)
	{
		if(go.CompareTag("Enemy"))
		{
			Tau t = go.GetComponent<Tau>();
			if(t != null)
			{
				if(!_controller.isGrounded)
				{
					if(!t.IsImmune)
					{
						if(t.GotHit ())
						{
							receiveInput = false;
						}

						Jump (jumpHeight * 0.8f);
					}
				}
				else if(!t.IsImmune)
				{	
					//Debug.Log("BOSSSSS HIT");
					Invoke("ResetBoss",0.05f);
					//Die ();
				}
			}
		}
	}

	private void ResetBoss(){
		//Debug.Log("BOSS HIT");
		Application.LoadLevel(Application.loadedLevel);
	}

	private void Die()
	{
		//Debug.Log(transform.position + "-" + Checkpoint.last_checkpoint);
		this.transform.position = Checkpoint.last_checkpoint + respawnOffset;

		SetGravityDown();

		receiveInput = true;

		_velocity = Vector3.zero;
	}
}
