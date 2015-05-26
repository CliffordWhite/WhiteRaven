using UnityEngine;
using System.Collections;

public class FireDeathWall : MonoBehaviour 
{
	public AudioSource sfxSource;	// use source to play clip
	public AudioClip fireSound;		// fire crackle
	public Transform exitDoor;		// used to calculate where the fire will move towards
	public float speed;				// how long in seconds it will take to get to the exit
	// currently public for debuging
	public Vector3 fireStart;		// keep track of origin point

	private float startTime;		// used to reference when the wall starts moving
	private float musicTime;		// used to loop the fire sound if necessary
	private Vector3 fireEnd;		// keep track of end point

	void Start()
	{
		//fireStart = transform.position;
		fireEnd = new Vector3 (fireStart.x, exitDoor.position.y, 0.0f);
	}
	void Update () 
	{
		float i = (Time.time - startTime)/speed; // ratio of time elapsed to overall time to complete
		transform.position = Vector3.Lerp(fireStart,fireEnd,i);

		// replay the sound effect as necessary
		if (Time.time - musicTime >= fireSound.length)
			sfxSource.PlayOneShot(fireSound, 0.5f);
	}

	// used to initialize the death wall
	public void BringThePain()
	{
		startTime = musicTime = Time.time;
		transform.position = fireStart;
		sfxSource.PlayOneShot(fireSound, 0.5f);

	}
}
