using UnityEngine;
using System.Collections;

public class Crumbling_Platform_Controller : MonoBehaviour
{
	Animator animate = null;
	bool dieing = false;
	bool OnCollider = false;
	bool OnTrigger = false;
	bool breaking = false;
	float deathTimer = 2.5f;
	public GameObject HardSurface = null;
	public AudioSource SFXPlayer = null;
	public AudioClip Cracking = null;
	public AudioClip Crumbling = null;
	
	void Start ()
	{
		animate = transform.GetComponent<Animator>();
	}
	
	void Update ()
	{
		if (dieing)
		{
			deathTimer -= Time.deltaTime;
			if (deathTimer <= 0.0f)
				Destroy(gameObject);
		}
	}

	void FixedUpdate()
	{
		if (!breaking && OnTrigger && OnCollider)
		{
			breaking = true;
			animate.Play("Cracked");
			SFXPlayer.PlayOneShot(Cracking);
		}
	}
	
	void OnTriggerEnter(Collider _obj)
	{
		if( _obj.tag == "Player" )
		{
			OnTrigger = true;
		}
	}
	
	void OnTriggerExit(Collider _obj)
	{
		if (_obj.tag == "Player")
		{
			OnTrigger = false;
		}
	}
	
	void collided(bool _collided)
	{
		OnCollider = _collided;
		if(breaking && !_collided )
		{
			dieing = true;
			animate.Play("Breaking");
			SFXPlayer.PlayOneShot(Crumbling);
			GetComponent<BoxCollider>().enabled = false;
			HardSurface.SetActive(false);
			gameObject.AddComponent<Rigidbody>();
			transform.GetComponent<Rigidbody>().useGravity = true;
		}
	}
	
}
