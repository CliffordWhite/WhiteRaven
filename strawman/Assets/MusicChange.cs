using UnityEngine;
using System.Collections;

public class MusicChange : MonoBehaviour 
{
	public AudioSource musicSource;			// music source volume control
	public AudioClip backgroundCalm;		// normal background music
	public AudioClip backgroundHaste;		// chase background music

	void Start () 
	{
		if (musicSource == null)
			musicSource = GetComponent<AudioSource>();
		musicSource.PlayOneShot(backgroundCalm);
	}
	
	public void PlayHasteMusic()
	{
		musicSource.Stop();
		musicSource.PlayOneShot(backgroundHaste);
	}

}
