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
            GameManager.manager.levelCompleted[Application.loadedLevel - 6] = true;

            if (Application.loadedLevel == 6)//level 1
            {
                GameManager.manager.levelUnlocked[1] = true;
            }
            else if (Application.loadedLevel == 7)//level 2
            {
                GameManager.manager.levelUnlocked[2] = true;
            }
            if (Application.loadedLevel == 8)//level 3
            {
                GameManager.manager.levelUnlocked[3] = true;
            }
            else if(Application.loadedLevel == 9)//Level 4
            {
                GameManager.manager.levelUnlocked[4] = true;
                GameManager.manager.levelUnlocked[5] = true;
            }
            else if (Application.loadedLevel == 10)//level 5A
            {
                GameManager.manager.levelUnlocked[8] = true;
                GameManager.manager.levelUnlocked[7] = true;
            }
            else if (Application.loadedLevel == 11)//Level 5B
            {
                GameManager.manager.levelUnlocked[7] = true;
                GameManager.manager.levelUnlocked[6] = true;
            }
            else if (Application.loadedLevel == 12)//Level 6A
            {
                GameManager.manager.levelUnlocked[10] = true;
                GameManager.manager.levelUnlocked[11] = true;
            }
            else if (Application.loadedLevel == 13)//Level 6B
            {
                GameManager.manager.levelUnlocked[9] = true;
                GameManager.manager.levelUnlocked[10] = true;
                GameManager.manager.levelUnlocked[11] = true;
            }
            else if (Application.loadedLevel == 14)//Level 6C
            {
                GameManager.manager.levelUnlocked[9] = true;
                GameManager.manager.levelUnlocked[10] = true;
            }
            else if (Application.loadedLevel == 15)//Level 7A
            {
                GameManager.manager.levelUnlocked[13] = true;
            }
            else if (Application.loadedLevel == 16)//Level 7B
            {
                GameManager.manager.levelUnlocked[12] = true;
                GameManager.manager.levelUnlocked[13] = true;
            }
            else if (Application.loadedLevel == 17)//Level 7C
            {
                GameManager.manager.levelUnlocked[12] = true;
            }
            else if (Application.loadedLevel == 19 || Application.loadedLevel == 18)//Level 8A & 8B
            {
                GameManager.manager.levelUnlocked[14] = true;
            }
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
			Application.LoadLevel (1);
		// if level 1-3, load the next level
		else
			Application.LoadLevel (Application.loadedLevel + 1);
	}
}