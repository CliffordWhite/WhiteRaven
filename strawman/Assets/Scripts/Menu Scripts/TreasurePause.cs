using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TreasurePause : MonoBehaviour 
{
	[Header ("Set each level's treasure icon here")]
	public GameObject level4;
	public GameObject level5a;
	public GameObject level5b;
	public GameObject level6a;
	public GameObject level6b;
	public GameObject level6c;
	public GameObject level7a;
	public GameObject level7b;
	public GameObject level7c;
	public GameObject level8a;
	public GameObject level8b;
	public GameObject level9a;
	public GameObject level9b;
	public List<GameObject> treasures;

	[Header ("Set bonus treasure icon here")]
	public GameObject lvl2;
	public GameObject lvl4;
	public GameObject lvl5;
	public GameObject lvl6;
	public GameObject lvl7;
	public GameObject lvl8;
	public GameObject lvl9;
	public List<GameObject> secrets;

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

		for (int i = 0; i < treasures.Count; i++)
		{
			if (GameManager.manager.treasureCollected[i + 3])
				treasures[i].GetComponent<Image>().sprite = GameManager.manager.GetComponent<TreasureList>().treasures[i];
		}
		for (int i = 0; i < secrets.Count; i++)
		{
			if (i >= secrets.Count) continue;
			if (GameManager.manager.secrettreasureCollected[i])
				secrets[i].GetComponent<Image>().sprite = GameManager.manager.GetComponent<TreasureList>().secrets[i];
		}
	}
}
