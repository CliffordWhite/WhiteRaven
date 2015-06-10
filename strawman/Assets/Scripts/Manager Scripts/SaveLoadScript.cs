using UnityEngine;
using System.Collections;

public class SaveLoadScript : MonoBehaviour
{
    bool Play = false;
    int LoadSelected = 1;
    public GUIStyle style;		// allow for large font for timer
    string HardModeString = "Off";
    string AttackTimeString = "Off";


    void FixedUpdate()
    {
        if (Play)
        {
            Application.LoadLevel(1);
        }

        if(GameManager.manager.timeAttackOn)
        {
            AttackTimeString = "On";
        }
        else
        {
            AttackTimeString = "Off";
        }

        if(GameManager.manager.hardModeOn)
        {
            HardModeString = "On";
        }
        else
        {
            HardModeString = "Off";
        }
    }

    void OnGUI()
	{
        int Levelholder = 0;
        int TreasureHolder = 0;
        int STreasureHolder = 0;
        for (int i = 0; i < 15; i++)
        {
            if (GameManager.manager.levelCompleted[i] == true)
            {
                Levelholder++;
            }
        }
        for (int i = 0; i < 15; i++)
        {
            if (GameManager.manager.treasureCollected[i] == true)
            {
                TreasureHolder++;
            }
        }
        for (int i = 0; i < 15; i++)
        {
            if (GameManager.manager.secrettreasureCollected[i] == true)
            {
                STreasureHolder++;
            }
        }

		GUI.TextField(new Rect(Screen.width - 105, 20, 100, 20), "time: " + GameManager.manager.gameTime.ToString());
		GUI.TextField(new Rect(Screen.width - 105, 50, 100, 20), "keys: " + GameManager.manager.keys.ToString());
        GUI.TextField(new Rect(Screen.width - 105, 80, 100, 20), "lives: " + GameManager.manager.lives.ToString());
        GUI.TextField(new Rect(Screen.width - 105, 110, 100, 20), "Treasure: " + TreasureHolder.ToString());
        GUI.TextField(new Rect(Screen.width - 105, 140, 100, 20), "Secrets: " + STreasureHolder.ToString());
        GUI.TextField(new Rect(Screen.width - 105, 170, 100, 20), "Levels: " + Levelholder.ToString());


        //if(GUI.Button (new Rect((Screen.width / 2) + 105, 100, 100, 25), "Save 1"))
        //{
        //    GameManager.manager.save = 1;
        //    GameManager.manager.Save();
        //}
        //if(GUI.Button (new Rect((Screen.width / 2) + 105, 200, 100, 25), "Save 2"))
        //{
        //    GameManager.manager.save = 2;
        //    GameManager.manager.Save();
        //}
        //if(GUI.Button (new Rect((Screen.width / 2) + 105, 300, 100, 25), "Save 3"))
        //{
        //    GameManager.manager.save = 3;
        //    GameManager.manager.Save();
        //}

		if(GUI.Button (new Rect((Screen.width / 2) - 105, 150, 100, 25), "Erase 1"))
		{
			GameManager.manager.save = 1;
            if (!GameManager.manager.webMode)
                GameManager.manager.EraseFile();
            else
                GameManager.manager.PlayerPrefsErase();
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 250, 100, 25), "Erase 2"))
		{
			GameManager.manager.save = 2;
            if (!GameManager.manager.webMode)
                GameManager.manager.EraseFile();
            else
                GameManager.manager.PlayerPrefsErase();
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 350, 100, 25), "Erase 3"))
		{
			GameManager.manager.save = 3;
            if (!GameManager.manager.webMode)
                GameManager.manager.EraseFile();
            else
                GameManager.manager.PlayerPrefsErase();
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 100, 100, 25), "Load 1"))
		{
            LoadSelected = 1;
            GameManager.manager.save = 1;
            if (!GameManager.manager.webMode)
                GameManager.manager.Load(1);
            else
                GameManager.manager.PlayerPrefsLoad();

		}
        if (GUI.Button(new Rect((Screen.width / 2) - 250, 100, 100, 25), "Play "+LoadSelected.ToString())) //When play is clicked.
        {
            Play = true;
            GameManager.manager.save = LoadSelected;
        }
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 200, 100, 25), "Load 2"))
		{
            GameManager.manager.save = 2;
            if (!GameManager.manager.webMode)
                GameManager.manager.Load(2);
            else
                GameManager.manager.PlayerPrefsLoad();
            LoadSelected = 2;
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 300, 100, 25), "Load 3"))
		{
            GameManager.manager.save = 3;
            if (!GameManager.manager.webMode)
                GameManager.manager.Load(3);
            else
                GameManager.manager.PlayerPrefsLoad();
            LoadSelected = 3;
		}


        //Start of TimeAttack/Hardmode buttons
        if (GameManager.manager.levelUnlocked[1] == false)
        {
        GUI.Box(new Rect((Screen.width / 2) + 105, 100, 110, 80), "Play Modes");

        // Hard Mode button
        if (GUI.Button(new Rect((Screen.width / 2) + 105, 125, 110, 25), "Hard Mode "+ HardModeString.ToString()))
        {
            GameManager.manager.hardModeOn = !GameManager.manager.hardModeOn;
            if (!GameManager.manager.hardModeOn)
                { HardModeString = "Off"; }
            else
                { HardModeString = "On"; }
        }
        // Time Attack Button
        if (GUI.Button(new Rect((Screen.width / 2) + 105, 155, 110, 25), "Time Attack "+AttackTimeString.ToString()))
        {
            GameManager.manager.timeAttackOn = !GameManager.manager.timeAttackOn;
            if (!GameManager.manager.timeAttackOn)
            { AttackTimeString = "Off";}
            else
            { AttackTimeString = "On";}
        }
        //End of Timeattack/Hardmode
	    }
    }

    public int loadSelected
    {
        get
        {
            return LoadSelected;
        }
    }
}
