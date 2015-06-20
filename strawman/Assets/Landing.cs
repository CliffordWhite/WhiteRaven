using UnityEngine;
using System.Collections;

public class Landing : MonoBehaviour 
{
	public AudioSource sfxManager;
	public AudioClip landing;
	public ParticleSystem dust;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Floor" || other.tag == "Wall")
		{
			sfxManager.PlayOneShot(landing);
			Object thing = Instantiate(dust, transform.position, transform.rotation);
			Destroy(thing, 0.5f);
		}
	}


}
