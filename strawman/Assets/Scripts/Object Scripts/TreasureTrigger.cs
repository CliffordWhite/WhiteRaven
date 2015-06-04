using UnityEngine;
using System.Collections;

public class TreasureTrigger : MonoBehaviour 
{
	public AudioSource sfxManager;	// attach the scenes SXF manager
	public AudioClip pickupSfx;		// sound clip to play when treasure is collected
	public GameObject deathWall;	// deathwall to activate
	public GameObject musicSource;	// access music change script on pickup

    //Camera
     GameObject _Camera; // set this via inspector
     float shake = 0.0f;
     float shakeAmount = 0.1f;
     float decreaseFactor = 1.0f;
     //Player Pos
     GameObject Player;



	bool collected;					// flag on when treasure collected
	public bool Collected {
		get {
			return collected;
		}
	}



	void Start () 
	{
		collected = false;			// initialize bool to false
        _Camera = GameObject.FindWithTag("MainCamera");
        Player = GameObject.FindWithTag("Player");
	}

    void Update()
    {
        if (shake > 0)
        {
            _Camera.transform.localPosition = new Vector3((Random.Range(0.1f, 5.0f) * shakeAmount) + Player.transform.position.x, Player.transform.position.y, -10.0f);
            shake -= Time.deltaTime * decreaseFactor;

        }
        else if(shake < 0)
        {
            shake = 0.0f;
            _Camera.transform.localPosition = new Vector3(Player.transform.localPosition.x, Player.transform.localPosition.y, -10.0f);
        }
    }

	void OnTriggerEnter(Collider other)
	{
		// only trigger pickup once for player only
		if (other.tag == "Player" && !collected)
		{
			collected = true;
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<ParticleSystem>().Stop();
			if (gameObject.tag == "MainTreasure"){
				sfxManager.PlayOneShot(pickupSfx, 1.0f);
				deathWall.SetActive(true);
				deathWall.GetComponent<FireDeathWall>().BringThePain();
				musicSource.GetComponent<MusicChange>().PlayHasteMusic();
				GameManager.manager.DoorUnlocked = true;
				shake = 0.5f;
			}
			else {
				GameManager.manager.secretGot = true;
			}
			Invoke ("DestroyAfterWait", 0.5f);
		}
	}
	void DestroyAfterWait()
	{
		Destroy (gameObject);
	}
}
