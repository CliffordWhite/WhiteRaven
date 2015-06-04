using UnityEngine;
using System.Collections;

public class Bucket_Breaker : MonoBehaviour {

	void OnTriggerEnter(Collider _Obj)
	{
		if( _Obj.tag == "Projectile" )
			_Obj.gameObject.SendMessage("Kill");
	}
}
