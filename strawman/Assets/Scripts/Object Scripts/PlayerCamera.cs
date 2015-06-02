using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{
	public enum CameraMode{
		Delayed,
		BoundingBox,
		RecenteringBoundingBox,
		Centered
	}
	[Header ("They way the camera will follow it's Target.")]
	public CameraMode CameraStyle = CameraMode.Centered;
	public GameObject target;	// player to follow with camera
	public float camDist;		// pull camera away from player
	public float w, t;			// formatting numbers for timer
	public Texture lifeIcon;	// icon for how many lives player has	
	public GUIStyle style;		// allow for large font for timer

	public Texture KeyTexture;


	float showTime = 2.0f; 		// how long to show lives before hiding

	[Header ("Used With Delayed Camera Style.")]
	public float moveDelay = 1.0f;
	Vector3 moveDirection = Vector3.zero;
	Vector3 ZeroVector = Vector3.zero;

	[Header ("Used With Recentering Bounding Box Camera Style.")]
	public float camMoveSpeed = 0.2f;
	bool Moving = false;

	void Start()
	{
		Vector3 newPosition = target.transform.position;
		newPosition.z = transform.position.z;
		transform.position = newPosition;
	}
	
	void Update () 
	{
		// if it's time attack mode, increment the timer for gameplay!
		if (GameManager.manager.timeAttackOn && Application.loadedLevel >= 6)
			GameManager.manager.gameTime += Time.deltaTime;

	}

	void FixedUpdate()
	{
		if (CameraStyle == CameraMode.Delayed)
		{
			Vector3 focus = Camera.main.WorldToViewportPoint(target.transform.position);
			Vector3 smoothZone = target.transform.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, focus.z));
			Vector3 moveTo = transform.position + smoothZone;
			transform.position = Vector3.SmoothDamp(transform.position, moveTo, ref ZeroVector, moveDelay);
		}
		else if (CameraStyle == CameraMode.BoundingBox)
		{
			Vector3 camPos = new Vector3(transform.position.x, transform.position.y, target.transform.position.z);
			
			if (!Moving && (target.transform.position - camPos).magnitude >= 6.0f)
				Moving = true;
			if (Moving && (target.transform.position - camPos).magnitude <= 5.9f)
				Moving = false;
			
			else if (Moving)
			{
				moveDirection = (target.transform.position - camPos).normalized * camMoveSpeed;
				// follow the player!
				transform.position += moveDirection;
			}
		}
		else if (CameraStyle == CameraMode.RecenteringBoundingBox)
		{
			Vector3 camPos = new Vector3(transform.position.x, transform.position.y, target.transform.position.z);
			
			if (!Moving && (target.transform.position - camPos).magnitude >= 6.0f)
			{
				Moving = true;
			}
			else if (Moving)
			{
				moveDirection = (target.transform.position - camPos).normalized * camMoveSpeed;
				// follow the player!
				transform.position += moveDirection;
				
				if ((target.transform.position - camPos).magnitude < 0.1f)
					Moving = false;
			}
		}
		else if (CameraStyle == CameraMode.Centered)
		{
			Vector3 newPosition = target.transform.position;
			newPosition.z = transform.position.z;
			transform.position = newPosition;
		}
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
		if (GameManager.manager.achievePopTime > 0.0f) {
			GUI.Label (new Rect((Screen.width/2)-128, Screen.height-64, 128,64),GameManager.manager.achievePopString, style);
			GameManager.manager.achievePopTime -= Time.deltaTime;
		}
	}
}
