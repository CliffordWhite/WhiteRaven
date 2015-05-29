using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public float speed;			// speed of projectile
	public GameObject explode;	// particle system for explosion
	private Vector3 direction;	// defaults to the right
	private bool deflected;		// only allows one bounce from shield

	void Start () 
	{
		direction = transform.right; 
		deflected = false;
	}
	
	void Update () 
	{
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnCollisionEnter(Collision other)
	{
		// check all tags that this object will destroy on
		if (other.collider.tag == "Wall" || other.collider.tag == "Player" || other.collider.tag == "Floor")
		{
			Instantiate(explode, transform.position, transform.rotation);
            if (other.collider.tag == "Player" && other.gameObject.GetComponent<PlayerController>().HasArmor)
            {
                other.gameObject.GetComponent<PlayerController>().HitWithArmor();
            }
			Destroy (gameObject);
		}
		// deflect from shield once
		else if (other.collider.tag == "Shield" && !deflected)
		{
			deflected = true;	// flag this true
			foreach(ContactPoint contact in other.contacts)
			{
				// making Bahin proud
				direction = 2 * (Vector3.Dot (direction, Vector3.Normalize (contact.normal)))*Vector3.Normalize(contact.normal) - direction;
				direction *= -1;
				direction.z = 0.0f;
			}
		}
        else if (other.collider.tag == "ReflectiveWall")
        {
            foreach (ContactPoint contact in other.contacts)
            {
                // making Bahin proud
                direction = 2 * (Vector3.Dot(direction, Vector3.Normalize(contact.normal))) * Vector3.Normalize(contact.normal) - direction;
                direction *= -1;
                direction.z = 0.0f;
            }
        }
	}
}
