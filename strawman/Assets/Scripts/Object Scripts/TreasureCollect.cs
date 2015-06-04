﻿using UnityEngine;
using System.Collections;

public class TreasureCollect : MonoBehaviour {

    public AudioSource sfxManager;	// attach the scenes SXF manager
    public AudioClip pickupSfx;		// sound clip to play when treasure is collected
  
    bool collected;					// flag on when treasure collected
    public bool Collected
    {
        get
        {
            return collected;
        }
    }

    void Start()
    {
        collected = false;			// initialize bool to false
        if(GameManager.manager.secrettreasureCollected[Application.loadedLevel - 6] == true)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // only trigger pickup once for player only
        if (other.tag == "Player" && !collected)
        {
            collected = true;
            sfxManager.PlayOneShot(pickupSfx, 1.0f);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<ParticleSystem>().Stop();
			GameManager.manager.secretGot = true;
        }
    }
    void DestroyAfterWait()
    {
        Destroy(gameObject);
    }
}
