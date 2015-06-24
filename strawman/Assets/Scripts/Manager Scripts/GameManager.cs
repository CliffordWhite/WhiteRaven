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
    //cheats begin
    public bool flyMode;                    //IF the cheat Fly is on, will stay on.
    public bool godMode;
    public bool marcoPoloMode;
    //cheats end
	public float gameTime; 					// elapsed time for time attack
	public bool hardModeOn, timeAttackOn; 	// flags for game modes
	public bool[] treasureCollected;		// flags for collected treasures
    public bool[] secrettreasureCollected;		//flags for collected treasures
    public bool[] levelCompleted;           //Flags for Levels Completed
    public bool[] levelUnlocked;            //Flags Level unlocked
    public int save; 						// which save slot
	public string saveName;					// the name of the save
	public float MusicVolume;
	public float SFXVolume;
	public bool isFullscreen;
	public float keyShowTime;
	public bool endGameTransition;
	public bool isExiting;


	//acheive info
	public bool[] achieveList;

	//treasure checks
	public bool DoorUnlocked;
	public bool secretGot;
    //webplayer
    string SaveFile;
    public bool webMode;


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
        webMode = false;
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
        info.LevelUnlocked = levelUnlocked;
        info.FlyMode = flyMode;
        info.GodMode = godMode;
        info.MarcoPoloMode = marcoPoloMode;

		bf.Serialize (file, info);
		file.Close ();
	}
   
    public void PlayerPrefSave()
    {

        SaveFile = "SaveFile " + save;
        PlayerPrefs.SetInt(SaveFile, save);
        PlayerPrefs.SetInt(SaveFile +" Keys", keys);
        PlayerPrefs.SetInt(SaveFile +" Lives", lives);
        PlayerPrefs.SetFloat(SaveFile +" GameTime", gameTime);

        if(hardModeOn)
            PlayerPrefs.SetInt(SaveFile + " HardModeOn", 1);
        else
            PlayerPrefs.SetInt(SaveFile + " HardModeOn", 0);

        if (timeAttackOn)
            PlayerPrefs.SetInt(SaveFile + " TimeAttackOn", 1);
        else
            PlayerPrefs.SetInt(SaveFile + " TimeAttackOn", 0);

        for (int i = 0; i < treasureCollected.Length; i++)
        {
            if(treasureCollected[i] == true)
                PlayerPrefs.SetInt(SaveFile + " TreasureCollected" + i, 1);
            else
                PlayerPrefs.SetInt(SaveFile + " TreasureCollected" + i, 0);
		}
		for (int i = 0; i < secrettreasureCollected.Length; i++)
		{
			if (secrettreasureCollected[i] == true)
                PlayerPrefs.SetInt(SaveFile + " SecretTreasureCollected" + i, 1);
            else
                PlayerPrefs.SetInt(SaveFile + " SecretTreasureCollected" + i, 0);
		}
		for (int i = 0; i < levelCompleted.Length; i++)
		{
			if (levelCompleted[i] == true)
               PlayerPrefs.SetInt(SaveFile + " LevelCompleted" + i, 1);
           else
               PlayerPrefs.SetInt(SaveFile + " LevelCompleted" + i, 0);
		}
		for (int i = 0; i < levelUnlocked.Length; i++)
		{
			if (levelUnlocked[i] == true)
               PlayerPrefs.SetInt(SaveFile + " LevelUnlocked" + i, 1);
           else
               PlayerPrefs.SetInt(SaveFile + " LevelUnlocked" + i, 0);
        }
        if (flyMode)
            PlayerPrefs.SetInt(SaveFile + " FlyMode", 1);
        else
            PlayerPrefs.SetInt(SaveFile + " FlyMode", 0);
        if (godMode)
            PlayerPrefs.SetInt(SaveFile + " GodMode", 1);
        else
            PlayerPrefs.SetInt(SaveFile + " GodMode", 0);
        if (marcoPoloMode)
            PlayerPrefs.SetInt(SaveFile + " MarcoPoloMode", 1);
        else
            PlayerPrefs.SetInt(SaveFile + " MarcoPoloMode", 0);
            PlayerPrefs.Save();
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
		//////////////////////////////////////////////////////////////
		// KNOWN BUG 3
		// index out of range when erase happens since treasure pause 
		// menu is trying to access index position 15, which requires 
		// size 16. Making this code more dynamic to allow for arrays 
		// to be any size and more readable
		//////////////////////////////////////////////////////////////
		
		bool[] treasure = new bool[treasureCollected.Length];
        bool[] secrets = new bool[secrettreasureCollected.Length];
		bool[] completed = new bool[levelCompleted.Length];
        bool[] unlocked = new bool[levelUnlocked.Length];

		for (int i = 0; i < treasure.Length; i++)
			treasure[i] = false;
		for (int i = 0; i < secrets.Length; i++)
			secrets[i] = false;
		for (int i = 0; i < completed.Length; i++)
			completed[i] = false;
		for (int i = 0; i < unlocked.Length; i++)
			unlocked[i] = false;

		info.TreasureCollected = treasure;
        info.SecrettreasureCollected = secrets;
        info.LevelCompleted = completed;
        info.LevelUnlocked = unlocked;
		//////////////////////////////////////////////////////////////
		// END OF KNOWN BUG 3
		//////////////////////////////////////////////////////////////

        info.LevelUnlocked[0] = true;
        info.FlyMode = false;
        info.GodMode = false;
        info.MarcoPoloMode = false;
        
		bf.Serialize (file, info);
		file.Close ();
        Load(save);
	}
    public void PlayerPrefsErase()
    {
        SaveFile = "SaveFile " + save;
        PlayerPrefs.DeleteKey(SaveFile);
        PlayerPrefs.DeleteKey(SaveFile + " Keys");
        PlayerPrefs.DeleteKey(SaveFile + " Lives");
        PlayerPrefs.DeleteKey(SaveFile + " GameTime");
        PlayerPrefs.DeleteKey(SaveFile + " HardModeOn");
        PlayerPrefs.DeleteKey(SaveFile + " TimeAttackOn");
		//////////////////////////////////////////////////////////////
		// KNOWN BUG 3
		//////////////////////////////////////////////////////////////
		
        for (int i = 0; i < treasureCollected.Length; i++)
            PlayerPrefs.DeleteKey(SaveFile + " TreasureCollected" + i);
        for (int i = 0; i < secrettreasureCollected.Length; i++)
			PlayerPrefs.DeleteKey(SaveFile + " SecretTreasureCollected" + i);
        for (int i = 0; i < levelCompleted.Length; i++)
            PlayerPrefs.DeleteKey(SaveFile + " LevelCompleted" + i);
        for (int i = 0; i < levelUnlocked.Length; i++)
            PlayerPrefs.DeleteKey(SaveFile + " LevelUnlocked" + i);
		//////////////////////////////////////////////////////////////
		// END KNOWN BUG 3
		//////////////////////////////////////////////////////////////
		
        PlayerPrefs.DeleteKey(SaveFile + " FlyMode");
        PlayerPrefs.DeleteKey(SaveFile + " GodMode");
        PlayerPrefs.DeleteKey(SaveFile + " MarcoPoloMode");

        PlayerPrefsLoad();
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
            levelUnlocked = info.LevelUnlocked;
            flyMode = info.FlyMode;
            godMode = info.GodMode;
            marcoPoloMode = info.MarcoPoloMode;

		}
	}

	public void PlayerPrefsLoad()
    {
        SaveFile = "SaveFile " + save;

        keys = PlayerPrefs.GetInt(SaveFile + " Keys");
        lives = PlayerPrefs.GetInt(SaveFile + " Lives");
        gameTime = PlayerPrefs.GetFloat(SaveFile + " GameTime");

        if (PlayerPrefs.GetInt(SaveFile + " HardModeOn") == 1)
            hardModeOn = true;
        else
            hardModeOn = false;

        if (PlayerPrefs.GetInt(SaveFile + " TimeAttackOn") == 1)
            timeAttackOn = true;
        else
            timeAttackOn = false;
		//////////////////////////////////////////////////////////////
		// KNOWN BUG 3
		//////////////////////////////////////////////////////////////
		
		for (int i = 0; i < treasureCollected.Length; i++)
        {
            if (PlayerPrefs.GetInt(SaveFile + " TreasureCollected" + i) == 1)
                treasureCollected[i] = true;
            else
                treasureCollected[i] = false;
		}
		for (int i = 0; i < secrettreasureCollected.Length; i++)
		{
            if (PlayerPrefs.GetInt(SaveFile + " SecretTreasureCollected" + i) == 1)
                secrettreasureCollected[i] = true;
            else
                secrettreasureCollected[i] = false;
		}
		for (int i = 0; i < levelCompleted.Length; i++)
		{
            if (PlayerPrefs.GetInt(SaveFile + " LevelCompleted" + i) == 1)
                levelCompleted[i] = true;
            else
                levelCompleted[i] = false;
		}
		for (int i = 0; i < levelUnlocked.Length; i++)
		{
            if (PlayerPrefs.GetInt(SaveFile + " LevelUnlocked" + i) == 1)
                levelUnlocked[i] = true;
            else
                levelUnlocked[i] = false;

            if (i == 0)
                levelUnlocked[0] = true;


        }

		//////////////////////////////////////////////////////////////
		// END KNOWN BUG 3
		//////////////////////////////////////////////////////////////
		
        if (PlayerPrefs.GetInt(SaveFile + " FlyMode") == 1)
            flyMode = true;
        else
            flyMode = false;
        if (PlayerPrefs.GetInt(SaveFile + " GodMode") == 1)
            godMode = true;
        else
            godMode = false;
        if (PlayerPrefs.GetInt(SaveFile + " MarcoPoloMode") == 1)
            marcoPoloMode = true;
        else
            marcoPoloMode = false;
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
        Debug.Log("GameInfo manager NextLevel()");
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

    bool flyMode;
    public bool FlyMode
    {
        get
        {
            return flyMode;
        }
        set
        {
            flyMode = value;
        }
    }
    bool godMode;
    public bool GodMode
    {
        get
        {
            return godMode;
        }
        set
        {
            godMode = value;
        }
    }
    bool marcoPoloMode;
    public bool MarcoPoloMode
    {
        get
        {
            return marcoPoloMode;
        }
        set
        {
            marcoPoloMode = value;
        }
    }
    bool webMode;
    public bool WebMode
    {
        get
        {
            return webMode;
        }
        set
        {
            webMode = value;
        }
    }
}
