using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour 
{
	public GameObject itemToFire;	// prefab to be created and fired from cannon
	public Transform spawnLoc;		// Transform for instantiating ball location
	public float fireRate;			// how often in seconds between firing
	public float delay;				// how long to wait firing, allow multiple cannons at same speed to be alternating
	public AudioClip fireSound;		// sound effect to play when firing
	public AudioSource masterVolume;// get volume settings

	float fireTimer;				// keep track of when to fire next


	void Update () 
	{
		if (Time.timeSinceLevelLoad > fireTimer && Time.timeSinceLevelLoad > delay) 
		{
			fireTimer = Time.timeSinceLevelLoad + fireRate;
			Instantiate(itemToFire, spawnLoc.position, spawnLoc.rotation);
			masterVolume.PlayOneShot(fireSound, 1.0f);
		}
	}
}
