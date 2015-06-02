using UnityEngine;
using System.Collections;

public class TreasureTrigger : MonoBehaviour 
{
	public AudioSource sfxManager;	// attach the scenes SXF manager
	public AudioClip pickupSfx;		// sound clip to play when treasure is collected
	public GameObject deathWall;	// deathwall to activate
	public GameObject musicSource;	// access music change script on pickup

	bool collected;					// flag on when treasure collected
	public bool Collected {
		get {
			return collected;
		}
	}

	void Start () 
	{
		collected = false;			// initialize bool to false
	}
	
	void OnTriggerEnter(Collider other)
	{
		// only trigger pickup once for player only
		if (other.tag == "Player" && !collected)
		{
			collected = true;
			sfxManager.PlayOneShot(pickupSfx, 1.0f);
			deathWall.SetActive(true);
			deathWall.GetComponent<FireDeathWall>().BringThePain();
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<ParticleSystem>().Stop();
			musicSource.GetComponent<MusicChange>().PlayHasteMusic();
			Invoke ("DestroyAfterWait", 0.5f);
            GameManager.manager.treasureCollected[Application.loadedLevel - 6] = true;
		}
	}
	void DestroyAfterWait()
	{
		Destroy (gameObject);
	}
}
