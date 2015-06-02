using UnityEngine;
using System.Collections;

public class FallingWall : MonoBehaviour
{

	[Header ("Z Direction will default to 0 regardless")]
	[Header ("Start with small numbers like 0.01")]
	public Vector3 FallSpeed = new Vector3(0, -0.01f, 0);
	Vector3 rayCastDirection = Vector3.zero;
	float rayCastDist = 0.0f;
	// Used for raycast to detect walls.
	LayerMask levelStuff = 256;
	bool Moving = true;

	void Start ()
	{
		FallSpeed.z = 0.0f;
		if( FallSpeed.x < 0 )
			rayCastDirection.x = -1;
		else if( FallSpeed.x > 0 )
				rayCastDirection.x = 1;
		
		if( FallSpeed.y < 0 )
			rayCastDirection.y = -1;
		else if( FallSpeed.y > 0 )
			rayCastDirection.y = 1;
	}
	
	void Update ()
	{
		
	}
	
	void FixedUpdate ()
	{
		if (Moving)
			transform.position += FallSpeed;
	}
	
	void OnCollisionStay(Collision _obj)
	{
		if( _obj.gameObject.layer == 8 )
			Moving = false;
		if (Moving && _obj.transform.tag == "Player")
		{
			Vector3 rayOrigin = new Vector3();
			rayOrigin.x = _obj.transform.position.x;
			rayOrigin.z = _obj.transform.position.z;
			rayOrigin.y = transform.position.y;
			if (rayCastDirection.x != 0)
			{
				if (rayOrigin.y < transform.position.y - transform.GetComponent<BoxCollider>().size.y * 0.5f * transform.localScale.y
				    || rayOrigin.y > transform.position.y + transform.GetComponent<BoxCollider>().size.y * 0.5f * transform.localScale.y)
					return;
				rayCastDist = _obj.transform.GetComponent<BoxCollider>().size.x;
				if (rayCastDirection.x < 0)
					rayOrigin.y -= transform.GetComponent<BoxCollider>().size.x * 0.5f * transform.localScale.x;
				else if (rayCastDirection.x > 0)
					rayOrigin.y += transform.GetComponent<BoxCollider>().size.x * 0.5f * transform.localScale.x;
			}
			else if (rayCastDirection.y != 0)
			{
				if (rayOrigin.x < transform.position.x - transform.GetComponent<BoxCollider>().size.x * 0.5f * transform.localScale.x
				    || rayOrigin.x > transform.position.x + transform.GetComponent<BoxCollider>().size.x * 0.5f * transform.localScale.x)
					return;
				rayCastDist = _obj.transform.GetComponent<BoxCollider>().size.y;
				if (rayCastDirection.y < 0)
					rayOrigin.y -= transform.GetComponent<BoxCollider>().size.y * 0.5f * transform.localScale.y;
				else if (rayCastDirection.y > 0)
					rayOrigin.y += transform.GetComponent<BoxCollider>().size.y * 0.5f * transform.localScale.y;
				
			}		
			
			
			if (Physics.Raycast(rayOrigin, rayCastDirection, rayCastDist, levelStuff))
				_obj.transform.SendMessage("KillPlayer");
		}
	}
	
	void Trigger()
	{
		Moving = true;	
	}
}
