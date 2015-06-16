using UnityEngine;
using System.Collections;

public class BatControllerAnimated : MonoBehaviour 
{
	public Animator animate;			// animator controler
	public Transform[] waypoints;		// where the bat will travel to
	int wpIndex = 0;					// where in the array of waypoints the bat is in
	Vector3 startPos, endPos;			// the two waypoints the bat is between

	public float stayTime = 1.0f;		// how long to stay at waypoint
	public float idleTimer = 0.0f;		// for comparing against stay timer
	public float speed = 2.0f;			// how fast for bast to move
	
	float startTime; 					// keep track of when a bat moved

	void Start () 
	{
		startPos = transform.position = waypoints[wpIndex].position;
		if (animate == null)
			animate = transform.GetComponentInChildren<Animator>();
		if (gameObject.tag == "HM Fatal" && !GameManager.manager.hardModeOn)
			gameObject.SetActive(false);
	}
	
	void Update () 
	{
		if (idleTimer <= stayTime)
		{
			idleTimer += Time.deltaTime;
			if (idleTimer > stayTime)
			{
				startTime = Time.time;
				wpIndex++;
				if (wpIndex == waypoints.Length)
					wpIndex = 0;
				endPos = waypoints[wpIndex].position;
				DirectionCheck();
			}
		}
		else
		{
			float i = (Time.time - startTime) / speed;
			transform.position = Vector3.Lerp(startPos, endPos, i);
			if (i >= 1)
			{
				idleTimer = 0.0f;
				startPos = endPos;
			}
		}
	}

	void DirectionCheck()
	{
		// get a direction vector to know which way bat is flying
		Vector2 direction = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

		// play the appropriate animation for which way the bat is flying
		if (direction.x > 0.0f && direction.y < 0.01f)
			animate.Play("FlyRight");
		else if (direction.x < 0.0f && direction.y < 0.01f)
			animate.Play("FlyLeft");
		else if (direction.y > 0.0f && direction.x < 0.01f)
			animate.Play("FlyUp");
		else if (direction.y < 0.0f && direction.x < 0.01f)
			animate.Play("FlyDown");
	}
}
