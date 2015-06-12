using UnityEngine;
using System.Collections;

public class SandDeathWall : MonoBehaviour
{

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
			_obj.GetComponent<PlayerController>().IsInSand = true;
		}
		else if( _obj.tag == "SandDepth" )
		{
			_obj.transform.parent.SendMessage("KillPlayer");
		}
	}
	
	void OnTriggerExit(Collider _obj)
	{
		if( _obj.tag == "Player" )
		{
			_obj.GetComponent<PlayerController>().IsInSand = false;
		}
	}
}
