using UnityEngine;
using System.Collections;

public class Attach_Player : MonoBehaviour {

	void Start () {
	
	}

	void Update () {
	
	}

	void OnCollisionEnter(Collision _Obj)
	{
		if (_Obj.transform.tag == "Player")
			_Obj.transform.parent = transform;
	}

	void OnCollisionExit(Collision _Obj)
	{
		if (_Obj.transform.tag == "Player")
			_Obj.transform.parent = null;
	}
}
