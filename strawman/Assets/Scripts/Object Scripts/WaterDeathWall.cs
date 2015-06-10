using UnityEngine;
using System.Collections;

public class WaterDeathWall : MonoBehaviour {

	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}
	
	void OnTriggerEnter(Collider _obj)
	{
		if( _obj.tag == "Player" )
		{
			_obj.GetComponent<PlayerController>().WaterMode = true;	
		}
	}
	
	void OnTriggerExit(Collider _obj)
	{
		if( _obj.tag == "Player" )
		{
			_obj.GetComponent<PlayerController>().WaterMode = false;	
		}
	}
}
