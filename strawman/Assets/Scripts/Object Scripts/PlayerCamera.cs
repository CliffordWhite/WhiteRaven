using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{
	public GameObject target;	// player to follow with camera
	public float camDist;		// pull camera away from player
	public float w, t;			// formatting numbers for timer
	public GUIStyle style;		// allow for large font for timer

	void Update () 
	{
		// follow the player!
		transform.position = new Vector3 (target.transform.position.x,
		                                 target.transform.position.y,
		                                 target.transform.position.z-camDist);

		// if it's time attack mode, increment the timer for gameplay!
		if (GameManager.manager.timeAttackOn && Application.loadedLevel >= 6)
			GameManager.manager.gameTime += Time.deltaTime;
	}

	void OnGUI()
	{
		if (GameManager.manager.timeAttackOn && Application.loadedLevel >= 6)
		{
			// lots of Math
			float min = Mathf.Floor(GameManager.manager.gameTime / 60);
			float sec = (GameManager.manager.gameTime % 60);
			// "#" only writes number if there is one to write, "0" writes a placeholder zero
			GUI.Label(new Rect((Screen.width - w) / 2 ,t,200,200), min.ToString("#0:") + sec.ToString("0#.0"), style);
		}
	}
}
