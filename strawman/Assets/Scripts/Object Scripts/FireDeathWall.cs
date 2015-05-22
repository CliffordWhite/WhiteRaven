using UnityEngine;
using System.Collections;

public class FireDeathWall : MonoBehaviour 
{
	public Transform exitDoor;		// used to calculate where the fire will move towards
	public float speed;				// how long in seconds it will take to get to the exit

	private float startTime;		// used to reference when the wall starts moving
	private Vector3 fireStart;		// keep track of origin point
	private Vector3 fireEnd;		// keep track of end point

	void Start()
	{
		fireStart = transform.position;
		fireEnd = new Vector3 (fireStart.x, exitDoor.position.y, 0.0f);
	}
	void Update () 
	{
		float i = (Time.time - startTime)/speed; // ratio of time elapsed to overall time to complete
		transform.position = Vector3.Lerp(fireStart,fireEnd,i);
	}

	// used to initialize the death wall
	public void BringThePain()
	{
		startTime = Time.time;
		transform.position = fireStart;
	}
}
