using UnityEngine;
using System.Collections;

public class Shield_Controller : MonoBehaviour {

	GameObject shield = null;
	Vector3 mousePos = Vector3.zero;
	
	void Start ()
	{
		shield = transform.FindChild ("Shield").gameObject;
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Mouse1))
			shield.SetActive(true);
		else if (Input.GetKeyUp(KeyCode.Mouse1))
			shield.SetActive(false);
	}
	
	void FixedUpdate()
	{
		if (shield.activeSelf)
		{
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = transform.parent.transform.position.z;
			transform.LookAt(mousePos);
		}
	}
}
