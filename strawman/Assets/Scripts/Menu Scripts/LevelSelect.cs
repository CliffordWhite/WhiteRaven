using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

    public int SceneNumber;
    public bool LevelBeat;
    public bool LevelUnlock;


	
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
        int Levelholder = 0;
        int TreasureHolder = 0;
         for (int i = 0; i < 15; i++)
        {
            if (GameManager.manager.levelCompleted[i] == true)
            { 
                Levelholder++;
            }
            
        }

         for (int i = 0; i < 12; i++)
         {
             if (GameManager.manager.treasureCollected[i] == true)
             {
                 TreasureHolder++;
             }
         }

        GUI.Label(new Rect(100,100,100,100),Levelholder.ToString() + "/15 Levels Completed");
        GUI.Label(new Rect(100, 150, 100, 100), TreasureHolder.ToString() + "/12 Treasure Collected");

    }
}
