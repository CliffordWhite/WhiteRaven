using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public float speed;			// speed of projectile
	public Vector3 velocity;	// values determine direction
	public GameObject explode;	// particle system for explosion

	//void Start () 
	//{
	//	gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (velocity.x, velocity.y, 0.0f);
	//}

	void Start () 
	{
		velocity = transform.right;
	}
	
	void Update () 
	{
		transform.position += velocity * speed * Time.deltaTime;
	}

	void OnCollisionEnter(Collision other)
	{
		// check all tags that this object will destroy on
		if (other.collider.tag == "Wall" || other.collider.tag == "Player")
		{
			Instantiate(explode, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}
