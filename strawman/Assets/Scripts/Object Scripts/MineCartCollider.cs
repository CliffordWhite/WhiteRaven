using UnityEngine;
using System.Collections;

public class MineCartCollider : MonoBehaviour {

	public GameObject Target = null;
	
	void FixedUpdate()
	{
		transform.position = Target.transform.position;
		transform.rotation = Target.transform.rotation;
	}
	
	void OnTriggerEnter(Collider _obj)
	{
		if( _obj.tag == "Fatal" || _obj.tag == "HM Fatal" || _obj.tag == "Projectile")
		{
			Target.BroadcastMessage("KillPlayer");
		}
	}
}
