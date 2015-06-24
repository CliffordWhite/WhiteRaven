using UnityEngine;
using System.Collections;

public class Landing : MonoBehaviour 
{
	public AudioSource sfxManager;
	public AudioClip landing;
	public ParticleSystem dust;
	public float landVel;
	public GameObject player;

	void Start()
	{
		if (player == null)
			player = GameObject.FindWithTag("Player");
	}

	void OnTriggerEnter(Collider other)
	{
		if (player.GetComponent<Rigidbody>().velocity.y < landVel && (other.tag == "Floor" || other.tag == "Wall"))
		{

			sfxManager.PlayOneShot(landing);
			Object thing = Instantiate(dust, transform.position, transform.rotation);
			Destroy(thing, 0.5f);
		}
	}


}
