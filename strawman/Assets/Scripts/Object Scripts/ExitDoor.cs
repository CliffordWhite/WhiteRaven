using UnityEngine;
using System.Collections;

public class ExitDoor : MonoBehaviour 
{
	public AudioSource _SFXsource;
	public AudioClip doorSound;

	void OnTriggerEnter(Collider other)
	{
		// Load the next level when player runs into door
		if (other.tag == "Player")
		{
			float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
			_SFXsource.PlayOneShot(doorSound, 1.0f);
			Invoke("NextLevel",fadetime);
		}
	}

	void NextLevel()
	{
		// if current level is 4 or more, load back to main menu
		// load to level select instead once created
		if (Application.loadedLevel >= 9)
			Application.LoadLevel (0);
		// if level 1-3, load the next level
		else
			Application.LoadLevel (Application.loadedLevel + 1);
	}
}