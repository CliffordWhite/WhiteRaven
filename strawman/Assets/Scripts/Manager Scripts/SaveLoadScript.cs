using UnityEngine;
using System.Collections;

public class SaveLoadScript : MonoBehaviour
{
    bool Play = false;
    int LoadSelected = 1;
//    public GUIStyle style;		// allow for large font for timer
    string HardModeString = "Off";
    string AttackTimeString = "Off";
	public GUISkin skin;		// for using custom font

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
		GUI.skin = skin;
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
        GUI.Box(new Rect((Screen.width / 2) - 55, 250, 100, 175), "Game Info");
        GUI.TextField(new Rect((Screen.width / 2) - 55, 280, 100, 25), "Time: " + GameManager.manager.gameTime.ToString());
        GUI.TextField(new Rect((Screen.width / 2) - 55, 310, 100, 25), "Keys: " + GameManager.manager.keys.ToString());
        GUI.TextField(new Rect((Screen.width / 2) - 55, 340, 100, 25), "Lives: " + GameManager.manager.lives.ToString());
        GUI.TextField(new Rect((Screen.width / 2) - 55, 370, 100, 25), "Treasure: " + TreasureHolder.ToString());
        GUI.TextField(new Rect((Screen.width / 2) - 55, 400, 100, 25), "Secrets: " + STreasureHolder.ToString());
        GUI.TextField(new Rect((Screen.width / 2) - 55, 430, 100, 25), "Levels: " + Levelholder.ToString());


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

        if (GUI.Button(new Rect(0, Screen.height - 100, 110, 25), "Erase " + LoadSelected.ToString()))
		{
            GameManager.manager.save = LoadSelected;
            if (!GameManager.manager.webMode)
                GameManager.manager.EraseFile();
            else
                GameManager.manager.PlayerPrefsErase();
		}
        //if (GUI.Button(new Rect((Screen.width / 2) - 250, 250, 100, 25), "Erase 2"))
        //{
        //    GameManager.manager.save = 2;
        //    if (!GameManager.manager.webMode)
        //        GameManager.manager.EraseFile();
        //    else
        //        GameManager.manager.PlayerPrefsErase();
        //}
        //if (GUI.Button(new Rect((Screen.width / 2) - 250, 350, 100, 25), "Erase 3"))
        //{
        //    GameManager.manager.save = 3;
        //    if (!GameManager.manager.webMode)
        //        GameManager.manager.EraseFile();
        //    else
        //        GameManager.manager.PlayerPrefsErase();
        //}
		if(GUI.Button (new Rect(0, 100, 100, 25), "Load 1"))
		{
            LoadSelected = 1;
            GameManager.manager.save = 1;
            if (!GameManager.manager.webMode)
                GameManager.manager.Load(1);
            else
                GameManager.manager.PlayerPrefsLoad();

		}

        if (GUI.Button(new Rect((Screen.width / 2) - 55, 200, 100, 25), "Play "+LoadSelected.ToString())) //When play is clicked.
        {
            Play = true;
            GameManager.manager.save = LoadSelected;
        }
        if (GUI.Button(new Rect((Screen.width / 2) - 55, 100, 100, 25), "Load 2"))
		{
            GameManager.manager.save = 2;
            if (!GameManager.manager.webMode)
                GameManager.manager.Load(2);
            else
                GameManager.manager.PlayerPrefsLoad();
            LoadSelected = 2;
		}
        if (GUI.Button(new Rect((Screen.width) - 105, 100, 100, 25), "Load 3"))
		{
            GameManager.manager.save = 3;
            if (!GameManager.manager.webMode)
                GameManager.manager.Load(3);
            else
                GameManager.manager.PlayerPrefsLoad();
            LoadSelected = 3;
		}

        //Back to Menu button
        if (GUI.Button(new Rect(Screen.width - 105, Screen.height - 100, 100, 25), "Main Menu"))
        {
            Application.LoadLevel(0);
        }

        //Start of TimeAttack/Hardmode buttons
        if (GameManager.manager.levelUnlocked[1] == false)
        {
            GUI.Box(new Rect((Screen.width) - 105, 250, 100, 25), "Play Modes");

        // Hard Mode button
            if (GUI.Button(new Rect((Screen.width) - 105, 280, 100, 25), "Hard Mode " + HardModeString.ToString()))
        {
            GameManager.manager.hardModeOn = !GameManager.manager.hardModeOn;
            if (!GameManager.manager.hardModeOn)
                { HardModeString = "Off"; }
            else
                { HardModeString = "On"; }
        }
        // Time Attack Button
            if (GUI.Button(new Rect((Screen.width) - 105, 310, 100, 25), "Time Attack " + AttackTimeString.ToString()))
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
