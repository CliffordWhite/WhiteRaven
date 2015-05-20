using UnityEngine;
using System.Collections;

public class SkullProjectile : MonoBehaviour 
{
	public float speed;			// speed of projectile
	public GameObject explode;	// particle system for explosion
	public float spinSpeed;		// positive for counter-clockwise, negative for clockwise
	private Vector3 direction;	// defaults to the right
	
	void Start () 
	{
		direction = transform.right; 
	}
	
	void Update () 
	{
		transform.position += direction * speed * Time.deltaTime;
		transform.Rotate (0.0f, 0.0f, spinSpeed * Time.deltaTime);	//give spinning for effect
	}
	
	void OnCollisionEnter(Collision other)
	{
		// list all possible items with which this can collide
		if (other.collider.tag == "Wall" || other.collider.tag == "Shield" || other.collider.tag == "Player" ||
		    other.collider.tag == "ReflectiveWall")
		{
			Instantiate(explode, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}
