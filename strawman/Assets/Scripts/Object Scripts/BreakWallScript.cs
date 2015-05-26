using UnityEngine;
using System.Collections;

public class BreakWallScript : MonoBehaviour 
{
	public int hp;						// how many hits to break
	public AudioSource sfxSource;		// sfx source for volume
	public AudioClip hitSound;			// sound to play when hit
	public ParticleSystem breakEffect;	// particle system to play when hit

	private int fullHP;					// keep track of full HP for scale ratio
	private Vector3 origScale;			// keep track of original scale for shrinking on hit

	void Start () 
	{
		fullHP = hp;
		origScale = transform.localScale;
	}

	void OnCollisionEnter(Collision other)
	{
		// only do work if hit by projectile
		if (other.collider.tag == "Projectile")
		{
			// play sound and particles for each contact point
			foreach (ContactPoint contact in other.contacts)
			{
				sfxSource.PlayOneShot(hitSound);
				Instantiate(breakEffect, contact.point, transform.rotation);
			}
			hp--;								// decrement health

			// if hp is 0, destroy the object
			if (hp == 0)
			{
				Instantiate (breakEffect, new Vector3(transform.position.x, transform.position.y+2.0f, 0.0f), transform.rotation);
				Instantiate (breakEffect, new Vector3(transform.position.x, transform.position.y-2.0f, 0.0f), transform.rotation);
				Destroy(gameObject);
				return;
			}
			float ratio = ((float)hp/(float)fullHP);	// get ratio of current to full
			transform.localScale = new Vector3(origScale.x * ratio, origScale.y, origScale.z);
			//Color.black + ratio;
			//GetComponent<SpriteRenderer>().color = new Color(ratio, ratio, ratio, 255.0f);	// set color to darken
		}
	}
	
}
