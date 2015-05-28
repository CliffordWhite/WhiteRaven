using UnityEngine;
using System.Collections;

public class LockedDoor : MonoBehaviour {
	public AudioClip sound;
	public AudioSource SFXSource;

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player") {
			if (GameManager.manager.keys >= 1){
				GameManager.manager.keys--;
				SFXSource.PlayOneShot(sound,1.0f);
				gameObject.SetActive(false);
				GameManager.manager.keyShowTime = 2.0f;
			}
		}
	}
}
