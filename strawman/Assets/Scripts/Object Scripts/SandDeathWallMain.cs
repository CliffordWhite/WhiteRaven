using UnityEngine;
using System.Collections;

public class SandDeathWallMain : MonoBehaviour {

	public GameObject[] SandDeathWalls;
	public float[] ActivationDelays;
	float TimeActive = 0.0f;
	bool Done = false;
	void Start ()
	{
	
	}

	void Update ()
	{
		TimeActive += Time.deltaTime;
		Done = true;
		for (int i = 0; i < ActivationDelays.Length; i++)
		{
			if (TimeActive >= ActivationDelays[i])
			{
				SandDeathWalls[i].SetActive(true);
			}
			if (!SandDeathWalls[i].activeSelf)
				Done = false;
		}
		if (Done)
			gameObject.SetActive (false);
	}
}
