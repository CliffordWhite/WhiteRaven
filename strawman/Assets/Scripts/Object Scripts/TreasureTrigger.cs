using UnityEngine;
using System.Collections;

public class TreasureTrigger : MonoBehaviour 
{
	public AudioSource sfxManager;	// attach the scenes SXF manager
	public AudioClip pickupSfx;		// sound clip to play when treasure is collected
	public GameObject deathWall;	// deathwall to activate
	public GameObject musicSource;	// access music change script on pickup

    //Camera
    // GameObject camera; // set this via inspector
   //  float shake = 0.0f;
    // float shakeAmount = 0.1f;
   //  float decreaseFactor = 1.0f;
     //Player Pos
   //  GameObject Player;



	bool collected;					// flag on when treasure collected
	public bool Collected {
		get {
			return collected;
		}
	}



	void Start () 
	{
		collected = false;			// initialize bool to false
       // camera = GameObject.FindWithTag("MainCamera");
      //  Player = GameObject.FindWithTag("Player");
	}

    void Update()
    {
      /*  if (shake > 0)
        {
            camera.transform.localPosition = Player.transform.position * shakeAmount;
            shake -= Time.deltaTime * decreaseFactor;

        }
        else if(shake < 0)
        {
            shake = 0.0f;
            camera.transform.localPosition = new Vector3(Player.transform.localPosition.x,Player.transform.localPosition.y,-10.0f);
        }*/
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
			GameManager.manager.DoorUnlocked = true;
            //shake = 0.5f;

		}
	}
	void DestroyAfterWait()
	{
		Destroy (gameObject);
	}
}
