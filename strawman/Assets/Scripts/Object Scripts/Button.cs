using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	//object to effect, bool keeps track of state
	public GameObject toEffect;
	public bool pressed;

	void OnTriggerStay(Collider other) {
		//if player within range allow interaction
		if (other.tag == "Player") {
			if (Input.GetKeyDown(KeyCode.E)) {
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
