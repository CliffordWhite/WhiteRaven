using UnityEngine;
using System.Collections;

public class AudioRange : MonoBehaviour {
	public AudioClip sound;
	public AudioSource SFXSource;
	float loopTime;

	void Start()
	{
		loopTime = 0.0f;
	}

	void OnTriggerStay()
	{
		if (loopTime <= 0.0f) {
			loopTime = sound.length;
			SFXSource.PlayOneShot (sound, 0.25f);
		}
		loopTime -= Time.deltaTime;
	}

	void OnTriggerExit()
	{
		SFXSource.Stop ();
		loopTime = 0.0f;
	}
}
