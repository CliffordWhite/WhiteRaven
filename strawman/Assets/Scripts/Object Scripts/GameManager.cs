using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//Our singleton that takes care of game data
public class GameManager : MonoBehaviour 
{
	public static GameManager manager; //allow singleton access

	public int lives, keys; // number of lives for hard, number of keys held
	public float gameTime; // elapsed time for time attack
	public bool hardModeOn, timeAttackOn; // flags for game modes
	public bool[] treasureCollected; // flags for collected treasures
	public int save; // which save slot
	public string saveName; // the name of the save

	void Awake () 
	{
		if (manager == null) // assign this one as our singleton if it's the first
		{
			manager = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (manager != this) // destroy this one if another already exists
		{
			Destroy(gameObject);
		}
	}

	// public function to save the current content from anywhere with this object
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();

		string saveDestination; // seperate save file for each slot
		if (save == 1)
			saveDestination = Application.persistentDataPath + "/saveOne.dat";
		else if (save == 2)
			saveDestination = Application.persistentDataPath + "/saveTwo.dat";
		else if (save == 3)
			saveDestination = Application.persistentDataPath + "/saveThree.dat";
		else
			return;
		Debug.Log (saveDestination);
		FileStream file = File.Create (saveDestination);

		// store all the game information into our container class
		GameInfo info = new GameInfo();
		info.Keys = keys;
		info.Lives = lives;
		info.GameTime = gameTime;
		info.HardModeOn = hardModeOn;
		info.TimeAttackOn = timeAttackOn;
		info.Save = save;
		info.TreasureCollected = treasureCollected;

		bf.Serialize (file, info);
		file.Close ();
	}

	// used to clear a save file
	public void EraseFile()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		
		string saveDestination; // seperate save file for each slot
		if (save == 1)
			saveDestination = Application.persistentDataPath + "/saveOne.dat";
		else if (save == 2)
			saveDestination = Application.persistentDataPath + "/saveTwo.dat";
		else if (save == 3)
			saveDestination = Application.persistentDataPath + "/saveThree.dat";
		else
			return;
		Debug.Log (saveDestination);
		FileStream file = File.Create (saveDestination);
		
		// store all the game information into our container class
		GameInfo info = new GameInfo();
		info.Keys = 0;
		info.Lives = 0;
		info.GameTime = 0;
		info.HardModeOn = false;
		info.TimeAttackOn = false;
		info.Save = save;
		info.TreasureCollected = treasureCollected;
		
		bf.Serialize (file, info);
		file.Close ();
	}

	// public function to load the passed in save from anywhere with this object
	public void Load(int saveToOpen)
	{
		string loadFile; // file to load
		if (saveToOpen == 1)
			loadFile = Application.persistentDataPath + "/saveOne.dat";
		else if (saveToOpen == 2)
			loadFile = Application.persistentDataPath + "/saveTwo.dat";
		else if (saveToOpen == 3)
			loadFile = Application.persistentDataPath + "/saveThree.dat";
		else
			return;

		if(File.Exists(loadFile))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(loadFile, FileMode.Open);
			GameInfo info = (GameInfo)bf.Deserialize(file);
			file.Close();

			// set values of our manager to those loaded from save
			keys = info.Keys;
			lives = info.Lives;
			gameTime = info.GameTime;
			hardModeOn = info.HardModeOn;
			timeAttackOn = info.TimeAttackOn;
			save = info.Save;
			treasureCollected = info.TreasureCollected;

		}
	}
	// All of this is for testing
	void OnGUI()
	{
		GUI.TextField (new Rect (100, 300, 150, 20), "Current Level: " + Application.loadedLevel);
		if (GUI.Button(new Rect(10,10,100,20), "Next Level"))
		{
			if (Application.loadedLevel == 20) // total count of menus and levels
				return;
			else
			{
				float fadeTime = manager.GetComponent<Fade>().BeginFade(1);
				Invoke("NextLevel", fadeTime);
			}
		}		
		if (GUI.Button(new Rect(10,50,100,20), "Last Level"))
		{
			if (Application.loadedLevel == 0) // there is no negetive scene
				return;
			else
			{
				float fadeTime = manager.GetComponent<Fade>().BeginFade(1);
				Invoke("PrevLevel", fadeTime);
			}		
		}
	}

	
	void NextLevel()
	{
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	void PrevLevel()
	{
		Application.LoadLevel(Application.loadedLevel - 1);
	}
}

// class for saving data
[Serializable]
class GameInfo
{
	int lives, keys, save;

	public int Save {
		get {
			return save;
		}
		set {
			save = value;
		}
	}

	public int Keys {
		get {
			return keys;
		}
		set {
			keys = value;
		}
	}

	public int Lives {
		get {
			return lives;
		}
		set {
			lives = value;
		}
	}

	float gameTime;

	public float GameTime {
		get {
			return gameTime;
		}
		set {
			gameTime = value;
		}
	}

	bool hardModeOn, timeAttackOn;
	public bool HardModeOn {
		get {
			return hardModeOn;
		}
		set {
			hardModeOn = value;
		}
	}


	public bool TimeAttackOn {
		get {
			return timeAttackOn;
		}
		set {
			timeAttackOn = value;
		}
	}
	bool[] treasureCollected;

	public bool[] TreasureCollected {
		get {
			return treasureCollected;
		}
		set {
			treasureCollected = value;
		}
	}
}