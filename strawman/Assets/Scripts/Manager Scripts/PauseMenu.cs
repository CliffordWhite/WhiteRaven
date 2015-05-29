using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	
	public GUISkin skin;		// load a custum GUISkin, currently takes a default
	public Color statColor = Color.yellow;	// sets the color of pause menu buttons
	public enum Page { None,Main,Options }	// change between different pause states
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
			case Page.Options: ShowBackButton();/*ShowToolbar();*/ break;
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
	void ShowBackButton() 
	{
		if (GUI.Button(new Rect(20, Screen.height - 50, 50, 20),"Back")) 
			currentPage = Page.Main;
	}

	// draw buttons for main pause menu
	void MainPauseMenu() 
	{
		BeginPage(200,200);
		if (GUILayout.Button ("Continue")) 
			UnPauseGame();
			
		if (GUILayout.Button ("Options")) 
			currentPage = Page.Options;
		EndPage();
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