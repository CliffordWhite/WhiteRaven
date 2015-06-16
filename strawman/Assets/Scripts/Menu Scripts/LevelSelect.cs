using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

    public int SceneNumber;
    public bool LevelBeat;
    public bool LevelUnlock;
	public string levelName;		// for determining which sprite to use
	public GUISkin skin;			// for using custom font
	
    // Use this for initialization
	void Start () {
        LevelUnlock = false;
        LevelBeat = false;
        
        for (int i = 0; i < 15; i++)
        {
            if (GameManager.manager.levelCompleted[i] && i == (SceneNumber - 6))
            { 
            LevelBeat = true;
            LevelUnlock = true;
            }
        }
        for (int i = 0; i < 15; i++)
        {
            if (GameManager.manager.levelUnlocked[i] && i == (SceneNumber - 6))
            {
                LevelUnlock = true;
            }
        }
        if(!LevelUnlock)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, .5F);
        }

		// use treasure sprite if level was complete
		if (LevelBeat)
		{
			if (levelName == "tutorial")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().tutorialLevel;
			else if (levelName == "4")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level4;
			else if (levelName == "5a")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level5a;
			else if (levelName == "5b")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level5b;
			else if (levelName == "6a")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level6a;
			else if (levelName == "6b")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level6b;
			else if (levelName == "6c")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level6c;
			else if (levelName == "7a")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level7a;
			else if (levelName == "7b")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level7b;
			else if (levelName == "7c")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level7c;
			else if (levelName == "8a")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level8a;
			else if (levelName == "8b")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level8b;
			else if (levelName == "9")
				gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.manager.GetComponent<TreasureList>().level9b;
		}
	}
	
    void OnMouseOver()
	{

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {

            if(LevelUnlock)
            {
                LoadScene(SceneNumber);
            }
            //else
            //play error noise
        }
	}
    
    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
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

        GUI.Label(new Rect(50, 100, 200,100),Levelholder.ToString() + "/15 Levels Completed");
        GUI.Label(new Rect(50, 150, 200, 100), TreasureHolder.ToString() + "/12 Treasure Collected");
        GUI.Label(new Rect(50, 200, 200, 100), STreasureHolder.ToString() + "/7 Secret Treasure Collected");


        if (GUI.Button(new Rect(Screen.width - 105, 155, 110, 25), "Main Menu"))
        {
            Application.LoadLevel(0);
        }


    }
}
