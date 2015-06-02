using UnityEngine;
using System.Collections;

public class Crumbling_Platform_Controller2 : MonoBehaviour {

	Transform parent = null;
	
	void Start()
	{
		parent = transform.parent.transform;	
	}
	
	void OnCollisionEnter(Collision _obj)
	{
		if( _obj.transform.tag == "Player" )
		{
			parent.SendMessage("collided", true);
		}
	}
	
	void OnCollisionExit(Collision _obj)
	{
		if( _obj.transform.tag == "Player" )
		{
			parent.SendMessage("collided", false);
		}
	}
}
