using UnityEngine;
using System.Collections;

public class SaveLoadScript : MonoBehaviour
{
    bool Play = false;
    int LoadSelected = 1;
    float ModePoPTimer = 0.0f;
    public GUIStyle style;		// allow for large font for timer
    bool HardModeClicked = false, TimeAttackModeClicked = false;

    void FixedUpdate()
    {
        if (Play)
        {
            Application.LoadLevel(1);
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

		GUI.TextField(new Rect(Screen.width - 105, 10, 100, 20), "time: " + GameManager.manager.gameTime.ToString());
		GUI.TextField(new Rect(Screen.width - 105, 40, 100, 20), "keys: " + GameManager.manager.keys.ToString());
        GUI.TextField(new Rect(Screen.width - 105, 70, 100, 20), "lives: " + GameManager.manager.lives.ToString());
        GUI.TextField(new Rect(Screen.width - 105, 100, 100, 20), "Treasure: " + TreasureHolder.ToString());
        GUI.TextField(new Rect(Screen.width - 105, 130, 100, 20), "Secrets: " + STreasureHolder.ToString());
        GUI.TextField(new Rect(Screen.width - 105, 160, 100, 20), "Levels: " + Levelholder.ToString());


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


        //Start of TimeAttack/Hardmode buttons
        GUI.Box(new Rect((Screen.width / 2) + 105, 100, 100,80), "Play Modes");

        // Hard Mode button
        if (GUI.Button (new Rect((Screen.width / 2) + 105, 125, 100, 25), "Hard Mode"))
        {
            GameManager.manager.hardModeOn = !GameManager.manager.hardModeOn;
            if(!GameManager.manager.hardModeOn)
            { HardModeClicked = true; }
            ModePoPTimer = 5.0f;

        }

        // Time Attack Button
        if(GUI.Button (new Rect((Screen.width / 2) + 105, 155, 100, 25), "Time Attack"))
        {
            GameManager.manager.timeAttackOn = !GameManager.manager.timeAttackOn;
            if (!GameManager.manager.timeAttackOn)
            { TimeAttackModeClicked = true; }
            ModePoPTimer = 5.0f;
        }

        //Time Attack / HardMode headsup display.
        if (GameManager.manager.hardModeOn && ModePoPTimer > 0.0f)
        {
                GUI.Label(new Rect((Screen.width / 2) - 128, Screen.height - 64, 128, 64), "Hard Mode On", style);
                ModePoPTimer -= Time.deltaTime;
        }
        else if (HardModeClicked && ModePoPTimer > 0.0f)
        {
            GUI.Label(new Rect((Screen.width / 2) - 128, Screen.height - 64, 128, 64), "Hard Mode Off", style);
            ModePoPTimer -= Time.deltaTime;
        }
        else
        {
            HardModeClicked = false;
        }

        if (GameManager.manager.timeAttackOn && ModePoPTimer > 0.0f)
        {
            GUI.Label(new Rect((Screen.width / 2) + 105, Screen.height - 64, 128, 64), "Time Attack On", style);
            ModePoPTimer -= Time.deltaTime;
        }
            else if (TimeAttackModeClicked && ModePoPTimer > 0.0f)
        {
            GUI.Label(new Rect((Screen.width / 2) + 105, Screen.height - 64, 128, 64), "Time Attack Off", style);
            ModePoPTimer -= Time.deltaTime;
            TimeAttackModeClicked = true;
        }
        else
        {
            TimeAttackModeClicked = false;
        }
        //End of Timeattack/Hardmode

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
            LoadSelected = 1;
		}
        if (GUI.Button(new Rect((Screen.width / 2) - 250, 100, 100, 25), "Play "+LoadSelected.ToString())) //When play is clicked.
        {
            Play = true;
            GameManager.manager.save = LoadSelected;
        }
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 200, 100, 25), "Load 2"))
		{
			GameManager.manager.Load (2);
            LoadSelected = 2;
		}
		if(GUI.Button (new Rect((Screen.width / 2) - 105, 300, 100, 25), "Load 3"))
		{
			GameManager.manager.Load (3);
            LoadSelected = 3;
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
