using UnityEngine;
using System.Collections;

public class Crumbling_Platform_Controller : MonoBehaviour
{
	Animator animate = null;
	bool dieing = false;
	float deathTimer = 1.25f;
	public GameObject HardSurface = null;
	
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
	
	void OnTriggerEnter(Collider _obj)
	{
		if (_obj.tag == "Player")
			animate.Play("Cracked");
	}
	
	void OnTriggerExit(Collider _obj)
	{
		if (_obj.tag == "Player")
		{
			dieing = true;	
			animate.Play("Breaking");
			GetComponent<BoxCollider>().enabled = false;
			HardSurface.SetActive(false);
		}
	}
	
}
