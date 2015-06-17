using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureList : MonoBehaviour 
{
	[Header ("Set each level's treasure sprite here")]
	public Sprite closedChest;
	public Sprite tutorialLevel;
	public Sprite level4;
	public Sprite level5a;
	public Sprite level5b;
	public Sprite level6a;
	public Sprite level6b;
	public Sprite level6c;
	public Sprite level7a;
	public Sprite level7b;
	public Sprite level7c;
	public Sprite level8a;
	public Sprite level8b;
	public Sprite level9a;
	public Sprite level9b;
	public List<Sprite> treasures;
	
	[Header ("Set bonus treasure sprite here")]
	public Sprite lvl2;
	public Sprite lvl4;
	public Sprite lvl5;
	public Sprite lvl6;
	public Sprite lvl7;
	public Sprite lvl8;
	public Sprite lvl9;
	public List<Sprite> secrets;

	void Start()
	{
		treasures.Add(level4);
		treasures.Add(level5a);
		treasures.Add(level5b);
		treasures.Add(level6a);
		treasures.Add(level6b);
		treasures.Add(level6c);
		treasures.Add(level7a);
		treasures.Add(level7b);
		treasures.Add(level7c);
		treasures.Add(level8a);
		treasures.Add(level8b);
		treasures.Add(level9a);
		treasures.Add(level9b);
		
		secrets.Add(lvl2);
		secrets.Add(lvl4);
		secrets.Add(lvl5);
		secrets.Add(lvl6);
		secrets.Add(lvl7);
		secrets.Add(lvl8);
		secrets.Add(lvl9);
	}
}
