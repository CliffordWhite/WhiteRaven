using UnityEngine;
using System.Collections;

public class Bucket_Collector : MonoBehaviour {

	public int RequiredCannonBalls = 1;
	int CurrentCannonBalls = 0;
	bool Moving = false;
	bool Finished = false;
	float Distance = 0.0f;
	public AudioClip SFX = null;
	public AudioSource SFXPlayer = null;
	
	void Start()
	{
		Distance = ( transform.parent.GetComponent<RopePulleySystem>().EndPosOne - transform.localPosition ).magnitude;
		SFXPlayer = GetComponent<AudioSource>();
	}
	
	void Update()
	{
		if(!Finished)
		{
			if (CurrentCannonBalls >= RequiredCannonBalls )
			{
				transform.parent.GetComponent<RopePulleySystem>().Moving = true;
				Finished = true;
			}
			
			if(Moving && (transform.parent.GetComponent<RopePulleySystem>().EndPosOne - transform.localPosition).magnitude <= Distance * (1.0f - (float)CurrentCannonBalls / (float)RequiredCannonBalls))
			{
				transform.parent.GetComponent<RopePulleySystem>().Moving = false;
				Moving = false;
			}
		}
	}
	
	void OnTriggerEnter(Collider _Obj)
	{
		if( _Obj.tag == "Projectile" )
		{
			if (CurrentCannonBalls < 2)
			{
				_Obj.transform.parent = transform;
				_Obj.transform.localPosition = new Vector3(CurrentCannonBalls * 0.5f + -0.25f, 0.5f, 0.1f);
				Destroy(_Obj.transform.GetComponent<Projectile>());
				_Obj.transform.GetComponent<SphereCollider>().enabled = false;
			}
			else
				Destroy(_Obj.gameObject);
			if (!Finished)
			{
				SFXPlayer.PlayOneShot(SFX);
				CurrentCannonBalls ++;
				transform.parent.GetComponent<RopePulleySystem>().Moving = true;
				Moving = true;
			}
		}
	}
}
