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
    public bool flyMode;                    //IF the cheat Fly is on, will stay on.
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

        for (int i = 0; i < 15; i++)
        {
            if(treasureCollected[i] == true)
                PlayerPrefs.SetInt(SaveFile + " TreasureCollected" + i, 1);
            else
                PlayerPrefs.SetInt(SaveFile + " TreasureCollected" + i, 0);

            if (secrettreasureCollected[i] == true)
                PlayerPrefs.SetInt(SaveFile + " SecretTreasureCollected" + i, 1);
            else
                PlayerPrefs.SetInt(SaveFile + " SecretTreasureCollected" + i, 0);

            if (levelCompleted[i] == true)
                PlayerPrefs.SetInt(SaveFile + " LevelCompleted" + i, 1);
            else
                PlayerPrefs.SetInt(SaveFile + " LevelCompleted" + i, 0);

            if (levelUnlocked[i] == true)
                PlayerPrefs.SetInt(SaveFile + " LevelUnlocked" + i, 1);
            else
                PlayerPrefs.SetInt(SaveFile + " LevelUnlocked" + i, 0);

        }
        if (flyMode)
            PlayerPrefs.SetInt(SaveFile + " FlyMode", 1);
        else
            PlayerPrefs.SetInt(SaveFile + " FlyMode", 0);

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
        bool[] ClearSaves1 = new bool[15];
        bool[] ClearSaves2 = new bool[15];
        bool[] ClearSaves3 = new bool[15];
        bool[] ClearSaves4 = new bool[15];

         for (int i = 0; i < 15; i++)
        {
            ClearSaves1[i] = false;
            ClearSaves2[i] = false;
            ClearSaves3[i] = false;
            ClearSaves4[i] = false;
        }
         info.TreasureCollected = ClearSaves1;
         info.SecrettreasureCollected = ClearSaves2;
         info.LevelCompleted = ClearSaves3;
         info.LevelUnlocked = ClearSaves4;
         info.LevelUnlocked[0] = true;
         info.FlyMode = flyMode;
        
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
        for (int i = 0; i < 15; i++)
        {
            PlayerPrefs.DeleteKey(SaveFile + " TreasureCollected" + i);
            PlayerPrefs.DeleteKey(SaveFile + " SecretTreasureCollected" + i);
            PlayerPrefs.DeleteKey(SaveFile + " LevelCompleted" + i);
            PlayerPrefs.DeleteKey(SaveFile + " LevelUnlocked" + i);
        }
        PlayerPrefs.DeleteKey(SaveFile + " FlyMode");
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

        for (int i = 0; i < 15; i++)
        {
            if (PlayerPrefs.GetInt(SaveFile + " TreasureCollected" + i) == 1)
                treasureCollected[i] = true;
            else
                treasureCollected[i] = false;

            if (PlayerPrefs.GetInt(SaveFile + " SecretTreasureCollected" + i) == 1)
                secrettreasureCollected[i] = true;
            else
                secrettreasureCollected[i] = false;

            if (PlayerPrefs.GetInt(SaveFile + " LevelCompleted" + i) == 1)
                levelCompleted[i] = true;
            else
                levelCompleted[i] = false;

            if (PlayerPrefs.GetInt(SaveFile + " LevelUnlocked" + i) == 1)
                levelUnlocked[i] = true;
            else
                levelUnlocked[i] = false;

            if (i == 0)
                levelUnlocked[0] = true;


        }
        if (PlayerPrefs.GetInt(SaveFile + " FlyMode") == 1)
            flyMode = true;
        else
            flyMode = false;

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