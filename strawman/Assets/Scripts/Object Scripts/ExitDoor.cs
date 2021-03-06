﻿using UnityEngine;
using System.Collections;

public class ExitDoor : MonoBehaviour 
{
	public AudioSource _SFXsource;
	public AudioClip doorSound;
    public bool IsExiting;

	void Start(){
		GameManager.manager.secretGot = false;
		GameManager.manager.DoorUnlocked = false;
        GameManager.manager.isExiting = false;
	}

	void OnTriggerEnter(Collider other)
	{
		// Load the next level when player runs into door
		if (other.tag == "Player" && (GameManager.manager.DoorUnlocked || Application.loadedLevel < 9))
		{
			GameManager.manager.isExiting = true;
            GameManager.manager.levelCompleted[Application.loadedLevel - 6] = true;
            if(Application.loadedLevel > 8)
    			GameManager.manager.treasureCollected[Application.loadedLevel-6] = true;
			
            if (GameManager.manager.secretGot){
//				GameManager.manager.secrettreasureCollected[Application.loadedLevel-6]=true; // commented out to refactor secret array
				//Check if achieved all treasure
				int numGot = 0;
				for (int i = 0; i < GameManager.manager.secrettreasureCollected.Length; i++) {
					if (GameManager.manager.secrettreasureCollected[i])
						numGot++;
				}
				if (numGot == 7) 
					GameManager.manager.achieveList[6] = true;
			}
			//Check if completed all levels and give achieve
			bool complete = true;
			for (int l = 0; l < GameManager.manager.levelCompleted.Length; l++) {
				if (!GameManager.manager.levelCompleted[l]){
					complete = false;
					break;
				}
			}
			if (complete){
				GameManager.manager.achieveList[1] = true;
				if (GameManager.manager.hardModeOn)
					GameManager.manager.achieveList[3] = true;
				if (GameManager.manager.timeAttackOn && GameManager.manager.gameTime <= 1800.0f)
					GameManager.manager.achieveList[5] = true;
			}

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
                GameManager.manager.levelUnlocked[6] = true;
                GameManager.manager.levelUnlocked[7] = true;
            }
            else if (Application.loadedLevel == 11)//Level 5B
            {
                GameManager.manager.levelUnlocked[7] = true;
                GameManager.manager.levelUnlocked[8] = true;
            }
            else if (Application.loadedLevel == 12)//Level 6A
            {
				GameManager.manager.levelUnlocked[9] = true;
                GameManager.manager.levelUnlocked[10] = true;
            }
            else if (Application.loadedLevel == 13)//Level 6B
            {
                GameManager.manager.levelUnlocked[9] = true;
                GameManager.manager.levelUnlocked[10] = true;
                GameManager.manager.levelUnlocked[11] = true;
            }
            else if (Application.loadedLevel == 14)//Level 6C
            {
                GameManager.manager.levelUnlocked[10] = true;
				GameManager.manager.levelUnlocked[11] = true;
            }
            else if (Application.loadedLevel == 15)//Level 7A
            {
                GameManager.manager.levelUnlocked[12] = true;
            }
            else if (Application.loadedLevel == 16)//Level 7B
            {
                GameManager.manager.levelUnlocked[12] = true;
                GameManager.manager.levelUnlocked[13] = true;
            }
            else if (Application.loadedLevel == 17)//Level 7C
            {
                GameManager.manager.levelUnlocked[13] = true;
            }
            else if (Application.loadedLevel == 19 || Application.loadedLevel == 18)//Level 8A & 8B
            {
                GameManager.manager.levelUnlocked[14] = true;
            }
			else if (Application.loadedLevel == 20) // Level 9
			{
				///////////////////////////////////////////////
				/// KNOWN BUG 5
				/// manually setting final treasure to collected
				/// since this statement can only be reached if 
				/// it was collected
				///////////////////////////////////////////////
				GameManager.manager.treasureCollected[15] = true;
				///////////////////////////////////////////////
				/// END KNOWN BUG 5
				///////////////////////////////////////////////
				GameManager.manager.achieveList[0] = true;
				if (GameManager.manager.hardModeOn)
					GameManager.manager.achieveList[2] = true;
				if (GameManager.manager.timeAttackOn && GameManager.manager.gameTime <= 600.0f)
					GameManager.manager.achieveList[4] = true;
			}
            if (!GameManager.manager.webMode) // not web
                GameManager.manager.Save();
            else
                GameManager.manager.PlayerPrefSave();

			
            float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
			_SFXsource.PlayOneShot(doorSound, 1.0f);
			Invoke("NextLevel",fadetime);

            //Debug.Log("attempting to load level 2 from exit door");
            //Application.LoadLevel("Level 2");
		}
	}

	void NextLevel()
	{
		// if current level is 4 or more, load back to Level Select
		// load to level select instead once created
        if (Application.loadedLevel >= 9 && Application.loadedLevel != 20)
			LoadNewLevel (1);
            //Application.LoadLevel(1);
        // if level 1-3, load the next level
        else if (Application.loadedLevel == 20) {
			GameManager.manager.endGameTransition = true;
			LoadNewLevel (5);
		}
            //Application.LoadLevel(5);
        else
            LoadNewLevel(Application.loadedLevel + 1);
			//Application.LoadLevel (Application.loadedLevel + 1);
	}
    void LoadNewLevel(int Level)
    {
        Debug.Log("attempting to load level "+Level);
        if (Application.CanStreamedLevelBeLoaded(Level))
        {
            Application.LoadLevel(Level);
        }
    }
}
