using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	//object to effect, bool keeps track of state
	public GameObject toEffect;
	public bool pressed;
	public AudioClip clickSound;
	public AudioSource SFXSource;
	
	void OnTriggerStay(Collider other) {
		//if player within range allow interaction
		if (other.tag == "Player") {
			if (Input.GetKeyDown(KeyCode.E)) {
				SFXSource.PlayOneShot (clickSound, 1.0f);
				pressed = !pressed;
				toEffect.SetActive(!toEffect.activeSelf);
				//change color on click
				if (pressed) 
					GetComponent<SpriteRenderer>().color = Color.blue;
				else
					GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
	}



}
