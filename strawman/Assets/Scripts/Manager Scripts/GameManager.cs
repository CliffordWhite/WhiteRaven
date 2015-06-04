using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//Our singleton that takes care of game data
public class GameManager : MonoBehaviour 
{
	public static GameManager manager; 		// allow singleton access
	public static bool paused = false; 		// bool to tell other scripts the game is paused

	public int lives, keys; 				// number of lives for hard, number of keys held
	public float gameTime; 					// elapsed time for time attack
	public bool hardModeOn, timeAttackOn; 	// flags for game modes
	public bool[] treasureCollected;		// flags for collected treasures
    public bool[] secrettreasureCollected;		// flags for collected treasures
    public bool[] levelCompleted;           //Flags for Levels Completed
    public bool[] levelUnlocked;            //Flags Level unlocked
    public int save; 						// which save slot
	public string saveName;					// the name of the save
	public float MusicVolume;
	public float SFXVolume;
	public bool isFullscreen;
	public float keyShowTime;

	//acheive info
	public bool[] achieveList;
	public float achievePopTime;
	public string achievePopString;

	//treasure checks
	public bool DoorUnlocked;
	public bool secretGot;

	void Awake () 
	{
        //levelUnlocked[0].Equals(true);
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

	void Update(){
		if (!achieveList [7] && GameManager.manager.levelCompleted [0]) {
			achieveList [7] = true;
			achievePopString = "Test Earned";
			achievePopTime = 5.0f;
		}

		if (!achieveList [0] && GameManager.manager.levelCompleted [14]) {
			achieveList [0] = true;
			achievePopString = "Beat the Game";
			achievePopTime = 5.0f;
		}

		if (!achieveList [1]) {
			bool earn = true;
			for (int i = 0; i < GameManager.manager.levelCompleted.Length; i++) {
				if (!GameManager.manager.levelCompleted[i]) {
					earn = false;
					break;
				}
			}
			if (earn) {
				achieveList [1] = true;
				achievePopString = "Complete All Levels";
				achievePopTime = 5.0f;
			}
		}

		if (!achieveList [2] && GameManager.manager.levelCompleted [14] && GameManager.manager.hardModeOn ) {
			achieveList [2] = true;
			achievePopString = "Beat the Game (Hard)";
			achievePopTime = 5.0f;
		}

		if (!achieveList [3] && GameManager.manager.hardModeOn) {
			bool earn = true;
			for (int i = 0; i < GameManager.manager.levelCompleted.Length; i++) {
				if (!GameManager.manager.levelCompleted[i]) {
					earn = false;
					break;
				}
			}
			if (earn) {
				achieveList [3] = true;
				achievePopString = "Complete All Levels (Hard)";
				achievePopTime = 5.0f;
			}
		}

		if (!achieveList [4] && GameManager.manager.levelCompleted [14] && GameManager.manager.timeAttackOn && GameManager.manager.gameTime < 600.0f) {
			achieveList [4] = true;
			achievePopString = "Beat the Game under 10 min";
			achievePopTime = 5.0f;
		}

		if (!achieveList [5] && GameManager.manager.timeAttackOn && GameManager.manager.gameTime < 1800.0f) {
			bool earn = true;
			for (int i = 0; i < GameManager.manager.levelCompleted.Length; i++) {
				if (!GameManager.manager.levelCompleted[i]) {
					earn = false;
					break;
				}
			}
			if (earn) {
				achieveList [5] = true;
				achievePopString = "Complete All Levels (Hard)";
				achievePopTime = 5.0f;
			}
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
        info.SecrettreasureCollected = secrettreasureCollected;
        info.LevelCompleted = levelCompleted;

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
        bool[] ClearSaves = new bool[15];
         for (int i = 0; i < 15; i++)
        {
           ClearSaves[i] = false;
        }
         info.TreasureCollected = ClearSaves;
         info.SecrettreasureCollected = ClearSaves;
         info.LevelCompleted = ClearSaves;
        
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
            secrettreasureCollected = info.SecrettreasureCollected;
            levelCompleted = info.LevelCompleted;
		}
	}
	// All of this is for testing, preserved for testing
	//void OnGUI()
	//{
	//	GUI.TextField (new Rect (100, 300, 150, 20), "Current Level: " + Application.loadedLevel);
	//	if (GUI.Button(new Rect(10,10,100,20), "Next Level"))
	//	{
	//		if (Application.loadedLevel == 20) // total count of menus and levels
	//			return;
	//		else
	//		{
	//			float fadeTime = manager.GetComponent<Fade>().BeginFade(1);
	//			Invoke("NextLevel", fadeTime);
	//		}
	//	}		
	//	if (GUI.Button(new Rect(10,50,100,20), "Last Level"))
	//	{
	//		if (Application.loadedLevel == 0) // there is no negetive scene
	//			return;
	//		else
	//		{
	//			float fadeTime = manager.GetComponent<Fade>().BeginFade(1);
	//			Invoke("PrevLevel", fadeTime);
	//		}		
	//	}
	//}

	
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
    bool[] secrettreasureCollected;
    public bool[] SecrettreasureCollected
    {
        get
        {
            return secrettreasureCollected;
        }
        set
        {
            secrettreasureCollected = value;
        }
    }

    bool[] levelCompleted;
    public bool[] LevelCompleted
    {
        get
        {
            return levelCompleted;
        }
        set
        {
            levelCompleted = value;
        }
    }

    bool[] levelUnlocked;
    public bool[] LevelUnlocked
    {
        get
        {
            return levelUnlocked;
        }
        set
        {
            levelUnlocked = value;
        }
    }

}