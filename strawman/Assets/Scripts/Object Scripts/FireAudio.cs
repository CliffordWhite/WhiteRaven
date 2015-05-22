using UnityEngine;
using System.Collections;

public class FireAudio : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GetComponent<AudioSource>().volume = GameManager.manager.SFXVolume*0.01f;
	}
}
