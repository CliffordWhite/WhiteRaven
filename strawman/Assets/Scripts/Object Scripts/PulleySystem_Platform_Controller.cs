using UnityEngine;
using System.Collections;

public class PulleySystem_Platform_Controller : MonoBehaviour {

	GameObject Parent = null;
	void Start ()
	{
		Parent = transform.parent.gameObject;
	}
	
	void OnTriggerEnter(Collider _Obj)
	{
		if( _Obj.tag == "Player" )
		{
			Parent.SendMessage("GoForward", true);
			Parent.GetComponent<RopePulleySystem>().Moving = true;
		}
	}
	
	void OnTriggerExit(Collider _Obj)
	{
		if( _Obj.tag == "Player" )
		{
			Parent.SendMessage("GoForward", false);
		}
	}
}
