using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public float speed;			// speed of projectile
	public GameObject explode;	// particle system for explosion
	private Vector3 direction;	// defaults to the right

	void Start () 
	{
		direction = transform.right; 
	}
	
	void Update () 
	{
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnCollisionEnter(Collision other)
	{
		// check all tags that this object will destroy on
		if (other.collider.tag == "Wall" || other.collider.tag == "Player")
		{
			Instantiate(explode, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		// deflect from all of these tags
		else if (other.collider.tag == "Shield")
		{
			foreach(ContactPoint contact in other.contacts)
			{
				// making Bahin proud
				direction = 2 * (Vector3.Dot (direction, Vector3.Normalize (contact.normal)))*Vector3.Normalize(contact.normal) - direction;
				direction *= -1;
				direction.z = 0.0f;
			}
		}

	}
}
