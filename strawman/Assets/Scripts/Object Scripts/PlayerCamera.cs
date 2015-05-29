using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{
	public GameObject target;	// player to follow with camera
	public float camDist;		// pull camera away from player
	public float w, t;			// formatting numbers for timer
	public Texture lifeIcon;	// icon for how many lives player has	
	public GUIStyle style;		// allow for large font for timer

	public Texture KeyTexture;

	float showTime = 2.0f; 		// how long to show lives before hiding

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
		// Show timer if toggled on and on an actual level, 6 is currently level 1
		if (GameManager.manager.timeAttackOn && Application.loadedLevel >= 6)
		{
			// lots of Math
			float min = Mathf.Floor(GameManager.manager.gameTime / 60);
			float sec = (GameManager.manager.gameTime % 60);
			// "#" only writes number if there is one to write, "0" writes a placeholder zero
			GUI.Label(new Rect((Screen.width - w) / 2 ,t,200,200), min.ToString("#0:") + sec.ToString("0#.0"), style);
		}
		// Show lives for a brief moment
		if (GameManager.manager.hardModeOn && showTime > Time.timeSinceLevelLoad)
		{
			GUI.Label (new Rect(Screen.width - lifeIcon.width * 5, lifeIcon.height / 2, 200,200), GameManager.manager.lives.ToString("##") + "x", style);
			GUI.DrawTexture(new Rect(Screen.width - lifeIcon.width * 2, lifeIcon.height / 2, lifeIcon.width, lifeIcon.height), lifeIcon);
		}
		//Show Keys for a brief moment
		if (GameManager.manager.keyShowTime > 0.0f) {
			GUI.Label (new Rect(KeyTexture.width, KeyTexture.height / 2, 64,64), "x" + GameManager.manager.keys.ToString("##"), style);
			GUI.DrawTexture(new Rect(0, KeyTexture.height / 2, KeyTexture.width, KeyTexture.height), KeyTexture);
			GameManager.manager.keyShowTime -= Time.deltaTime;
		}
	}
}
