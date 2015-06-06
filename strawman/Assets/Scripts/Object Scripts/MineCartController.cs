using UnityEngine;
using System.Collections;

public class MineCartController : MonoBehaviour {

	bool Enabled = false;
	GameObject Player = null;
	int tracks = 0;
	
	void FixedUpdate()
	{
		if( Enabled )
		{
			transform.position += transform.right * 0.2f;
//			if (tracks == 0)
//				transform.position += Vector3.down * 0.01f;
		}
		
		if (transform.eulerAngles.z > 30.0f && transform.eulerAngles.z <= 180.0f)
		{
			Vector3 fixRot = transform.eulerAngles;
			fixRot.z = 30.0f;
			transform.eulerAngles = fixRot;
		}
		else if (transform.eulerAngles.z < 330.0f && transform.eulerAngles.z >= 180.0f)
		{
			Vector3 fixRot = transform.eulerAngles;
			fixRot.z = 330.0f;
			transform.eulerAngles = fixRot;
		}
	}
	
	void OnTriggerEnter(Collider _obj)
	{
		if( !Enabled && _obj.transform.tag == "Player" )
		{
			Enabled = true;
			Player = _obj.transform.gameObject;
			Player.SendMessage("MineCartMode", transform);
		}
	}
	
	void OnCollisionEnter(Collision _obj)
	{
		if( _obj.transform.tag == "MineRail" )
			tracks++;
	}
	
	void OnCollisionExit(Collision _obj)
	{
		if( _obj.transform.tag == "MineRail" )
			tracks--;
	}
	
	public int OnTracks
	{
		get
		{
			return tracks;
		}
	}
	
	void LeaveCart()
	{
		GetComponent<BoxCollider>().enabled = false;
	}
}