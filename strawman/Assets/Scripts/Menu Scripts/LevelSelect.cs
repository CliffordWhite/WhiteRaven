﻿using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour
{

    public int SceneNumber;
    public bool LevelBeat;
    public bool LevelUnlock;
	public string levelName;		// for determining which sprite to use
    public GUISkin skin;			// for using custom font
    string CheatCodeString = "Hello Jones";
    bool FlyModeOn;
    GameObject Player;
    bool ShowCheats;

    //To show hidden levels;
    GameObject Level2, Level4, Level5A, Level7A, Level9;




    // Use this for initialization
    void Start()
    {
        LevelUnlock = false;
        LevelBeat = false;

        for (int i = 0; i < 15; i++)
        {
            if (GameManager.manager.levelCompleted[i] && i == (SceneNumber - 6))
            {
                LevelBeat = true;
                LevelUnlock = true;
            }
        }
        for (int i = 0; i < 15; i++)
        {
            if (GameManager.manager.levelUnlocked[i] && i == (SceneNumber - 6))
            {
                LevelUnlock = true;
            }
        }
        if (!LevelUnlock)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, .5F);
        }

		// use treasure sprite if level was complete
		if (LevelBeat)
		{
			if (levelName == "tutorial")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().tutorialLevel;
			else if (levelName == "4")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level4;
			else if (levelName == "5a")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level5a;
			else if (levelName == "5b")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level5b;
			else if (levelName == "6a")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level6a;
			else if (levelName == "6b")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level6b;
			else if (levelName == "6c")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level6c;
			else if (levelName == "7a")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level7a;
			else if (levelName == "7b")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level7b;
			else if (levelName == "7c")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level7c;
			else if (levelName == "8a")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level8a;
			else if (levelName == "8b")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level8b;
			else if (levelName == "9")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level9b;
		}
	
        FlyModeOn = GameManager.manager.flyMode;
        Player = GameObject.FindWithTag("Player");
        ShowCheats = false;
        //Setting up which levels have secret treasures.
        Level2 = GameObject.FindWithTag("Level 2");
        Level4 = GameObject.FindWithTag("Level 4");
        Level5A = GameObject.FindWithTag("Level 5A");
        Level7A = GameObject.FindWithTag("Level 7A");
        Level9 = GameObject.FindWithTag("Level 9");

        //Making sure they are not on.
        if (!GameManager.manager.marcoPoloMode)
        {
            Level2.GetComponent<ParticleSystem>().Stop();
            Level4.GetComponent<ParticleSystem>().Stop();
            Level5A.GetComponent<ParticleSystem>().Stop();
            Level7A.GetComponent<ParticleSystem>().Stop();
            Level9.GetComponent<ParticleSystem>().Stop();
        }
    }

    void OnMouseOver()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (LevelUnlock)
            {
				//////////////////////////////////////////////
				/// FOUND BUG 2
				/// using the fade script of gameManager to 
				/// create an automatic fade when level is selected
				/// also rewrote LoadScene to accomodate an Invoke
				//////////////////////////////////////////////
				float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
				Invoke("LoadScene", fadetime);
                //LoadScene(SceneNumber);
            }
            //else
            //play error noise
        }
    }

    public void LoadScene(/*int level*/)
    {
        Application.LoadLevel(SceneNumber);
    }
	//////////////////////////////////////////////
	/// END FOUND BUG 2
	//////////////////////////////////////////////
	

    void OnGUI()
    {
        GUI.skin = skin;
        int Levelholder = 0;
        int TreasureHolder = 0;
        int STreasureHolder = 0;
        for (int i = 0; i < GameManager.manager.levelCompleted.Length; i++)
        {
            if (GameManager.manager.levelCompleted[i] == true)
            {
                Levelholder++;
            }

        }

        for (int i = 0; i < GameManager.manager.treasureCollected.Length; i++)
        {
            if (GameManager.manager.treasureCollected[i] == true)
            {
                TreasureHolder++;
            }
        }
        for (int i = 0; i < GameManager.manager.secrettreasureCollected.Length; i++)
        {
            if (GameManager.manager.secrettreasureCollected[i] == true)
            {
                STreasureHolder++;
            }
        }

        GUI.Label(new Rect(50, 100, 200, 200), Levelholder.ToString() + "/15 Levels Completed");
        GUI.Label(new Rect(50, 150, 200, 200), TreasureHolder.ToString() + "/13 Treasure Collected");
        GUI.Label(new Rect(50, 200, 200, 200), STreasureHolder.ToString() + "/7 Secret Treasure Collected");


        if (GUI.Button(new Rect(Screen.width - 105, 155, 110, 25), "Main Menu"))
        {
			//////////////////////////////////////
			/// FOUND BUG 2
			/// added fade script functionality
			//////////////////////////////////////
			float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
			Invoke("LoadMain", fadetime);
			//////////////////////////////////////
			/// END FOUND BUG 2
			//////////////////////////////////////
        }
        if (GUI.Button(new Rect(Screen.width - 105, 185, 110, 25), "Cheat Menu"))
        {
            ShowCheats = true;
        }
        if (ShowCheats)
            ShowCheatPage();

    }
	void LoadMain()
	{
		Application.LoadLevel(0);
	}
    public void ShowCheatPage()
    {
        GUILayout.BeginArea(new Rect((Screen.width - 200) / 2, (Screen.height - 200) / 2, 200, 200));
        CheatCodeString = GUILayout.TextField(CheatCodeString, 25);
        if (CheatCodeString == "FlyMode") // What you want them to type in.
        {
            FlyModeOn = !FlyModeOn;

            if (FlyModeOn)
                CheatCodeString = "Fly Mode On"; //To confrim
            else
                CheatCodeString = "Fly Mode Off";//To confrim
            GameManager.manager.flyMode = FlyModeOn;
        }
        else if (CheatCodeString == "AddLives") //What you want them to type in.
        {
            GameManager.manager.lives += 30;
            CheatCodeString = "Added 30 Lives"; // To confrim
        }
        else if (CheatCodeString == "GodMode")
        {
            GameManager.manager.godMode = !GameManager.manager.godMode;
            if (GameManager.manager.godMode)
                CheatCodeString = "God Mode On"; //To confrim
            else
                CheatCodeString = "God Mode Off"; //To confrim
        }
        else if (CheatCodeString == "Marco Polo")
        {
            GameManager.manager.marcoPoloMode = !GameManager.manager.marcoPoloMode;
            if (GameManager.manager.marcoPoloMode)
                CheatCodeString = "Marco Polo On"; //To confrim
            else
                CheatCodeString = "Marco Polo Off"; //To confrim
            ShowHiddenTreasure();
        }
        else if (CheatCodeString == "Unlock Levels")
        {
            for (int i = 0; i < 15; i++)
            {
                GameManager.manager.levelUnlocked[i] = true;
            }
            CheatCodeString = "Levels Unlocked";
            Application.LoadLevel(1);
        }

        if (GUILayout.Button("Back"))
        {
            ShowCheats = false;
        }
        GUILayout.EndArea();
    }
    void ShowHiddenTreasure()
    {
        if (GameManager.manager.marcoPoloMode)
        {
            Level2.GetComponent<ParticleSystem>().Play();
            Level4.GetComponent<ParticleSystem>().Play();
            Level5A.GetComponent<ParticleSystem>().Play();
            Level7A.GetComponent<ParticleSystem>().Play();
            Level9.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            Level2.GetComponent<ParticleSystem>().Stop();
            Level4.GetComponent<ParticleSystem>().Stop();
            Level5A.GetComponent<ParticleSystem>().Stop();
            Level7A.GetComponent<ParticleSystem>().Stop();
            Level9.GetComponent<ParticleSystem>().Stop();
        }
    }
}
