using UnityEngine;
using System.Collections;

public class TreasureTrigger : MonoBehaviour 
{
	public AudioSource sfxManager;	// attach the scenes SXF manager
	public AudioClip pickupSfx;		// sound clip to play when treasure is collected
	public GameObject deathWall;	// deathwall to activate
	private bool collected;			// flag on when treasure collected

	void Start () 
	{
		collected = false;			// initialize bool to false
		deathWall.SetActive (false);// set activate wall to false
	}
	
	void OnTriggerEnter(Collider other)
	{
		// only trigger pickup once for player only
		if (other.tag == "Player" && !collected)
		{
			collected = true;
			sfxManager.PlayOneShot(pickupSfx, 1.0f);
			deathWall.SetActive(true);
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<ParticleSystem>().Stop();
			Invoke ("DestroyAfterWait", 1.0f);
		}
	}
	void DestroyAfterWait()
	{
		Destroy (gameObject);
	}
}
