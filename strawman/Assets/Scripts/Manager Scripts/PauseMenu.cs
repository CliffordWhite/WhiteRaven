﻿using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{

    public GUISkin skin;		// load a custum GUISkin, currently takes a default
    public Color statColor = Color.yellow;	// sets the color of pause menu buttons
    public enum Page { None, Main, Options, Music, SFX, LvlSel, CheatCode, Treasure }	// change between different pause states
	public GameObject treasurePanel;	// MUST be set with treasure panel child
	public GameObject treasureBack;		// button on treasure page
    // None = live gameplay
    // Main = main pause menu
    // Options = page to load stats	**Currently Not Implemented**

    private Page currentPage;			// which is current?
    private float savedTimeScale;		// should always be 1, but save it just in case
    string CheatCodeString = "Hello Jones";
    bool FlyModeOn;
    GameObject Player;
    GameObject ExitDoor;
    void Start()
    {
        Time.timeScale = 1;	// normal play speed
        FlyModeOn = GameManager.manager.flyMode;
        Player = GameObject.FindWithTag("Player");
        ExitDoor = GameObject.FindWithTag("ExitDoor");
		if (treasurePanel)
			treasurePanel.SetActive(false);
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown("escape") && !ExitDoor.GetComponent<ExitDoor>().IsExiting)
        {
            switch (currentPage)
            {
                case Page.None:
                    PauseGame(); // pause game if live gameplay
                    break;

                case Page.Main:
                    UnPauseGame(); // unpause if main pause menu
                    break;
				case Page.Treasure:
					treasurePanel.SetActive(false);
					currentPage = Page.Main;
					break;

                default:
                    currentPage = Page.Main; // go to main pause menu from any pause submenu
                    break;
            }
        }
    }

    void OnGUI()
    {
        if (skin != null)
            GUI.skin = skin;
        if (IsGamePaused())
        {
            GUI.color = statColor;
            switch (currentPage) // draw menu based on current page
            {
                case Page.Main: MainPauseMenu(); break;
                case Page.Options: ShowOptions(); break;
                case Page.Music: ShowMusic(); break;
                case Page.SFX: ShowSFX(); break;
                //case Page.LvlSel: Application.LoadLevel(1); UnPauseGame; break;
                case Page.CheatCode: ShowCheatCode(); break;
				case Page.Treasure: ShowTreasure(); break;
            }
        }
    }
    // helper function
    void BeginPage(int width, int height)
    {
        GUILayout.BeginArea(new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));
    }

    // helper function
    void EndPage()
    {
        GUILayout.EndArea();
    }

    // button to return to main pause from any submenu


    // draw buttons for main pause menu
    void MainPauseMenu()
    {
        if (CheatCodeString != "Hello Jones")
        {
            CheatCodeString = "Hello Jones"; //reset string when you go back to main pause menu
        }
        BeginPage(200, 200);
        if (GUILayout.Button("Continue"))
            UnPauseGame();

        if (GUILayout.Button("Options"))
            currentPage = Page.Options;

        if (GUILayout.Button("Return to Level Select"))
		{
			////////////////////////////////////////
			/// FOUND BUG 2
			/// added fade script functionality
			////////////////////////////////////////
			UnPauseGame();	// game must be unpaused for fade to begin
			float fadetime = GameManager.manager.GetComponent<Fade>().BeginFade(1);
			Invoke("LevelSelect", fadetime);
			////////////////////////////////////////
			/// END FOUND BUG 2
			////////////////////////////////////////
		}
        if (GUILayout.Button("Cheat Code"))
            currentPage = Page.CheatCode;

		if (GUILayout.Button("Treasures"))
			currentPage = Page.Treasure;
        EndPage();
    }

	void LevelSelect()
	{
		Application.LoadLevel(1);
	}

    void ShowOptions()
    {
        BeginPage(200, 200);
		///////////////////////////////////////////////
		// FOUND BUG 15 
		// using setResolution
		///////////////////////////////////////////////
		
        if (GUILayout.Button("Toggle Fullscreen"))
        {
            //Screen.fullScreen = !Screen.fullScreen;
            GameManager.manager.isFullscreen = !GameManager.manager.isFullscreen;
			Screen.SetResolution(Screen.width, Screen.height, GameManager.manager.isFullscreen);
        }
		///////////////////////////////////////////////
        // END FOUND BUG 15
		///////////////////////////////////////////////
		
		if (GUILayout.Button("Music Volume"))
            currentPage = Page.Music;
        if (GUILayout.Button("SFX Volume"))
            currentPage = Page.SFX;
        if (GUILayout.Button("Back"))
            currentPage = Page.Main;
        EndPage();
    }

    void ShowMusic()
    {
        BeginPage(200, 200);
        GameManager.manager.MusicVolume = GUILayout.HorizontalSlider(GameManager.manager.MusicVolume, 0.0f, 10.0f);
        if (GUILayout.Button("Back"))
            currentPage = Page.Options;
        EndPage();
    }

    void ShowSFX()
    {
        BeginPage(200, 200);
        GameManager.manager.SFXVolume = GUILayout.HorizontalSlider(GameManager.manager.SFXVolume, 0.0f, 10.0f);
        if (GUILayout.Button("Back"))
            currentPage = Page.Options;
        EndPage();
    }

	void ShowTreasure()
	{
		if (treasurePanel)
			treasurePanel.SetActive(true);
	}

	public void BackButton()
	{
		if (treasurePanel)
			treasurePanel.SetActive(false);
		currentPage = Page.Main;
	}
    void PauseGame()
    {
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;		// this is what pauses the game
        AudioListener.pause = true;
        currentPage = Page.Main;
        GameManager.paused = true;
    }

    void UnPauseGame()
    {
        Time.timeScale = savedTimeScale;	// this unpauses the game
        AudioListener.pause = false;
        GameManager.paused = false;
        currentPage = Page.None;
    }

    bool IsGamePaused()
    {
        return (Time.timeScale == 0);
    }
    void ShowCheatCode() // Where to place more CheatCodes;
    {
        BeginPage(200, 200);
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
        else if (CheatCodeString == "Unlock Levels")
        {
            for (int i = 0; i < 15; i++)
            {
                GameManager.manager.levelUnlocked[i] = true;
            }
            CheatCodeString = "Levels Unlocked";
        }
        if (GUILayout.Button("Back"))
            currentPage = Page.Main;
        EndPage();

    }
}