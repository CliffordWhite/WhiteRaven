using UnityEngine;
using System.Collections;

public class SaveLoadScript : MonoBehaviour 
{

	void OnGUI()
	{
		GUI.TextField(new Rect(Screen.width - 105, 10, 100, 20), "time: " + GameManager.manager.gameTime.ToString());
		GUI.TextField(new Rect(Screen.width - 105, 40, 100, 20), "keys: " + GameManager.manager.keys.ToString());
		GUI.TextField(new Rect(Screen.width - 105, 70, 100, 20), "lives: " + GameManager.manager.lives.ToString());
		if(GUI.Button (new Rect((Screen.width / 2) + 105, 100, 100, 25), "Save 1"))
		{
			GameManager.manager.save = 1;
			GameManager.manager.Save();
		}
		if(GUI.Button (new Rect((Screen.width / 2) + 105, 200, 100, 25), "Save 2"))
		{
			GameManager.manager.save = 2;
			GameManager.manager.Save();
		}
		if(GUI.Button (new Rect((Screen.width / 2) + 105, 300, 100, 25), "Save 3"))
		{
			GameManager.manager.save = 3;
			GameManager.manager.Save();
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 150, 100, 25), "Erase 1"))
		{
			GameManager.manager.save = 1;
			GameManager.manager.EraseFile();
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 250, 100, 25), "Erase 2"))
		{
			GameManager.manager.save = 2;
			GameManager.manager.EraseFile();
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 350, 100, 25), "Erase 3"))
		{
			GameManager.manager.save = 3;
			GameManager.manager.EraseFile();
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 100, 100, 25), "Load 1"))
		{
			GameManager.manager.Load (1);
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 200, 100, 25), "Load 2"))
		{
			GameManager.manager.Load (2);
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 300, 100, 25), "Load 3"))
		{
			GameManager.manager.Load (3);
		}
	}
}
