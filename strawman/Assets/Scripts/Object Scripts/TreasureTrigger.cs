using UnityEngine;
using System.Collections;

public class TreasureTrigger : MonoBehaviour 
{
	public AudioSource sfxManager;	// attach the scenes SXF manager
	public AudioClip pickupSfx;		// sound clip to play when treasure is collected
	public GameObject deathWall;	// deathwall to activate
	public GameObject musicSource;	// access music change script on pickup
	public int hiddenIndex;			// used for populating the secret treasure array

	public bool level9 = false;	// used for KNOWN BUG 4 AND 34
	public int treasureOrder;	// used for KNOWN BUG 34
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
		if (gameObject.tag != "MainTreasure" && GameManager.manager.secrettreasureCollected[hiddenIndex]) 
			gameObject.SetActive(false);
		if (!level9)
			treasureOrder = 0;
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
			if (gameObject.tag == "MainTreasure" && !level9){
				sfxManager.PlayOneShot(pickupSfx, 1.0f);
				deathWall.SetActive(true);
				musicSource.GetComponent<MusicChange>().PlayHasteMusic();
				GameManager.manager.DoorUnlocked = true;
				shake = 0.5f;
			}
			//////////////////////////////////////////////////////////////
			// KNOWN BUG 4
			// made a special case bool that will not trigger the musicSource 
			// to play music again, instead only plays sfx and summons firewall
			// also check if loaded level is 9, so only the second treasure can unlock the door
			//////////////////////////////////////////////////////////////
				 

			//////////////////////////////////////////////////////////////
			/// KNOWN BUG 34
			/// making checks for both treasures on level 9
			//////////////////////////////////////////////////////////////
				 
			else if (level9 && treasureOrder == 1)	// for treasure from water wall
			{
				// Change music if no treasure was collected yet
				if (!GameManager.manager.levelNineFirst && !GameManager.manager.levelNineSecond)
					musicSource.GetComponent<MusicChange>().PlayHasteMusic();
				
				GameManager.manager.levelNineFirst = true;
				sfxManager.PlayOneShot(pickupSfx, 1.0f);
				deathWall.SetActive(true);
				shake = 0.5f;
			}
			else if (level9 && treasureOrder == 2)	// for treasure from fire wall
			{
				// Change music if no treasure was collected yet
				if (!GameManager.manager.levelNineFirst && !GameManager.manager.levelNineSecond)
					musicSource.GetComponent<MusicChange>().PlayHasteMusic();
				
				GameManager.manager.levelNineSecond = true;
				sfxManager.PlayOneShot(pickupSfx, 1.0f);
				deathWall.SetActive(true);
				shake = 0.5f;
			}
			else {
				GameManager.manager.secretGot = true;
				GameManager.manager.secrettreasureCollected[hiddenIndex] = true;
				sfxManager.PlayOneShot(pickupSfx, 1.0f);
			}
			// check if both treasures on level 9 were collected
			if (GameManager.manager.levelNineFirst && GameManager.manager.levelNineSecond)
				GameManager.manager.DoorUnlocked = true;
			//////////////////////////////////////////////////////////////
			// END KNOWN BUG 4 and 34
			//////////////////////////////////////////////////////////////
			
			Invoke ("DestroyAfterWait", 0.5f);
		}
	}
	void DestroyAfterWait()
	{
		Destroy (gameObject);
	}
}
