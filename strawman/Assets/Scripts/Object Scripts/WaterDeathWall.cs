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
		if (_obj.tag == "PlayerBody")
		{
			_obj.transform.parent.GetComponent<PlayerController> ().WaterMode = 1;	
		}
		else if (_obj.tag == "PlayerHead")
		{
			_obj.transform.parent.GetComponent<PlayerController> ().isDrowning = true;
		}
	}
	
	void OnTriggerExit(Collider _obj)
	{
		if( _obj.tag == "PlayerBody" )
		{
			_obj.transform.parent.GetComponent<PlayerController>().WaterMode = -1;	
		}
		else if (_obj.tag == "PlayerHead")
		{
			_obj.transform.parent.GetComponent<PlayerController> ().isDrowning = false;
		}
	}
}
