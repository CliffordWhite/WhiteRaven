using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	
	public GUISkin skin;		// load a custum GUISkin, currently takes a default
	public Color statColor = Color.yellow;	// sets the color of pause menu buttons
	public enum Page { None,Main,Options,Music,SFX,LvlSel }	// change between different pause states
												// None = live gameplay
												// Main = main pause menu
												// Options = page to load stats	**Currently Not Implemented**

	private Page currentPage;			// which is current?
	private float savedTimeScale;		// should always be 1, but save it just in case
	
	
	void Start() 
	{
		Time.timeScale = 1;	// normal play speed
	}	
	
	void LateUpdate () 
	{
		if (Input.GetKeyDown("escape")) 
		{
			switch (currentPage) 
			{
			case Page.None: 
				PauseGame(); // pause game if live gameplay
				break;
				
			case Page.Main: 
				UnPauseGame(); // unpause if main pause menu
				break;
				
			default: 
				currentPage = Page.Main; // go to main pause menu from any pause submenu
				break;
			}
		}
	}
	
	void OnGUI () 
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
			case Page.LvlSel: Application.LoadLevel(1); UnPauseGame(); break;
			}
		}   
	}

	// helper function
	void BeginPage(int width, int height) 
	{
		GUILayout.BeginArea( new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));
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
		BeginPage(200,200);
		if (GUILayout.Button ("Continue")) 
			UnPauseGame();
			
		if (GUILayout.Button ("Options")) 
			currentPage = Page.Options;

		if (GUILayout.Button ("Return to Level Select"))
			currentPage = Page.LvlSel;
		EndPage();
	}

	void ShowOptions(){
		BeginPage (200, 200);
		if (GUILayout.Button ("Toggle Fullscreen")) {
			Screen.fullScreen = !Screen.fullScreen;
			GameManager.manager.isFullscreen = !GameManager.manager.isFullscreen;
		}
		if (GUILayout.Button ("Music Volume"))
			currentPage = Page.Music;
		if (GUILayout.Button ("SFX Volume"))
			currentPage = Page.SFX;
		if (GUILayout.Button("Back")) 
			currentPage = Page.Main;
		EndPage ();
	}

	void ShowMusic(){
		BeginPage (200, 200);
		GameManager.manager.MusicVolume = GUILayout.HorizontalSlider (GameManager.manager.MusicVolume, 0.0f, 10.0f);
		if (GUILayout.Button("Back"))
			currentPage = Page.Options;
		EndPage ();
	}

	void ShowSFX(){
		BeginPage (200, 200);
		GameManager.manager.SFXVolume = GUILayout.HorizontalSlider (GameManager.manager.SFXVolume, 0.0f, 10.0f);
		if (GUILayout.Button("Back"))
			currentPage = Page.Options;
		EndPage ();
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
	
}