using UnityEngine;
using System.Collections;

public class AudioRange : MonoBehaviour {
	public AudioClip sound;
	public AudioSource SFXSource;
	public GameObject soundObj;
	float loopTime;

	void Start()
	{
		loopTime = 0.0f;
	}

	void OnTriggerStay()
	{
		if (soundObj.activeSelf) {
			if (loopTime <= 0.0f) {
				loopTime = sound.length;
				SFXSource.PlayOneShot (sound, 0.25f);
			}
			loopTime -= Time.deltaTime;
		} else {
			SFXSource.Stop ();
			loopTime = 0.0f;
		}
	}

	void OnTriggerExit()
	{
		SFXSource.Stop ();
		loopTime = 0.0f;
	}
}
