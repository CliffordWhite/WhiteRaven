using UnityEngine;
using System.Collections;

public class VolumeSet : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<AudioSource> ().volume = GameManager.manager.MusicVolume * 0.1f;
	}
}
