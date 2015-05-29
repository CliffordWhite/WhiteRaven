using UnityEngine;
using System.Collections;

public class ComingSoon : MonoBehaviour 
{
	public GUIStyle style;

	void OnGUI()
	{
		GUI.Label(new Rect(100,100,200,200), "More levels coming soon!", style);
		if (GUI.Button(new Rect(100, 200, 100, 30), "Main Menu"))
		{
			Application.LoadLevel(0);
		}
	}
}
