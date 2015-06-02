using UnityEngine;
using System.Collections;

public class SFXVolumeSet : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<AudioSource> ().volume = GameManager.manager.SFXVolume * 0.1f;
	}
}
