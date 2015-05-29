using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour {
	public AudioClip sound;
	public AudioSource SFXSource;

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			SFXSource.PlayOneShot(sound,1.0f);
			gameObject.SetActive(false);
			GameManager.manager.keys++;
			GameManager.manager.keyShowTime = 2.0f;
		}

	}
}
