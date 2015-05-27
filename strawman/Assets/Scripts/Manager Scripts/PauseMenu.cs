using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	
	public GUISkin skin;
	public Material mat;
	public Color statColor = Color.yellow;
	public enum Page { None,Main,Options,Credits }
	
	private Page currentPage;
	private float savedTimeScale;
	
	
	void Start() 
	{
		Time.timeScale = 1;
	}	
	
	void LateUpdate () 
	{
		if (Input.GetKeyDown("escape")) 
		{
			switch (currentPage) 
			{
			case Page.None: 
				PauseGame(); 
				break;
				
			case Page.Main: 
				UnPauseGame(); 
				break;
				
			default: 
				currentPage = Page.Main;
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
			switch (currentPage) 
			{
			case Page.Main: MainPauseMenu(); break;
			case Page.Options: ShowBackButton();/*ShowToolbar();*/ break;
			}
		}   
	}
	
	void BeginPage(int width, int height) 
	{
		GUILayout.BeginArea( new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));
	}
	
	void EndPage() 
	{
		GUILayout.EndArea();
	}
	
	void ShowBackButton() 
	{
		if (GUI.Button(new Rect(20, Screen.height - 50, 50, 20),"Back")) 
			currentPage = Page.Main;
	}
	
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
		Time.timeScale = 0;
		AudioListener.pause = true;
		currentPage = Page.Main;
		GameManager.paused = true;
	}
	
	void UnPauseGame() 
	{
		Time.timeScale = savedTimeScale;
		AudioListener.pause = false;
		GameManager.paused = false;
		currentPage = Page.None;
	}
	
	bool IsGamePaused() 
	{
		return (Time.timeScale == 0);
	}
	
	void OnApplicationPause(bool pause) 
	{
		if (IsGamePaused())
			AudioListener.pause = true;
	}
}