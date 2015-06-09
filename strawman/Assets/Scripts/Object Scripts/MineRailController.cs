using UnityEngine;
using System.Collections;

public class MineRailController : MonoBehaviour
{

	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}
	
	void OnCollisionStay(Collision _obj)
	{
//		if( _obj.transform.tag == "MineCart" && _obj.transform.GetComponent<MineCartController>().Enabled == true )
//		{
//			_obj.transform.GetComponent<Rigidbody>().velocity =  transform.right * 10.0f;
//		}
	}
}
