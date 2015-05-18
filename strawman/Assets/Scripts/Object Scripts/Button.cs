using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	public GameObject toEffect;
	public bool pressed;

	void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			if (Input.GetKeyDown(KeyCode.E)) {
				pressed = !pressed;
				toEffect.SetActive(!toEffect.activeSelf);
				if (pressed) 
					GetComponent<SpriteRenderer>().color = Color.blue;
				else
					GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
	}



}
